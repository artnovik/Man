using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDC;
using TDC.InputSystem;

public class PlayerControl : MonoBehaviourSingleton<PlayerControl>
{
    #region Data
    [Tooltip("Health reference")]
    private Health health;

    public Collider playerCollider;

    [Tooltip("User Interface reference")]
    [SerializeField]
    private UIGamePlay playerUI;

    [Header("Data")]
    public CameraControl cameraControl;
    public Locomotion locomotion;
    public PlayerView playerView;

    public bool stateLockTarget = false;
    public Transform target;

    public Vector3 movementDirection;

    [Header("Weapons")]
    public int curIndexWeapon = 0;
    public List<GameObject> listWeapons = new List<GameObject>();

    private Transform localTransform;

    public bool isPaused;
    public bool godMode;

    #endregion

    #region Unity

    private void Start()
    {
        Initialization();

        SwitchWeapon(curIndexWeapon);
    }

    private void Update()
    {
        CoreUpdate();
    }

    #endregion

    #region Core

    public void Initialization()
    {
        cameraControl.Initialization();
        locomotion.Initialization();
        health = locomotion.health;

        locomotion.animControl.transform.SetParent(null);
        localTransform = transform;
    }

    public void CoreUpdate()
    {
        cameraControl.CoreUpdate();

        if (stateLockTarget && target != null && !target.GetComponent<Health>().isDead)
        {
            cameraControl.target = target;
            locomotion.target = target.GetComponent<Locomotion>();
            playerUI.SetPlayerBarsStatus(true);
        }
        else
        {
            cameraControl.target = null;
            locomotion.target = null;
            playerUI.SetPlayerBarsStatus(false);
        }

        /*if (target)
        {
            locomotion.target = target.GetComponent<Locomotion>();
        }
        else
        {
            locomotion.target = null;
        }*/

        locomotion.CoreUpdate();

        Locomotion();
        LockTargetControl();
    }

    private void Locomotion()
    {
        Vector3 keyboardDirection = Vector3.zero;

        keyboardDirection.x = Input.GetAxis("Horizontal");
        keyboardDirection.z = Input.GetAxis("Vertical");

        keyboardDirection += movementDirection;

        locomotion.Movement(cameraControl.parentCamera.TransformDirection(keyboardDirection));
        localTransform.position = Vector3.Lerp(localTransform.position, locomotion.animControl.transform.position, 0.2f);

        if (!target)
        {
            if (keyboardDirection.magnitude > 0)
            {
                locomotion.Rotate(cameraControl.parentCamera.TransformDirection(keyboardDirection));
            }
        }
        else
        {
            locomotion.Rotate((target.position - localTransform.position).normalized);
        }
    }

    #endregion

    #region LockTarget

    public void LockTarget()
    {
        stateLockTarget = !stateLockTarget;
    }

    private void LockTargetControl()
    {
        if (stateLockTarget)
        {
            foreach (Transform item in playerView.listObject)
            {
                if (item && !item.GetComponent<Health>().isDead)
                {
                    target = item;
                    locomotion.typeSpeed = global::Locomotion.TSpeed.Walk;
                    return;
                }
            }

            target = null;
            locomotion.typeSpeed = global::Locomotion.TSpeed.Run;
        }
        else
        {
            locomotion.typeSpeed = global::Locomotion.TSpeed.Run;
            // ToDo little fix
            foreach (Transform item in playerView.listObject)
            {
                if (item != null && !item.GetComponent<Health>().isDead)
                {
                    target = item;
                    return;
                }
                else
                {
                    target = null;
                    playerView.listObject.Remove(item);
                    return;
                }
            }

            target = null;
        }
    }

    #endregion

    #region Block

    public bool isBlock;

    public void Block(bool pointerDownValue)
    {
        isBlock = pointerDownValue;

        if (isBlock)
        {
            UIGamePlay.Instance.DisplayMessage(Messages.messageBlockTrue, Colors.greenMessage, 1f, false);

            // ToDO HP control (don't affect it) and add animation
        }
        else
        {
            UIGamePlay.Instance.DisplayMessage(Messages.messageBlockFalse, Colors.redMessage, 1f, false);
        }
    }

    #endregion

    #region Weapon

    public void NextWeapon()
    {
        curIndexWeapon++;

        if (curIndexWeapon >= listWeapons.Count)
        {
            curIndexWeapon = 0;
        }

        SwitchWeapon(curIndexWeapon);
    }

    public void SwitchWeapon(int index)
    {
        for (int i = 0; i < listWeapons.Count; i++)
        {
            if (i == index)
            {
                listWeapons[i].SetActive(true);
            }
            else
            {
                listWeapons[i].SetActive(false);
            }
        }
    }

    #endregion
}
