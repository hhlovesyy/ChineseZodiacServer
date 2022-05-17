using System;
using System.Collections.Generic;

public class Room {
	//id
	public int id = 0;
	//最大玩家数
	public int maxPlayer = 5;
	//玩家列表
	public Dictionary<string, bool> playerIds = new Dictionary<string, bool>();
	//房主id
	public string ownerId = "";
	//状态
	public enum Status {
		PREPARE = 0,
		FIGHT = 1 ,
	}
	public Status status = Status.PREPARE;
    //出生点位置配置
    //出生点位置配置
    static float[,,] birthConfig = new float[5, 3, 6] {
		//阵营1出生点
		{
            {20.48343f, 34.061752f,2.543324f, 0f,0f,0f},//出生点1
			{-49.9f, 3.8f, -61.4f, 0, 21.4f, 0f},//出生点2
			{-6.2f,  3.8f, -70.7f, 0, 21.9f, 0f},//出生点3
		},
		//阵营2出生点
		{
            {11.42933f,34.061752f,-16.31121f, 0f, 0f, 0f},//出生点1
			{105f, 0f, 216.5f, 0, -156.8f, 0f},//出生点2
			{52.0f,0f, 239.2f, 0, -156.8f, 0f},//出生点3
		},
        //阵营1出生点
		{
            {16.30719f,34.061752f,-15.03763f,0f, 0f, 0f},//出生点1

            {-49.9f, 3.8f, -61.4f, 0, 21.4f, 0f},//出生点2
			{-6.2f,  3.8f, -70.7f, 0, 21.9f, 0f},//出生点3
		},
        //阵营1出生点
		{
            {-20.82579f,34.061752f,-1.150911f,0f, 0f, 0f},//出生点1
            
			{-49.9f, 3.8f, -61.4f, 0, 21.4f, 0f},//出生点2
			{-6.2f,  3.8f, -70.7f, 0, 21.9f, 0f},//出生点3
		},
        //阵营1出生点
		{
            {20.02537f, 14.061752f, -15.22326f, 0f,0f,0f},//出生点1
			{20.02537f, 4.061752f, -15.22326f, 0f,0f,0f},//出生点2
			{20.02537f, 4.061752f, -15.22326f, 0f,0f,0f},//出生点3
            
		},

    };
    //2022.5.8 新增:每个玩家的camp
    private int[] camps = new int[13];

    //上一次判断结果的时间
    private long lastjudgeTime = 0;

	//添加玩家
	public bool AddPlayer(string id){
		//获取玩家
		Player player = PlayerManager.GetPlayer(id);
		if(player == null){
			Console.WriteLine("room.AddPlayer fail, player is null");
			return false;
		}
		//房间人数
		if(playerIds.Count >= maxPlayer){
			Console.WriteLine("room.AddPlayer fail, reach maxPlayer");
			return false;
		}
		//准备状态才能加人
		if(status != Status.PREPARE){
			Console.WriteLine("room.AddPlayer fail, not PREPARE");
			return false;
		}
		//已经在房间里
		if(playerIds.ContainsKey(id)){
			Console.WriteLine("room.AddPlayer fail, already in this room");
			return false;
		}
		//加入列表
		playerIds[id] = true;
		//设置玩家数据
		player.camp = SwitchCamp();
		player.roomId = this.id;
		//设置房主
		if(ownerId == ""){
			ownerId = player.id;
		}
		//广播
		Broadcast(ToMsg());
		return true;
	}

