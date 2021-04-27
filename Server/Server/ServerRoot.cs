using Server.Cache;
using Server.DB;
using Server.Service;
using Server.Service.NetSvc;
using Server.System.LoginSys;

namespace Server
{
    public class ServerRoot
    {
        private static ServerRoot instance = null;

        public static ServerRoot Instance
        {
            get
            {
                if(instance==null)
                    instance = new ServerRoot();
                return instance;
            }
        }

        public void Init()
        {
            DBMgr.Instance.Init();
            CacheSvc.Instance.Init();
            NetSvc.Instance.Init();
            LoginSys.Instance.Init();
            GuideSys.Instance.Init();
            CfgSys.Instance.Init();
        }

        public void Update()
        {
            NetSvc.Instance.Update();
        }

        private int SessionID = 0;
        public int GetSessionID()
        {
            if (SessionID == int.MaxValue)
            {
                SessionID = 0;
            }

            return SessionID += 1;
        }
    }
}