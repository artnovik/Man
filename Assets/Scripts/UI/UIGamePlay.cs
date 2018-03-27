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

    public GameObject[] controlGroup;
    public GameObject pauseMenu;

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

    public void Attack()
    {
        PlayerControl.Instance.locomotion.AttackControl();
    }

    public void Block()
    {

    }

    public void LockTarget()
    {
        PlayerControl.Instance.LockTarget();
    }

    public void SwitchWeapon()
    {
        PlayerControl.Instance.NextWeapon();
        numberWeapon.text = (PlayerControl.Instance.curIndexWeapon + 1).ToString();
    }

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

    #region BlinkingTextOnPause

    [SerializeField]
    private Text pauseText;

    private Coroutine blinkCoroutine;
    private Coroutine waitForRealSecondsCoroutine;

    private IEnumerator Blink()
    {
        pauseText.color = new Color(pauseText.color.r, pauseText.color.g, pauseText.color.b, 0);

        while (true)
        {
            switch (pauseText.color.a.ToString())
            {
                case "0":
                    pauseText.color = new Color(pauseText.color.r, pauseText.color.g, pauseText.color.b, 1);
                    yield return waitForRealSecondsCoroutine = StartCoroutine(WaitForRealSeconds(0.5f));
                    break;
                case "1":
                    pauseText.color = new Color(pauseText.color.r, pauseText.color.g, pauseText.color.b, 0);
                    yield return waitForRealSecondsCoroutine = StartCoroutine(WaitForRealSeconds(0.5f));
                    break;
            }
        }
    }

    // TimeScale won't affect Coroutine
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
        blinkCoroutine = StartCoroutine(Blink());
    }

    private void StopBlinkingPauseText()
    {
        StopCoroutine(blinkCoroutine);
        StopCoroutine(waitForRealSecondsCoroutine);
    }

    #endregion

    #endregion
}
