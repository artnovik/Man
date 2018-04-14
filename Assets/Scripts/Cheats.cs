using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    #region Singleton

    public static Cheats Instance;

    private void Awake()
    {
        Instance = this;

        InitializePlayerPrefs();
    }

    #endregion

    public bool GOD_MODE;
    public bool FPS_SHOW;
    public bool LIFESTEAL;

    public float DEAD_BODIES_DURATION;

    private void InitializePlayerPrefs()
    {
        FPS_SHOW = PlayerPrefs.GetInt("FPS_SHOW") == 1;
        GOD_MODE = PlayerPrefs.GetInt("GOD_MODE") == 1;
        LIFESTEAL = PlayerPrefs.GetInt("LIFESTEAL") == 1;
    }

    public void SetGOD_MODE(bool value)
    {
        GOD_MODE = value;

        int playerPrefsValue;
        PlayerPrefs.SetInt("GOD_MODE", playerPrefsValue = value ? 1 : 0);
    }

    public void SetFPS_SHOW(bool value)
    {
        FPS_SHOW = value;

        int playerPrefsValue;
        PlayerPrefs.SetInt("FPS_SHOW", playerPrefsValue = value ? 1 : 0);
    }

    public void SetLIFESTEAL(bool value)
    {
        LIFESTEAL = value;

        int playerPrefsValue;
        PlayerPrefs.SetInt("LIFESTEAL", playerPrefsValue = value ? 1 : 0);
    }
}
