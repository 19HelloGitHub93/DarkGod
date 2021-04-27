using System;
using MySql.Data.MySqlClient;
using PEProtocol;
using Server.Common;

namespace Server.DB
{
    public class DBMgr: InstanceBase<DBMgr>
    {
        private MySqlConnection conn;
        public void Init()
        {
            conn = new MySqlConnection(
                "server=127.0.0.1;uid=root;password=root;database=darkgod;charset=utf8;");
            conn.Open();
            PECommon.Log("DBMgr Init Done");
        }

        public PlayerData QueryPlayerData(string acct, string pass)
        {
            PlayerData playerData = null;
            try
            {
                MySqlCommand command = new MySqlCommand("select * from account where acct = @acct", conn);
                command.Parameters.AddWithValue("acct", acct);
                MySqlDataReader reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    string p = reader.GetString("pass");
                    if (pass.Equals(p))
                    {
                        playerData = new PlayerData
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            lv = reader.GetInt32("level"),
                            exp = reader.GetInt32("exp"),
                            power = reader.GetInt32("power"),
                            coin = reader.GetInt32("coin"),
                            diamond = reader.GetInt32("diamond"),
                            
                            hp = reader.GetInt32("hp"),
                            ad = reader.GetInt32("ad"),
                            ap = reader.GetInt32("ap"),
                            addef = reader.GetInt32("addef"),
                            apdef = reader.GetInt32("apdef"),
                            dodge = reader.GetInt32("dodge"),
                            pierce = reader.GetInt32("pierce"),
                            critical = reader.GetInt32("critical"),
                            guideid = reader.GetInt32("guideid")
                        };
                    }
                    reader.Close();
                    return playerData;
                }
                if(reader!=null)
                    reader.Close();
 
                playerData = new PlayerData
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500,
                    hp = 2000,
                    ad = 275,
                    ap = 265,
                    addef = 67,
                    apdef = 43,
                    dodge = 7,
                    pierce = 5,
                    critical = 2,
                    guideid = 1001
                };
                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
            catch (Exception e)
            {
                PECommon.Log("Query PlayerData bt acct%pass error:" + e, LogType.Error);
            }
            return playerData;
        }

        private int InsertNewAcctData(string acct, string pass, PlayerData pd)
        {
            int id = -1;
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "insert into account set acct=@acct,pass=@pass,name=@name,level=@level,exp=@exp,power=@power,"+
                    "coin=@coin,diamond=@diamond,hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge," +
                    "pierce=@pierce,critical=@critical,guideid=@guideid",conn);
                cmd.Parameters.Add("acct", acct);
                cmd.Parameters.Add("pass", pass);
                cmd.Parameters.Add("name", pd.name);
                cmd.Parameters.Add("level", pd.lv);
                cmd.Parameters.Add("exp", pd.exp);
                cmd.Parameters.Add("power", pd.power);
                cmd.Parameters.Add("coin", pd.coin);
                cmd.Parameters.Add("diamond", pd.diamond);
                cmd.Parameters.AddWithValue("hp", pd.hp);
                cmd.Parameters.AddWithValue("ad", pd.ad);
                cmd.Parameters.AddWithValue("ap", pd.ap);
                cmd.Parameters.AddWithValue("addef", pd.addef);
                cmd.Parameters.AddWithValue("apdef", pd.apdef);
                cmd.Parameters.AddWithValue("dodge", pd.dodge);
                cmd.Parameters.AddWithValue("pierce", pd.pierce);
                cmd.Parameters.AddWithValue("critical", pd.critical);
                cmd.Parameters.AddWithValue("guideid", pd.guideid);
                cmd.ExecuteNonQuery();
                id = (int) cmd.LastInsertedId;
            }
            catch (Exception e)
            {
                PECommon.Log("insert playerdata error:" + e, LogType.Error);
            }
            return id;
        }

        public bool QueryNameData(string name)
        {
            bool exist = false;
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "select * from account where name = @name",conn);
                cmd.Parameters.AddWithValue("name", name);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                    exist = true;
            }
            catch (Exception e)
            {
                PECommon.Log("query name state error:" + e, LogType.Error);
            }
            finally
            {
                if(reader!=null)
                    reader.Close();
            }
            return exist;
        }

        public bool UpdatePlayerData(int id, PlayerData playerData)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "update account set name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,"+
                    "hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,critical=@critical,guideid=@guideid" +
                    " where id=@id",conn);
                cmd.Parameters.Add("id", @id);
                cmd.Parameters.Add("name", playerData.name);
                cmd.Parameters.Add("level", playerData.lv);
                cmd.Parameters.Add("exp", playerData.exp);
                cmd.Parameters.Add("power", playerData.power);
                cmd.Parameters.Add("coin", playerData.coin);
                cmd.Parameters.Add("diamond", playerData.diamond);
                cmd.Parameters.AddWithValue("hp", playerData.hp);
                cmd.Parameters.AddWithValue("ad", playerData.ad);
                cmd.Parameters.AddWithValue("ap", playerData.ap);
                cmd.Parameters.AddWithValue("addef", playerData.addef);
                cmd.Parameters.AddWithValue("apdef", playerData.apdef);
                cmd.Parameters.AddWithValue("dodge", playerData.dodge);
                cmd.Parameters.AddWithValue("pierce", playerData.pierce);
                cmd.Parameters.AddWithValue("critical", playerData.critical);
                cmd.Parameters.AddWithValue("guideid", playerData.guideid);

                cmd.ExecuteNonQuery();   
            }
            catch (Exception e)
            {
                PECommon.Log("update playerdata error:" + e, LogType.Error);
                return false;
            }

            return true;
        }
    }
}