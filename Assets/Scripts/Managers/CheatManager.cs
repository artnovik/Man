using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public float DEAD_BODIES_DURATION;
    public bool FPS_SHOW;

    public bool GOD_MODE;
    public bool LIFESTEAL;

    private void InitializePlayerPrefs()
    {
        FPS_SHOW = PlayerPrefs.GetInt("FPS_SHOW") == 1;
        GOD_MODE = PlayerPrefs.GetInt("GOD_MODE") == 1;
        LIFESTEAL = PlayerPrefs.GetInt("LIFESTEAL") == 1;
    }

    public void SetGOD_MODE(bool value)
    {
        GOD_MODE = value;

        PlayerPrefs.SetInt("GOD_MODE", value ? 1 : 0);
    }

    public void SetFPS_SHOW(bool value)
    {
        FPS_SHOW = value;

        PlayerPrefs.SetInt("FPS_SHOW", value ? 1 : 0);
    }

    public void SetLIFESTEAL(bool value)
    {
        LIFESTEAL = value;

        PlayerPrefs.SetInt("LIFESTEAL", value ? 1 : 0);
    }

    #region Singleton

    public static CheatManager Instance;

    private void Awake()
    {
        Instance = this;

        InitializePlayerPrefs();
    }

    #endregion
}