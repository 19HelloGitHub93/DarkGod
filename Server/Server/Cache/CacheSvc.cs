using System.Collections.Generic;
using PEProtocol;
using Server.Common;
using Server.Service;
using Server.Service.NetSvc;

namespace Server.Cache
{
    public class CacheSvc: InstanceBase<CacheSvc>,IService
    {
        private Dictionary<string,ServerSession> onLineAcctDic 
            = new Dictionary<string, ServerSession>();
        private Dictionary<ServerSession,PlayerData> onLineSessionDic 
            = new Dictionary<ServerSession, PlayerData>();
        
        public void Init()
        {
            PECommon.Log("CacheSvc Init Done.");
        }

        public bool IsAcctOnLine(string acct)
        {
            return onLineAcctDic.ContainsKey(acct);
        }

        public PlayerData GetPlayerData(string acct, string pass)
        {
            return null;
        }

        public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
        {
            onLineAcctDic.Add(acct,session);
            onLineSessionDic.Add(session,playerData);
        }
    }
}