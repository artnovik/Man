using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TDC
{
    public class CorePlayerPrefsEditor : EditorWindowWrapper
    {
        #region Data

        private static string nameWindow = "PlayerPrefs";

        #endregion

        #region Unity

        protected override void OnGUI()
        {
            base.OnGUI();

            Header(nameWindow);
            Floor();
        }

        #endregion

        #region Core

        [MenuItem("TDC/Player Prefs")]
        public static void InitializationWindow()
        {
            CorePlayerPrefsEditor window = (CorePlayerPrefsEditor)GetWindow(typeof(CorePlayerPrefsEditor));
            window.titleContent = new GUIContent(nameWindow);
            window.Show();
        }

        public void CoreUpdate()
        {

        }

        #endregion
    }
}