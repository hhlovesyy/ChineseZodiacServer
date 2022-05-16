//同步玩家信息
public class MsgSyncAnimal:MsgBase {
	public MsgSyncAnimal() {protoName = "MsgSyncAnimal";}
	//位置、旋转、
	public float x = 0f;		
	public float y = 0f;
	public float z = 0f;
	public float ex = 0f;		
	public float ey = 0f;
	public float ez = 0f;
	public float turretY = 0f;	
	//服务端补充
	public string id = "";		//哪个玩家
}
//同步钥匙信息：谁拿到了钥匙
public class MsgKey : MsgBase
{
	public MsgKey() { protoName = "MsgKey"; }

	//服务端补充
	public string id = "";      //哪个动物
}
//开火
public class MsgFire:MsgBase {
	public MsgFire() {protoName = "MsgFire";}
	//炮弹初始位置、旋转
	public float x = 0f;		
	public float y = 0f;
	public float z = 0f;
	public float ex = 0f;
	public float ey = 0f;
	public float ez = 0f;

    //哪一种攻击方式
    public string Fireid = "";

    //服务端补充
    public string id = "";		//哪个玩家
}

//击中
public class MsgHit:MsgBase {
	public MsgHit() {protoName = "MsgHit";}
	//击中谁
	public string targetId = "";
	//击中点	
	public float x = 0f;		
	public float y = 0f;
	public float z = 0f;

    //哪一种攻击方式
    public string Fireid = "";

    //服务端补充
    public string id = "";		//哪个玩家
	public int hp = 0;			//被击中玩家血量
	public int damage = 0;		//受到的伤害
}