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

    private void Start()
    {
        numberWeapon.text = (PlayerControl.Instance.curIndexWeapon + 1).ToString();
    }

    void Update ()
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
}
