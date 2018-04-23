using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [Header("Locomotion")]
    public RectTransform targetLocomotion;
    private Vector3 localLocomotionDir;

    [Header("Camera")]
    public RectTransform targetCamera;
    public RectTransform targetInvisibleCamera;
    private Vector3 visibleCameraDir;
    private Vector3 invisibleCameraDir;

    [Header("Weapons")]
    public Text numberWeapon;

    [Header("ControlUI")]
    [SerializeField]
    private Image eyeLockTargetImage;
    [SerializeField]
    private Image blockImage;

    [SerializeField]
    private Button attackButton;
    [SerializeField]
    private Button switchWeaponButton;

    [SerializeField]
    private GameObject[] controlGroup;

    [Header("MessageForPlayer")]
    [SerializeField]
    private GameObject messageGO;
    [SerializeField]
    private Text messageText;

    [Header("PlayerBars")]
    [SerializeField]
    public Image playerHealthBarCurrent;
    [SerializeField]
    public Image playerHealthBarEmpty;
    [SerializeField]
    public Image playerStaminaBarCurrent;
    [SerializeField]
    public Image playerStaminaBarEmpty;

    [SerializeField]
    public Image[] playerBars;

    #region Singleton

    public static UIGamePlay Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        inventoryMenu.GetComponent<InventoryUI>().StartCall();
        
        numberWeapon.text = (PlayerControl.Instance.curIndexWeapon + 1).ToString();
        messageGO.SetActive(false);

        pauseMenu.SetActive(true);
        InitializeCheatsMenu();
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
        playerHitScreenEffectImage.gameObject.SetActive(false);
        screenEffectsGO.SetActive(false);
    }

    private void Update()
    {
        Locomotion();
        Camera();
    }

    #endregion

    #region MovementAndCamera

    public void Locomotion()
    {
        localLocomotionDir.x = targetLocomotion.localPosition.x;
        localLocomotionDir.z = targetLocomotion.localPosition.y;

        PlayerControl.Instance.movementDirection = localLocomotionDir.normalized;
    }

    public void Camera()
    {
        visibleCameraDir.x = targetCamera.localPosition.x;
        visibleCameraDir.y = targetCamera.localPosition.y;

        invisibleCameraDir.x = targetInvisibleCamera.localPosition.x;
        invisibleCameraDir.y = targetInvisibleCamera.localPosition.y;


        PlayerControl.Instance.cameraControl.rotateDirection += visibleCameraDir.normalized;
        PlayerControl.Instance.cameraControl.rotateDirection += invisibleCameraDir.normalized;
    }

    #endregion

    #region PlayerBars

    public void HealthBarValueChange(int currentHealth)
    {
        playerHealthBarCurrent.fillAmount = (float)currentHealth / 100;
    }

    public void SetPlayerBarsStatus(bool brightVisibility)
    {
        foreach (var playerBarImage in playerBars)
        {
            playerBarImage.color = brightVisibility ? Colors.playerEngagedUI : Colors.playerCalmUI;
        }
    }

    #endregion

    #region Active Controls

    public void Attack()
    {
        PlayerControl.Instance.locomotion.AttackControl();
    }

    public void Block(bool pointerDownValue)
    {
        PlayerControl.Instance.Block(pointerDownValue);
        blockImage.color = pointerDownValue ? Colors.playerActiveUI : Colors.playerDefaultUI;
        attackButton.interactable = !pointerDownValue;
        switchWeaponButton.interactable = !pointerDownValue;
        inventoryButton.interactable = !pointerDownValue;
    }

    public void LockTarget()
    {
        PlayerControl.Instance.LockTarget();
        eyeLockTargetImage.color = PlayerControl.Instance.stateLockTarget ? Colors.playerActiveUI : Colors.playerDefaultUI;
    }

    public void SwitchWeapon()
    {
        PlayerControl.Instance.NextWeapon();
        AudioManager.Instance.WeaponChangeSound();
        numberWeapon.text = (PlayerControl.Instance.curIndexWeapon + 1).ToString();
    }

    #endregion

    #region Inventory

    [Header("Inventory")]
    [SerializeField]
    private GameObject inventoryMenu;

    [SerializeField]
    private Button inventoryButton;

    public void InventoryOpen()
    {
        inventoryMenu.SetActive(!inventoryMenu.activeSelf);
        InventoryUI.Instance.ClearInfoWindow();
        Time.timeScale = 0;
    }

    public void InventoryClose()
    {
        inventoryMenu.SetActive(!inventoryMenu.activeSelf);
        Time.timeScale = 1;
    }

    #endregion

    #region Message for player

    private Coroutine displayMessageCoroutine;
    private Coroutine blinkMessageCoroutine;

    public void DisplayMessage(string text, Color32 color, float duration, bool blinking)
    {
        if (displayMessageCoroutine != null)
        {
            StopCoroutine(displayMessageCoroutine);

            if (blinkMessageCoroutine != null)
                StopCoroutine(blinkMessageCoroutine);
        }

        displayMessageCoroutine = StartCoroutine(DisplayMessageRoutine(text, color, duration, blinking));
    }

    private IEnumerator DisplayMessageRoutine(string text, Color32 color, float duration, bool blinking)
    {
        messageText.text = text;
        messageText.color = color;
        messageGO.SetActive(true);
        if (blinking)
            blinkMessageCoroutine = StartCoroutine(Blink(messageText, false));

        yield return new WaitForSeconds(duration);
        if (blinking)
            StopCoroutine(blinkMessageCoroutine);
        messageGO.SetActive(false);
        messageText.text = string.Empty;
    }

    #endregion

    #region PauseResume

    [Header("PauseMenu")]
    public GameObject pauseMenu;

    public void PauseResume()
    {
        bool pause = PlayerControl.Instance.isPaused;

        if (!pause)                                  // If Game is resumed now (pause == false) and we pause it
        {
            pauseMenu.SetActive(!pause);
            AudioManager.Instance.WindowAppearSound();
            foreach (var controlElementGO in controlGroup)
            {
                controlElementGO.SetActive(pause);
            }
            StartBlinkingPauseText();
            Time.timeScale = 0f;
            PlayerControl.Instance.isPaused = true;
        }
        else                                         // If Game is paused now (pause == true) and we resume it
        {
            pauseMenu.SetActive(!pause);
            foreach (var controlElementGO in controlGroup)
            {
                controlElementGO.SetActive(pause);
            }
            StopBlinkingPauseText();
            Time.timeScale = 1f;
            PlayerControl.Instance.isPaused = false;
        }
    }

    #region BlinkingTextRoutine

    [SerializeField]
    private Text pauseText;

    private Coroutine blinkPauseCoroutine;
    private Coroutine waitForRealSecondsCoroutine;

    private IEnumerator Blink(Text text, bool ignoreTimeScale)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        while (true)
        {
            switch (text.color.a.ToString())
            {
                case "0":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                    if (ignoreTimeScale)
                    {
                        yield return waitForRealSecondsCoroutine = StartCoroutine(WaitForRealSeconds(0.5f));
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
                case "1":
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                    if (ignoreTimeScale)
                    {
                        yield return waitForRealSecondsCoroutine = StartCoroutine(WaitForRealSeconds(0.5f));
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
            }
        }
    }

    // "TimeScale won't affect" Coroutine
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

    private void StartBlinkingPauseText()
    {
        blinkPauseCoroutine = StartCoroutine(Blink(pauseText, true));
    }

    private void StopBlinkingPauseText()
    {
        StopCoroutine(blinkPauseCoroutine);
        StopCoroutine(waitForRealSecondsCoroutine);
    }

    #endregion

    #region CheatsMenu

    [Header("CheatMenu")]
    [SerializeField] private Toggle GOD_MODE_Toggle;
    [SerializeField] private Toggle FPS_SHOW_Toggle;
    [SerializeField] private Toggle LIFESTEAL_Toggle;

    [SerializeField] private GameObject FPS_Text_GO;

    public void InitializeCheatsMenu()
    {
        GOD_MODE_Toggle.isOn = Cheats.Instance.GOD_MODE;
        FPS_SHOW_Toggle.isOn = Cheats.Instance.FPS_SHOW;
        LIFESTEAL_Toggle.isOn = Cheats.Instance.LIFESTEAL;

        FPS_Text_GO.SetActive(Cheats.Instance.FPS_SHOW);
    }

    public void SetCheatValue(bool value)
    {
        var clickedCheatToggle = EventSystem.current.currentSelectedGameObject;

        if (clickedCheatToggle == GOD_MODE_Toggle.gameObject)
        {
            Cheats.Instance.SetGOD_MODE(value);
        }
        else if (clickedCheatToggle == FPS_SHOW_Toggle.gameObject)
        {
            Cheats.Instance.SetFPS_SHOW(value);
            FPS_Text_GO.SetActive(Cheats.Instance.FPS_SHOW);
        }
        else if (clickedCheatToggle == LIFESTEAL_Toggle.gameObject)
        {
            Cheats.Instance.SetLIFESTEAL(value);
        }
    }

    #endregion

    #endregion

    #region ScreenEffects

    [SerializeField]
    private GameObject screenEffectsGO;

    [SerializeField]
    private Image playerHitScreenEffectImage;
    private const float playerHitScreenEffectDuration = 2f;

    private Coroutine screenEffectCoroutine;
    private IEnumerator ScreenEffectRoutine(Image screenEffectImage, float duration)
    {
        screenEffectsGO.SetActive(true);
        screenEffectImage.gameObject.SetActive(true);
        screenEffectImage.color = Colors.screenEffect;
        screenEffectImage.CrossFadeAlpha(0f, duration, false);

        yield return new WaitForSeconds(duration);
        screenEffectImage.gameObject.SetActive(false);
        screenEffectsGO.SetActive(false);
    }

    public void ActivatePlayerHitScreenEffect()
    {
        screenEffectCoroutine = StartCoroutine(ScreenEffectRoutine(playerHitScreenEffectImage, playerHitScreenEffectDuration));
    }

    #endregion

    #region DeathScreen

    [Header("DeathScreen")]
    [SerializeField]
    private GameObject deathScreen;

    [SerializeField]
    private GameObject[] hideOnDeathObjects;

    public void ShowDeathScreen()
    {
        foreach (var element in hideOnDeathObjects)
        {
            element.SetActive(false);
        }

        AudioManager.Instance.OnDeathSound();

        deathScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
