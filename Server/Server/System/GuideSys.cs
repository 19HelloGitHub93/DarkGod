using PEProtocol;
using Server.Cache;
using Server.Common;
using Server.Service.NetSvc;

namespace Server.Service
{
    public class GuideSys:InstanceBase<GuideSys>,ISystem
    {
        private CacheSvc cacheSvc;
        private CfgSys cfgSys;
        
        public void Init()
        {
            cacheSvc = CacheSvc.Instance;
            cfgSys = CfgSys.Instance;
            PECommon.Log("GuideSys Init Done");
        }

        public void ReqGuide(MsgPack pack)
        {
            ReqGuide data = pack.msg.reqGuide;
            GameMsg msg = new GameMsg
            {
                cmd = (int) CMD.RspGuide
            };
            PlayerData pd = cacheSvc.GetPlayerDataSession(pack.session);
            GuideCfg gc = cfgSys.GetGuideCfg(data.guideid);
            
            if (pd.guideid == data.guideid)
            {
                pd.guideid += 1;
                
                pd.coin += gc.coin;
                calcExp(pd, gc.exp);

                if (!cacheSvc.UpdatePlayerData(pd.id, pd))
                {
                    msg.err = (int) ErrorCode.UpdateDBError;
                }
                else
                {
                    msg.rspGuide = new RspGuide
                    {
                        guideid = pd.guideid,
                        coin = pd.coin,
                        lv = pd.lv,
                        exp = pd.exp
                    };
                }
            }
            else
            {
                msg.err = (int)ErrorCode.ServerDataError;
            }
            pack.session.SendMsg(msg);
        }

        private void calcExp(PlayerData pd, int addExp)
        {
            int curtLv = pd.lv;
            int curtExp = pd.exp;
            int addRestExp = addExp;
            while (true)
            {
                int upNeedExp = PECommon.GetExpUpValByLv(curtLv) - curtExp;
                if (addRestExp >= upNeedExp)
                {
                    curtLv += 1;
                    curtExp = 0;
                    addRestExp -= upNeedExp;
                }
                else
                {
                    pd.lv = curtLv;
                    pd.exp = curtExp + addRestExp;
                    break;
                }
            }
        }
    }
}