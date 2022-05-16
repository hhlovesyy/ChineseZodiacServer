using System;


public partial class MsgHandler {


    //同步玩家的动画状态事件
    public static void MsgAnimation(ClientState c, MsgBase msgBase)
    {
        //MsgAnimation msg = (MsgAnimation)msgBase;
        //Player player = c.player;
        //msg.id = player.id;
        //if (player == null) return;
        //Room room = RoomManager.GetRoom(player.roomId);

        //if (room == null)
        //{
        //    return;
        //}
        ////status
        //if (room.status != Room.Status.FIGHT)
        //{
        //    return;
        //}

        //room.Broadcast(msg);

    }

    //同步位置协议
    public static void MsgSyncAnimal(ClientState c, MsgBase msgBase){
		MsgSyncAnimal msg = (MsgSyncAnimal)msgBase;
		Player player = c.player;
		if(player == null) return;
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if(room == null){
			return;
		}
		//status
		if(room.status != Room.Status.FIGHT){
			return;
		}
		//是否作弊
		if(Math.Abs(player.x - msg.x) > 5 ||
			Math.Abs(player.y - msg.y) > 5 ||
			Math.Abs(player.z - msg.z) > 5){
			Console.WriteLine("疑似作弊 " + player.id);
		}
		//更新信息
		player.x = msg.x;
		player.y = msg.y;
		player.z = msg.z;
		player.ex = msg.ex;
		player.ey = msg.ey;
		player.ez = msg.ez;
		//广播
		msg.id = player.id;
		room.Broadcast(msg);
	}

	//开火协议
	public static void MsgFire(ClientState c, MsgBase msgBase){

		MsgFire msg = (MsgFire)msgBase;
		Player player = c.player;
		if(player == null) return;
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if(room == null){
			return;
		}
		//status
		if(room.status != Room.Status.FIGHT){
			return;
		}
		//广播
		msg.id = player.id;
		room.Broadcast(msg);
	}

	//击中协议
	public static void MsgHit(ClientState c, MsgBase msgBase){
		MsgHit msg = (MsgHit)msgBase;
		Player player = c.player;
		if(player == null) return;
		//targetPlayer
		Player targetPlayer = PlayerManager.GetPlayer(msg.targetId);
		if(targetPlayer == null){
			return;
		}
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if(room == null){
			return;
		}
		//status
		if(room.status != Room.Status.FIGHT){
			return;
		}
		//发送者校验
		if(player.id != msg.id){
			return;
		}
		//状态
		int damage = 35;
		targetPlayer.hp -= damage;
		//广播
		msg.id = player.id;
		msg.hp = player.hp;
		msg.damage = damage;
		room.Broadcast(msg);
	}

	public static void MsgKey(ClientState c, MsgBase msgBase)
	{
		MsgKey msg = (MsgKey)msgBase;
		Player player = c.player;
		if (player == null) return;
		
		//room
		Room room = RoomManager.GetRoom(player.roomId);
		if (room == null)
		{
			return;
		}
		//status
		if (room.status != Room.Status.FIGHT)
		{
			return;
		}
		//发送者校验
		//if (player.id != msg.id)
		//{
		//	return;
		//}
		//状态
		
		//广播
		msg.id = player.id;
		
		room.Broadcast(msg);
	}



}


