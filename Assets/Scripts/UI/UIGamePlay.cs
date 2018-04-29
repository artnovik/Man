using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private Button attackButton;

    [SerializeField] private Image blockImage;

    [SerializeField] private Button pickUpButton;

    [SerializeField] private GameObject[] controlGroup;

    [Header("ControlUI")] [SerializeField] private Image eyeLockTargetImage;

    private Vector3 invisibleCameraDir;
    private Vector3 localLocomotionDir;

    [Header("MessageForPlayer")] [SerializeField]
    private GameObject messageGO;

    [SerializeField] private Text messageText;

    [Header("Weapons")] public Text numberWeapon;

    [SerializeField] public Image[] playerBars;

    [Header("PlayerBars")] [SerializeField]
    public Image playerHealthBarCurrent;

    [SerializeField] public Image playerHealthBarEmpty;

    [SerializeField] public Image playerStaminaBarCurrent;

    [SerializeField] public Image playerStaminaBarEmpty;

    [SerializeField] private Button switchWeaponButton;

    [Header("Camera")] public RectTransform targetCamera;

    public RectTransform targetInvisibleCamera;

    [Header("Locomotion")] public RectTransform targetLocomotion;

    private Vector3 visibleCameraDir;

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
        numberWeapon.text = (PlayerData.Instance.curIndexWeapon + 1).ToString();
        InitializeCheatsMenu();
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

        PlayerData.Instance.movementDirection = localLocomotionDir.normalized;
    }

    public void Camera()
    {
        visibleCameraDir.x = targetCamera.localPosition.x;
        visibleCameraDir.y = targetCamera.localPosition.y;

        invisibleCameraDir.x = targetInvisibleCamera.localPosition.x;
        invisibleCameraDir.y = targetInvisibleCamera.localPosition.y;


        PlayerData.Instance.cameraControl.rotateDirection += visibleCameraDir.normalized;
        PlayerData.Instance.cameraControl.rotateDirection += invisibleCameraDir.normalized;
    }

    #endregion

    #region PlayerBars

    public void HealthBarValueChange(uint currentHealth)
    {
        playerHealthBarCurrent.fillAmount = (float) currentHealth / 100;
    }

    public void SetPlayerBarsStatus(bool brightVisibility)
    {
        foreach (Image playerBarImage in playerBars)
            playerBarImage.color = brightVisibility ? Colors.playerEngagedUI : Colors.playerCalmUI;
    }

    #endregion

    #region Active Controls

    public void Attack()
    {
        PlayerData.Instance.locomotion.AttackControl();
    }

    public void Block(bool pointerDownValue)
    {
        PlayerData.Instance.Block(pointerDownValue);
        blockImage.color = pointerDownValue ? Colors.playerActiveUI : Colors.playerDefaultUI;
        attackButton.interactable = !pointerDownValue;
        switchWeaponButton.interactable = !pointerDownValue;
        inventoryButton.interactable = !pointerDownValue;
    }

    public void LockTarget()
    {
        PlayerData.Instance.LockTarget();
        eyeLockTargetImage.color =
            PlayerData.Instance.stateLockTarget ? Colors.playerActiveUI : Colors.playerDefaultUI;
    }

    public void SwitchWeapon()
    {
        PlayerData.Instance.NextWeapon();
        AudioManager.Instance.WeaponChangeSound();
        numberWeapon.text = (PlayerData.Instance.curIndexWeapon + 1).ToString();
    }

    public void PickUp()
    {
        Debug.Log("PickUp!");
    }

    public void PickUpVisibility(bool value)
    {
        pickUpButton.gameObject.SetActive(value);
    }

    #endregion

    #region Inventory

    [Header("Inventory")] [SerializeField] private GameObject inventoryMenu;

    [SerializeField] private Button inventoryButton;

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
            {
                StopCoroutine(blinkMessageCoroutine);
            }
        }

        displayMessageCoroutine = StartCoroutine(DisplayMessageRoutine(text, color, duration, blinking));
    }

    private IEnumerator DisplayMessageRoutine(string text, Color32 color, float duration, bool blinking)
    {
        messageText.text = text;
        messageText.color = color;
        messageGO.SetActive(true);
        if (blinking)
        {
            blinkMessageCoroutine = StartCoroutine(Blink(messageText, false));
        }

        yield return new WaitForSeconds(duration);
        if (blinking)
        {
            StopCoroutine(blinkMessageCoroutine);
        }

        messageGO.SetActive(false);
        messageText.text = string.Empty;
    }

    #endregion

    #region PauseResume

    [Header("PauseMenu")] public GameObject pauseMenu;

    public void PauseResume()
    {
        var pause = PlayerData.Instance.isPaused;
        ControlElementsVisibility(pause);
        pauseMenu.SetActive(!pause);

        if (!pause) // If Game is resumed now (pause == false) and we pause it
        {
            AudioManager.Instance.WindowAppearSound();
            StartBlinkingPauseText();
            Time.timeScale = 0f;
            PlayerData.Instance.isPaused = true;
        }
        else // If Game is paused now (pause == true) and we resume it
        {
            StopBlinkingPauseText();
            Time.timeScale = 1f;
            PlayerData.Instance.isPaused = false;
        }
    }

    public void ControlElementsVisibility(bool value)
    {
        foreach (GameObject controlElementGO in controlGroup) controlElementGO.SetActive(value);
    }

    #region BlinkingTextRoutine

    [SerializeField] private Text pauseText;

    private Coroutine blinkPauseCoroutine;
    private Coroutine waitForRealSecondsCoroutine;

    private IEnumerator Blink(Text text, bool ignoreTimeScale)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        while (true)
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

    // "TimeScale won't affect" Coroutine
    public static IEnumerator WaitForRealSeconds(float time)
    {
        var start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time) yield return null;
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

    [Header("CheatMenu")] [SerializeField] private Toggle GOD_MODE_Toggle;

    [SerializeField] private Toggle FPS_SHOW_Toggle;
    [SerializeField] private Toggle LIFESTEAL_Toggle;

    [SerializeField] private GameObject FPS_Text_GO;

    public void InitializeCheatsMenu()
    {
        GOD_MODE_Toggle.isOn = CheatManager.Instance.GOD_MODE;
        FPS_SHOW_Toggle.isOn = CheatManager.Instance.FPS_SHOW;
        LIFESTEAL_Toggle.isOn = CheatManager.Instance.LIFESTEAL;

        FPS_Text_GO.SetActive(CheatManager.Instance.FPS_SHOW);
    }

    public void SetCheatValue(bool value)
    {
        GameObject clickedCheatToggle = EventSystem.current.currentSelectedGameObject;

        if (clickedCheatToggle == GOD_MODE_Toggle.gameObject)
        {
            CheatManager.Instance.SetGOD_MODE(value);
        }
        else if (clickedCheatToggle == FPS_SHOW_Toggle.gameObject)
        {
            CheatManager.Instance.SetFPS_SHOW(value);
            FPS_Text_GO.SetActive(CheatManager.Instance.FPS_SHOW);
        }
        else if (clickedCheatToggle == LIFESTEAL_Toggle.gameObject)
        {
            CheatManager.Instance.SetLIFESTEAL(value);
        }
    }

    #endregion

    #endregion

    #region ScreenEffects

    [SerializeField] private GameObject screenEffectsGO;

    [SerializeField] private Image playerHitScreenEffectImage;

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
        screenEffectCoroutine =
            StartCoroutine(ScreenEffectRoutine(playerHitScreenEffectImage, playerHitScreenEffectDuration));
    }

    #endregion

    #region ContainerUI

    [Header("ContainerUI_Control")] [SerializeField]
    private GameObject containerGO;

    public void ContainerOpen()
    {
        if (!containerGO.activeSelf)
        {
            ControlElementsVisibility(false);
            containerGO.SetActive(true);
            AudioManager.Instance.WindowAppearSound();
            Time.timeScale = 0f;
            PlayerData.Instance.isPaused = true;
        }
    }

    public void ContainerClose()
    {
        Time.timeScale = 1f;
        PlayerData.Instance.isPaused = false;
        ControlElementsVisibility(true);
        containerGO.SetActive(false);
    }

    #endregion

    #region DeathScreen

    [Header("DeathScreen")] [SerializeField]
    private GameObject deathScreen;

    [SerializeField] private GameObject[] hideOnDeathObjects;

    public void ShowDeathScreen()
    {
        foreach (GameObject element in hideOnDeathObjects) element.SetActive(false);

        AudioManager.Instance.OnDeathSound();

        deathScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}