using System.Collections.Generic;
using PEProtocol;
using Server.Common;
using Server.DB;
using Server.Service;
using Server.Service.NetSvc;
using NotImplementedException = System.NotImplementedException;

namespace Server.Cache
{
    public class CacheSvc: InstanceBase<CacheSvc>,IService
    {
        private DBMgr _dbMgr;
        private Dictionary<string,ServerSession> onLineAcctDic 
            = new Dictionary<string, ServerSession>();
        private Dictionary<ServerSession,PlayerData> onLineSessionDic 
            = new Dictionary<ServerSession, PlayerData>();
        
        public void Init()
        {
            _dbMgr = DBMgr.Instance;
            PECommon.Log("CacheSvc Init Done.");
        }

        public bool IsAcctOnLine(string acct)
        {
            return onLineAcctDic.ContainsKey(acct);
        }

        public PlayerData GetPlayerData(string acct, string pass)
        {
            return _dbMgr.QueryPlayerData(acct,pass);
        }

        public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
        {
            onLineAcctDic.Add(acct,session);
            onLineSessionDic.Add(session,playerData);
        }

        public bool IsNameExist(string name)
        {
            return _dbMgr.QueryNameData(name);
        }

        public PlayerData GetPlayerDataSession(ServerSession session)
        {
            if (onLineSessionDic.TryGetValue(session, out PlayerData playerData))
                return playerData;
            return null;
        }

        public bool UpdatePlayerData(int id, PlayerData playerData)
        {
            return _dbMgr.UpdatePlayerData(id,playerData);
        }

        public void AcctOffLine(ServerSession session)
        {
            foreach (var item in onLineAcctDic)
            {
                if (item.Value == session)
                {
                    onLineAcctDic.Remove(item.Key);
                    break;
                }
            }

            bool succ = onLineSessionDic.Remove(session);
            PECommon.Log("offine result SessionID:"+session.sessionID + " " + succ);
        }
    }
}