	//分配阵营
	public int SwitchCamp() {
        //修改服务端的逻辑,分配小动物
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            for (int i = 1; i <= maxPlayer; i++)
            {
                if (camps[i] == 0)
                {
                    camps[i] = 1;
                    return i;
                }
            }
        }
        return 0;
    }

	//是不是房主
	public bool isOwner(Player player){
		return player.id == ownerId;
	}

	//删除玩家
	public bool RemovePlayer(string id) {
		//获取玩家
		Player player = PlayerManager.GetPlayer(id);
		if(player == null){
			Console.WriteLine("room.RemovePlayer fail, player is null");
			return false;
		}
		//没有在房间里
		if(!playerIds.ContainsKey(id)){
			Console.WriteLine("room.RemovePlayer fail, not in this room");
			return false;
		}
		//删除列表
		playerIds.Remove(id);
        //2022.5.8 删除这个玩家的小动物信息
        camps[player.camp] = 0;
        //设置玩家数据
        player.camp = 0;
		player.roomId = -1;
		//设置房主
		if(ownerId == player.id){
			ownerId = SwitchOwner();
		}
		//战斗状态退出
		if(status == Status.FIGHT){
			player.data.lost++;
			MsgLeaveBattle msg = new MsgLeaveBattle();
			msg.id = player.id;
			Broadcast(msg);
		}
		//房间为空
		if(playerIds.Count == 0){
			RoomManager.RemoveRoom(this.id);
		}
		//广播
		Broadcast(ToMsg());
		return true;
	}

	//选择房主
	public string SwitchOwner() {
		//选择第一个玩家
		foreach(string id in playerIds.Keys) {
			return id;
		}
		//房间没人
		return "";
	}


	//广播消息
	public void Broadcast(MsgBase msg){
		foreach(string id in playerIds.Keys) {
			Player player = PlayerManager.GetPlayer(id);
			player.Send(msg);
		}
	}



	//生成MsgGetRoomInfo协议
	public MsgBase ToMsg(){
		MsgGetRoomInfo msg = new MsgGetRoomInfo();
		int count = playerIds.Count;
		msg.players = new PlayerInfo[count];
        msg.roomId = id; //2022.5.8新增:房间的编号信息,为了显示在Text上
		//players
		int i = 0;
		foreach(string id in playerIds.Keys){
			Player player = PlayerManager.GetPlayer(id);
			PlayerInfo playerInfo = new PlayerInfo();
			//赋值
			playerInfo.id = player.id;
			playerInfo.camp = player.camp;
			playerInfo.win = player.data.win;
			playerInfo.lost = player.data.lost;
			playerInfo.isOwner = 0;
			if(isOwner(player)){
				playerInfo.isOwner = 1;
			}

			msg.players[i] = playerInfo;
			i++;
		}
		return msg;
	}

	//能否开战
	public bool CanStartBattle() {
		//已经是战斗状态
		if (status != Status.PREPARE){
			return false;
		}
		//统计每个队伍的玩家数
		int count1 = 0;
		int count2 = 0;
		foreach(string id in playerIds.Keys) {
			Player player = PlayerManager.GetPlayer(id);
			if(player.camp == 1){ count1++; }
			else { count2++; }
		}
		//每个队伍至少要有1名玩家
		if (count1 < 1 || count2 < 1){
			return false;
		}
		return true;
	}

	//初始化位置
	private void SetBirthPos(Player player, int index){
		int camp = player.camp;

		player.x  = birthConfig[camp-1, index,0];
		player.y  = birthConfig[camp-1, index,1];
		player.z  = birthConfig[camp-1, index,2];
		player.ex = birthConfig[camp-1, index,3];
		player.ey = birthConfig[camp-1, index,4];
		player.ez = birthConfig[camp-1, index,5];
	}

	//玩家数据转成AnimalInfo
	public AnimalInfo PlayerToAnimalInfo(Player player){
		AnimalInfo animalInfo = new AnimalInfo();
		animalInfo.camp = player.camp;
		animalInfo.id = player.id;
		animalInfo.hp = player.hp;

		animalInfo.x  = player.x;
		animalInfo.y  = player.y;
		animalInfo.z  = player.z;
		animalInfo.ex = player.ex;
		animalInfo.ey = player.ey;
		animalInfo.ez = player.ez;

		return animalInfo;
	}

    //重置玩家战斗属性
    private void ResetPlayers()
    {
        //位置和旋转
        int count1 = 0;
        int count2 = 0;
        int count3 = 0;
        int count4 = 0;
        int count5 = 0;
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            if (player.camp == 1)
            {
                SetBirthPos(player, count1);
                count1++;
            }
            else if (player.camp == 2)
            {
                SetBirthPos(player, count2);
                count2++;
            }
            else if (player.camp == 3)
            {
                SetBirthPos(player, count3);
                count3++;
            }
            else if (player.camp == 4)
            {
                SetBirthPos(player, count4);
                count4++;
            }
            else if (player.camp == 5)
            {
                SetBirthPos(player, count4);
                count5++;
            }
        }
        //生命值
        foreach (string id in playerIds.Keys)
        {
            Player player = PlayerManager.GetPlayer(id);
            player.hp = 100;
        }
    }

    //开战
    public bool StartBattle() {
		if(!CanStartBattle()){
			return false;
		}
		//状态
		status = Status.FIGHT;
		//玩家战斗属性
		ResetPlayers();
		//返回数据
		MsgEnterBattle msg = new MsgEnterBattle();
		msg.mapId = 1;
		msg.animals = new AnimalInfo[playerIds.Count];

		int i=0;
		foreach(string id in playerIds.Keys) {
			Player player = PlayerManager.GetPlayer(id);
			msg.animals[i] = PlayerToAnimalInfo(player);
			i++;
		}
		Broadcast(msg);
		return true;
	}


	//是否死亡
	public bool IsDie(Player player){
		return player.hp <= 0;
	}


	//定时更新
	//public void Update(){
	//	//状态判断
	//	if(status != Status.FIGHT){
	//		return;
	//	}
	//	//时间判断
	//	if(NetManager.GetTimeStamp() - lastjudgeTime < 10f){
	//		return;
	//	}
	//	lastjudgeTime = NetManager.GetTimeStamp();
	//	//胜负判断
	//	int winCamp = Judgment();
	//	//尚未分出胜负
	//	if(winCamp == 0){
	//		return;
	//	}
	//	//某一方胜利，结束战斗
	//	status = Status.PREPARE;
	//	//统计信息
	//	foreach(string id in playerIds.Keys) {
	//		Player player = PlayerManager.GetPlayer(id);
	//		if(player.camp == winCamp){player.data.win++;}
	//		else{player.data.lost++;}
	//	}
	//	//发送Result
	//	MsgBattleResult msg = new MsgBattleResult();
	//	msg.winCamp = winCamp;
	//	Broadcast(msg);
	//}

	//胜负判断
	public int Judgment(){
		//存活人数
		int count1 = 0;
		int wincamp=0;
		//int count2 = 0;
		foreach(string id in playerIds.Keys) {
			Player player = PlayerManager.GetPlayer(id);
			if(!IsDie(player)){
				//if(player.camp == 1){count1++;};
				//if(player.camp == 2){count2++;};
				count1++;
				wincamp = player.camp;
			}
		}
        if (count1 == 1)
        {
            return wincamp;
        }
        ////判断
        //if(count1 <= 0){
        //	return 2;
        //}
        //else if(count2 <= 0){
        //	return 1;
        //}
        return 0;
	}
	public void MakeOtherAnimalsLost(string winid)//当其中一个人拿到钥匙的时候其他人输了
	{
		
		foreach (string id in playerIds.Keys)
		{
			if(id!=winid)
            {
				Player player = PlayerManager.GetPlayer(id);
				player.hp = -1;
			}
		}

		return;
	}
	public void OneWin(string winid)
    {
		//状态判断
		if (status != Status.FIGHT)
		{
			return;
		}

		//某一方胜利，结束战斗
		//status = Status.PREPARE;
		status = Status.PREPARE;

		//统计信息
		foreach (string id in playerIds.Keys)
		{
			Player player = PlayerManager.GetPlayer(id);
			//if (player.camp == winCamp) 
			if (player.id == winid)
			{ player.data.win++; }
			else { player.data.lost++; }
		}
		//发送Result
		MsgBattleResult msg = new MsgBattleResult();
		//msg.winCamp = winCamp;
		msg.winId = winid;
		Broadcast(msg);
	}
}

