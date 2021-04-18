using PEProtocol;
using Server.Cache;
using Server.Common;
using Server.Service;
using Server.Service.NetSvc;
using NotImplementedException = System.NotImplementedException;

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

        public void ReqRename(MsgPack pack)
        {
            ReqRename data = pack.msg.reqRename;
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.RspRename
            };
            if (cacheSvc.IsNameExist(data.name))
            {
                msg.err = (int) ErrorCode.NameIsExist;
            }
            else
            {
                PlayerData playerData = cacheSvc.GetPlayerDataSession(pack.session);
                playerData.name = data.name;
                if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
                {
                    msg.err = (int) ErrorCode.UpdateDBError;
                }
                else
                {
                    msg.rspRename = new RspRename
                    {
                        name = data.name
                    };
                }
            }
            pack.session.SendMsg(msg);
        }
    }
}