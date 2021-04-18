using PEProtocol;
using Server.Cache;
using Server.Common;
using Server.Service;
using Server.Service.NetSvc;

namespace Server.System.LoginSys
{
    public class LoginSys: InstanceBase<LoginSys>,ISystem
    {
        private CacheSvc cacheSvc;
        
        public void Init()
        {
            cacheSvc = CacheSvc.Instance;
            PECommon.Log("LoginSys Init Done");
        }

        public void ReqLogin(MsgPack pack)
        {
            ReqLogin data = pack.msg.reqLogin;
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.RspLogin,
            };
            
            if (cacheSvc.IsAcctOnLine(data.acct))
            {
                msg.err = (int) ErrorCode.AcctIsOnline;
            }
            else
            {
                PlayerData playerData = cacheSvc.GetPlayerData(data.acct,data.pass);
                if (playerData == null)
                {
                    msg.err = (int)ErrorCode.WrongPass;
                }
                else
                {
                    msg.rspLogin = new RspLogin
                    {
                        playerData = playerData
                    };
                    cacheSvc.AcctOnline(data.acct,pack.session,playerData);
                }
            }
            
            pack.session.SendMsg(msg);
        }
    }
}