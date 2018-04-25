using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TDC.UI
{
    public class ToggleUI : CoreUI
    {
        #region Data

        [Header("Data")]
        public GameObject active;

        [Header("LocalData")]
        private ToggleManagerUI _ToggleManagerUI;

        #endregion

        #region Core

        public void Initialization(ToggleManagerUI target)
        {
            _ToggleManagerUI = target;
        }

        public void OnEnter()
        {
            Sound();
        }

        public void OnClick()
        {
            _ToggleManagerUI.Selected(this);
            Sound();
        }

        public void SwitchState(bool state)
        {
            SetActive(active, state);
        }

        #endregion
    }
}