using System;
using System.Configuration;

namespace Server
{
    internal class ServerStart
    {
        public static void Main(string[] args)
        {
            ServerRoot.Instance.Init();
            while (true)
            {
                ServerRoot.Instance.Update();
            }
        }
    }
}