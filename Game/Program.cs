using System;

namespace Game
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            if (!DbManager.Connect("game", "127.0.0.1", 3306, "root", "jdjjc552422"))
            {
                Console.WriteLine("laopo!dsfs!!");
                return;
            }

            NetManager.StartLoop(8888);
        }
    }
}
