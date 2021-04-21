using PENet;
using PEProtocol;
using Server.Cache;
using Server.System.LoginSys;

namespace Server.Service.NetSvc
{
    public class ServerSession: PESession<GameMsg>
    {
        public int sessionID = 0;
        protected override void OnConnected()
        {
            sessionID = ServerRoot.Instance.GetSessionID();
            PECommon.Log("SessionID:"+sessionID+" Client Connect");
        }

        protected override void OnReciveMsg(GameMsg msg)
        {
            PECommon.Log("SessionID:"+sessionID+" RcvPack CMD:"+ (CMD)msg.cmd);
            NetSvc.Instance.AddMsgQue(this,msg);
        }

        protected override void OnDisConnected()
        {
            LoginSys.Instance.ClearOffineData(this);
            PECommon.Log("SessionID:"+sessionID+" Client DisConnect");
        }
    }
}