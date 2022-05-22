
//玩家信息
[System.Serializable]
public class AnimalInfo{
	public string id = "";	//玩家id
	public int camp = 0;	//阵营
	public int hp = 0;		//生命值

	public float x = 0;		//位置
	public float y = 0;
	public float z = 0;
	public float ex = 0;	//旋转
	public float ey = 0;
	public float ez = 0;
}

//物品信息,发送到客户端的
[System.Serializable]
public class ItemInfo
{
	public int id; //道具的编号,1-maxboxNum
    public float x;
    public float y;
    public float z;
    public int kind; //表示这是哪种道具,指道具的类型
}


//进入战场（服务端推送）
public class MsgEnterBattle:MsgBase {
	public MsgEnterBattle() {protoName = "MsgEnterBattle";}
	//服务端回
	public AnimalInfo[] animals;

	//0519
	public ItemInfo[] items; //各个物品的信息
	public int mapId = 1;	//地图，只有一张
}

//战斗结果（服务端推送）
public class MsgBattleResult:MsgBase {
	public MsgBattleResult() {protoName = "MsgBattleResult";}
	//服务端回
	//public int winCamp = 0;	 //获胜的阵营
	public string winId = "0";  //获胜的阵营
}

//玩家退出（服务端推送）
public class MsgLeaveBattle:MsgBase {
	public MsgLeaveBattle() {protoName = "MsgLeaveBattle";}
	//服务端回
	public string id = "";	//玩家id
}

public class MsgAnimation : MsgBase
{
    public MsgAnimation() { protoName = "MsgAnimation"; }
    public int isJump; //是否跳跃
    public float speed; //当前速度
    public string id = "";

}