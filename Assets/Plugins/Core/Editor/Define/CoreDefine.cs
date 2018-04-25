using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using TDC.SaveAndLoad;

namespace TDC
{
    public static class CoreDefine
    {
        [System.Serializable]
        public class DefineData
        {
            public string name;
            public bool state;

            public void Initialization(string _nameDefine, bool _state)
            {
                name = _nameDefine;
                state = _state;
            }

            public void Switch(bool newState)
            {
                state = newState;
            }
        }

        public static List<DefineData> listPlatform = new List<DefineData>(); //{ get; private set; }

        private static List<BuildTargetGroup> listCorePlatform = new List<BuildTargetGroup> { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS };
        private const char delimiterDefine = ';';

        public static void GetNativeDefine()
        {
            if (listPlatform.Count > 0) { listPlatform.Clear(); }

            foreach (BuildTargetGroup platform in listCorePlatform)
            {
                string getDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
                
                if(getDefine != "" || getDefine != null)
                {
                    string[] splitDefine = getDefine.Split(delimiterDefine);

                    for (int i = 0; i < splitDefine.Length; i++)
                    {
                        bool allowed = true;

                        for (int check = 0; check < listPlatform.Count; check++)
                        {
                            if (listPlatform[check].name == splitDefine[i])
                            {
                                allowed = false;
                                break;
                            }
                        }

                        if (allowed)
                        {
                            DefineData newData = new DefineData();
                            newData.Initialization(splitDefine[i], true);

                            listPlatform.Add(newData);
                        }
                    }
                }
            }
        }

        private static void InitDefine()
        {
            foreach (BuildTargetGroup platform in listCorePlatform)
            {
                string setDefine = "";

                for (int i = 0; i < listPlatform.Count; i++)
                {
                    if (listPlatform[i].state)
                    {
                        setDefine += listPlatform[i].name;

                        if (i < listPlatform.Count - 1)
                        {
                            setDefine += delimiterDefine;
                        }
                    }
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, setDefine);
                AssetDatabase.Refresh();
            }
        }

        public static void LoadDefine()
        {
            if (listPlatform.Count > 0) { listPlatform = new List<DefineData>(); }

            var getList = CoreXml.Load(CoreConfiguration.pathDefineManager, "defineXml");

            if (getList == null || getList.Count == 0)
            {
                CoreXml.DataXml gConfExample = new CoreXml.DataXml();
                gConfExample.Initialization("Define", "ExampleDefineName");

                CoreXml.DataXmlAttribute gConfAttribute = new CoreXml.DataXmlAttribute();
                gConfAttribute.Initialization("Atr", "0");
                gConfExample.listAttribute.Add(gConfAttribute);

                List<CoreXml.DataXml> listExample = new List<CoreXml.DataXml>();
                listExample.Add(gConfExample);

                CoreXml.Save(CoreConfiguration.pathDefineManager, "defineXml", listExample);

                return;
            }

            for (int i = 0; i < getList.Count; i++)
            {
                DefineData newData = new DefineData();

                bool state = false;

                if (int.Parse(getList[i].listAttribute[0].value) == 1) { state = true; }

                newData.Initialization(getList[i].text, state);

                listPlatform.Add(newData);
            }
        }

        public static void AddDefine(string nameDefine)
        {
            DefineData newData = new DefineData();

            newData.Initialization(nameDefine, false);

            listPlatform.Add(newData);
            SaveDefine();
        }

        public static void RemoveDefine(string name)
        {
            for(int i=0; i<listPlatform.Count; i++)
            {
                if(listPlatform[i].name == name)
                {
                    listPlatform.RemoveAt(i);
                    SaveDefine();
                    return;
                }
            }
        }

        public static void SaveDefine()
        {
            List<CoreXml.DataXml> xmlListPlatform = new List<CoreXml.DataXml>();

            for (int i=0; i<listPlatform.Count; i++)
            {
                CoreXml.DataXml newData = new CoreXml.DataXml();
                newData.Initialization("Define", listPlatform[i].name);

                CoreXml.DataXmlAttribute newAtrribute = new CoreXml.DataXmlAttribute();

                int state = 0;

                if (listPlatform[i].state) { state = 1; }

                newAtrribute.Initialization("state", state.ToString());

                newData.listAttribute.Add(newAtrribute);
                xmlListPlatform.Add(newData);
            }

            CoreXml.Save(CoreConfiguration.pathDefineManager, "defineXml", xmlListPlatform);
            InitDefine();
        }
    }
}