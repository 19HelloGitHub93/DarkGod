using System;
using System.Collections.Generic;
using System.Xml;
using MySql.Data;
using PEProtocol;
using Server.Cache;
using Server.Common;
using NotImplementedException = System.NotImplementedException;

namespace Server.Service
{
    public class CfgSys:InstanceBase<CfgSys>,ISystem
    {
        private Dictionary<int, GuideCfg> guideDic = new Dictionary<int, GuideCfg>();
        
        public void Init()
        {
            InitGuideCfg();
            PECommon.Log("CfgSys Init Done");
        }
        
        private void InitGuideCfg()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"E:\Project\Unity\DarkGod\Client\Assets\Resources\ResCfgs\guide.xml");

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (string.IsNullOrEmpty(ele.GetAttributeNode("ID").InnerText))
                    continue;
                int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                GuideCfg guideCfg = new GuideCfg
                {
                    ID = id
                };

                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "coin":
                            guideCfg.coin = int.Parse(e.InnerText);
                            break;
                        case "exp":
                            guideCfg.exp = int.Parse(e.InnerText);
                            break;
                    }
                }
                
                guideDic.Add(id,guideCfg);
            }
        }
        public GuideCfg GetGuideCfg(int id)
        {
            GuideCfg autoGuideCfg;
            guideDic.TryGetValue(id, out autoGuideCfg);
            return autoGuideCfg;
        }
    }
    public class GuideCfg : BaseData<GuideCfg>
    {
        public int coin;
        public int exp;
    }

    public class BaseData<T>
    {
        public int ID;
    }
}