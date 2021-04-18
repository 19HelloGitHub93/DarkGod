using PENet;
using PEProtocol;

namespace Server.Service.NetSvc
{
    public class ServerSession: PESession<GameMsg>
    {
        protected override void OnConnected()
        {
            PECommon.Log("Client Connect");
        }

        protected override void OnReciveMsg(GameMsg msg)
        {
            PECommon.Log("RcvPack CMD:"+ (CMD)msg.cmd);
            NetSvc.Instance.AddMsgQue(this,msg);
        }

        protected override void OnDisConnected()
        {
            PECommon.Log("Client DisConnect");
        }
    }
}