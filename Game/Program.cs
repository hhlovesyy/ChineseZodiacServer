using System;

namespace Game
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //开始的时候服务端连接数据库,
            if (!DbManager.Connect("game", "127.0.0.1", 3306, "root", "Admin"))
            {
                Console.WriteLine("Can't connect with DB");
                return;
            }

            NetManager.StartLoop(8888); //服务器连接的端口号
        }
    }
}
