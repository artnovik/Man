using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [Header("Locomotion")]
    public RectTransform targetLocomotion;
    private Vector3 localLocomotionDir;

    [Header("Camera")]
    public RectTransform targetCamera;
    private Vector3 localCameraDir;

    [Header("Weapons")]
    public Text numberWeapon;

    [Header("PauseMenu")]
    public GameObject pauseMenu;

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

    private void Start()
    {
        numberWeapon.text = (PlayerControl.Instance.curIndexWeapon + 1).ToString();
    }

    private void Update()
    {
        Locomotion();
        Camera();
    }

    public void Locomotion()
    {
        localLocomotionDir.x = targetLocomotion.localPosition.x;
        localLocomotionDir.z = targetLocomotion.localPosition.y;

        PlayerControl.Instance.movementDirection = localLocomotionDir.normalized;
    }

    public void Camera()
    {
        localCameraDir.x = targetCamera.localPosition.x;
        localCameraDir.y = targetCamera.localPosition.y;

        PlayerControl.Instance.cameraControl.rotateDirection += localCameraDir.normalized;
    }

    #region PlayerBars visibility

    public void SetPlayerBarsStatus(bool brightVisibility)
    {
        foreach (var playerBarImage in playerBars)
        {
            playerBarImage.color = brightVisibility ? Colors.playerEngagedUI : Colors.playerCalmUI;
        }
    }

    #endregion

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
    }

    public void LockTarget()
    {
        PlayerControl.Instance.LockTarget();
        eyeLockTargetImage.color = PlayerControl.Instance.stateLockTarget ? Colors.playerActiveUI : Colors.playerDefaultUI;
    }

    public void SwitchWeapon()
    {
        PlayerControl.Instance.NextWeapon();
        numberWeapon.text = (PlayerControl.Instance.curIndexWeapon + 1).ToString();
    }

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

    public void PauseResume()
    {
        bool pause = PlayerControl.Instance.isPaused;

        if (!pause)                                  // If Game is resumed now (pause == false) and we pause it
        {
            pauseMenu.SetActive(!pause);
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

    #endregion
}
