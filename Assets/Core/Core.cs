using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDC.Audio;
//using UnityEditor;

#if GoogleMobileAds 
using GoogleMobileAds.Api;
#endif

namespace TDC
{
    [AddComponentMenu("TDC/Core")]
    public class Core : MonoBehaviourSingleton<Core>
    {
        #region Data

        #if GoogleMobileAds
        [SerializeField] private CoreAdMob coreAdMob;
        public CoreAdMob CoreAdMob { get { return coreAdMob; } }
        #endif

        [SerializeField] private DataDebug dataDebug;
        public DataDebug DataDebug { get { return dataDebug; } }

        [SerializeField] private UI.DialogApp dialogApp;
        public UI.DialogApp DialogApp { get { return dialogApp; } }

        [SerializeField] private CoreAudio coreAudio;
        public CoreAudio CoreAudio { get { return coreAudio; } }

        #endregion

        #region Unity

        private void Awake()
        {
            Initialization();
        }

        private void Update()
        {
            CoreUpdate();
        }

        #endregion

        #region Core

        public void Initialization()
        {
            Globals.Instance.Initialization();

            #if GoogleMobileAds
            coreAdMob.Initialization();
            #endif

            DontDestroyOnLoad(gameObject);


            List<int> initScene = new List<int>();

            initScene.Add(2);

            LoadScreen.LoadManager.Instance.LoadScene(initScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        public void CoreUpdate()
        {
            Globals.Instance.CoreUpdate();
        }

        #endregion

        #region Debug

        public string GetStringColor(string text, DDebug.TColor type = DDebug.TColor.Default)
        {
            foreach (var Item in DataDebug.listDebug)
            {
                if (Item.TypeColor == type)
                {
                    string StrColor = Item.Color.r.ToString("X2") + Item.Color.g.ToString("X2") + Item.Color.b.ToString("X2");
                    return "<color=#" + StrColor + ">" + text + "</color>";
                }
            }

            return text;
        }

        public string DebugLog(string text, DDebug.TColor type = DDebug.TColor.Default)
        {
            return (GetStringColor("[System] ", DDebug.TColor.System) + GetStringColor(text, type));
        }

        #endregion
    }

    //[InitializeOnLoad]
    public class CoreConfiguration
    {
        public static string pathDefineManager;

        private const string globalPathXml = "Assets\\Core\\Data\\";
        private const string globalNameXml = "globalConfiguration";

        public static List<SaveAndLoad.CoreXml.DataXml> globalConfiguration = new List<SaveAndLoad.CoreXml.DataXml>();

        static CoreConfiguration()
        {
            globalConfiguration = new List<SaveAndLoad.CoreXml.DataXml>();
            globalConfiguration = SaveAndLoad.CoreXml.Load(globalPathXml, globalNameXml);

            if (globalConfiguration == null)
            {
                SaveAndLoad.CoreXml.DataXml gConfExample = new SaveAndLoad.CoreXml.DataXml();
                gConfExample.Initialization("PathDefineManager", "Assets\\Core\\Data\\");

                List<SaveAndLoad.CoreXml.DataXml> listExample = new List<SaveAndLoad.CoreXml.DataXml>();
                listExample.Add(gConfExample);

                SaveAndLoad.CoreXml.Save(globalPathXml, globalNameXml, listExample);
                return;
            }

            Debug.Log("Unity Core Initialization [" + globalConfiguration.Count + "]");

            for (int i=0; i<globalConfiguration.Count; i++)
            {
                switch(globalConfiguration[i].name)
                {
                    case "PathDefineManager": pathDefineManager = globalConfiguration[i].text; break;
                }

                Debug.Log("Load path: " + globalConfiguration[i].text);
            }
        }

        public static void EditConfiguration(string name, string value)
        {
            if (globalConfiguration.Count == 0) { return; }

            for (int i = 0; i < globalConfiguration.Count; i++)
            {
                if (globalConfiguration[i].name == name)
                {

                    globalConfiguration[i].SetText(value);
                }
            }

            CoreSave();
        }

        public static void CoreSave()
        {
            SaveAndLoad.CoreXml.Save(globalPathXml, globalNameXml, globalConfiguration);
            Debug.Log("Unity Core Save");
        }
    }
}