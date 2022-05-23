using System;
using System.Collections.Generic;
using System.Text;


public class Item
{
    public int id; //道具的编号,1-maxboxNum
    public float x;
    public float y;
    public float z;
    public int kind; //表示这是哪种道具,指道具的类型

    
    public int GenerateKind(int itemKind)
    {
        int minNum = 1;
        int maxNum = itemKind;
        //随机数对象
        Random _random=new Random();
        int kind= _random.Next(minNum, maxNum);
        while(kind==4)  //不能生成kind=4的钥匙,后面再指定
        {
            kind = _random.Next(minNum, maxNum);
        }
        return kind;
    }
    public Item(int id,float x,float z,int itemKind)
    {
        this.id = id;
        this.x = x;
        y = 10f;
        this.z = z;
        kind = GenerateKind(itemKind);
    }
    public Item(int id,float x,float z)
    {
        this.id = id;
        this.x = x;
        y = 10f;
        this.z = z;
        kind = 4;//目前4表示钥匙,后续有需求再进行更新
    }
}

public class ItemGenerate
{
    //[Header("地图范围")]
    public int minX = -100;
    public int maxX = 96;
    public int minZ = -90;
    public int maxZ = 82;

    //[Header("随机道具箱")]
    //public int maxboxNum;
    //public float intervalTime = 2f;
    public int curBoxNum;    // 当前场景内道具箱个数
    private int totalBoxNum;// 从游戏开始到当前生成的道具总数
                                   //private GameObject guessBox;

    private int roomId; //保存申请创建物体的房间的id
    //各个道具顺序和id
    public List<Item> items = new List<Item>();
    //new
    public int maxBoxNum;
    public int maxItemKind;

    //是否已经产生了钥匙
    public bool hasKey;

    public ItemGenerate(int roomId,int itemNum,int itemKind)
    {
        this.roomId = roomId;
        this.maxBoxNum = itemNum;
        this.maxItemKind = itemKind;
        hasKey = false;
    }

    //随机数对象
    private Random _random = new Random();



    public void Start() //目前来说应该是从这个开始
    {
        //_random = new Random();
        curBoxNum = 0; //刚开始生成的箱子数目为0

        //生成指定数量的宝箱
        RandomBoxSpawn(maxBoxNum);
        //totalBoxNum = maxBoxNum;
        //curBoxNum = maxBoxNum;
        // 从第0s开始，每隔10s生成一波道具箱
        //InvokeRepeating("SpawnBox", 0, intervalTime);
        //StartRun(); //在游戏一开始就启动       
    }

    private void StartRun()
    {
        Generate();
    }


    /// <summary>
    /// 道具随机生成,随机生成位置和种类
    /// </summary>
    /// <param name="boxNum">道具生成数量</param>
    public void RandomBoxSpawn(int boxNum)
    {
        for (int i = 0; i < boxNum; i++)
        {
            RandomSingleBoxSpawn();
        }
        int keyId = this._random.Next(1, boxNum);
        items[keyId].kind = 4;// 暂时设定key的Id是4,用于生成key
        Console.WriteLine("the key's id is" + keyId);
    }
    public int GetRandomInt(int minNum, int maxNum)
    {
        return this._random.Next(minNum, maxNum); //该整数大于或等于minNum且小于maxNum
    }
    //创建生成物体的方法  
    private void RandomSingleBoxSpawn()
    {
        
        //Debug.Log("自动生成道具箱");
        // 获取随机位置x,z
        float x, z;
        x = GetRandomInt(minX, maxX); //-5f和(float)-5效果一样
        z = GetRandomInt(minZ, maxZ);
        curBoxNum++;

        //if (!hasKey)
        //{
        //    // 有10%的概率生成钥匙
        //    int r = this._random.Next(0, 10);
        //    if (r == 5)
        //    {
        //        hasKey = true;
        //        Item keyItem = new Item(curBoxNum, x, z);
        //        items.Add(keyItem);
        //        totalBoxNum++;
        //        Console.WriteLine("haskey1!!!");
        //        return;
        //    }
        //}

        // 显示在场景中 
        //guessBox = Instantiate(guessBoxPrefab, new Vector3(x, 10f, z), Quaternion.identity);
        totalBoxNum++;
        

        Item item = new Item(curBoxNum,x,z,maxItemKind);
        items.Add(item);


    }
    //public MsgBase ToMsg()
    //{

    //}

    //创建协程，命名Generate，并定义内容
    private void Generate()
    {
        curBoxNum = 0;
        while (true)//用while写个死循环让协程一直启动。
        {
            if (curBoxNum <= maxBoxNum)
            {
                RandomSingleBoxSpawn();
                if (totalBoxNum > 200)
                {
                    //StopRun();
                    break;
                }
            }
        }
    }
}
