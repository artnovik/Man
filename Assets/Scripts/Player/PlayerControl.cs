using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDC;
using TDC.InputSystem;

public class PlayerControl : MonoBehaviourSingleton<PlayerControl>
{
    #region Data
    [Tooltip("Health reference")]
    [HideInInspector]
    public Health playerHealth;

    [HideInInspector]
    public Collider playerCollider;

    [Tooltip("User Interface reference")]
    [SerializeField]
    private UIGamePlay playerUI;

    [Header("Data")]
    public CameraControl cameraControl;
    public Locomotion locomotion;
    public PlayerView playerView;

    [SerializeField]
    private float movementSpeed = 1.2f;

    public bool stateLockTarget = false;
    public Transform target;

    public Vector3 movementDirection;

    [Header("Weapons")]
    public int curIndexWeapon = 0;
    public List<GameObject> listWeapons = new List<GameObject>();

    private Transform localTransform;

    public bool isPaused;
    public bool inFightStatus;

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
        playerHealth = locomotion.health;

        locomotion.animControl.transform.SetParent(null);
        localTransform = transform;

        foreach (var weapon in listWeapons)
        {
            weapon.GetComponent<Collider>().enabled = false;
        }
    }

    public void CoreUpdate()
    {
        cameraControl.CoreUpdate();

        if (stateLockTarget && target != null && !target.GetComponent<Health>().isDead)
        {
            cameraControl.target = target;
            locomotion.targetLocomotion = target.GetComponent<Locomotion>();
        }
        else
        {
            cameraControl.target = null;
            locomotion.targetLocomotion = null;
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

    #endregion

    #region Locomotion

    private void Locomotion()
    {
        Vector3 keyboardDirection = Vector3.zero;

        keyboardDirection.x = Input.GetAxisRaw("Horizontal");
        keyboardDirection.z = Input.GetAxisRaw("Vertical");

        keyboardDirection += movementDirection;

        locomotion.Movement(cameraControl.parentCamera.TransformDirection(keyboardDirection));
        localTransform.position = Vector3.Lerp(localTransform.position, locomotion.animControl.transform.position, movementSpeed);

        if (!target)
        {
            if (keyboardDirection.magnitude > 0)
            {
                locomotion.Rotate(cameraControl.parentCamera.TransformDirection(keyboardDirection));
            }
        }

         // ToDo When at least 1 enemy after player
        if (inFightStatus)
        {
            BattleMusicControl(inFightStatus);
            playerUI.SetPlayerBarsStatus(inFightStatus);
        }
        else
        {
            BattleMusicControl(inFightStatus);
            playerUI.SetPlayerBarsStatus(inFightStatus);
        }
    }

    #endregion

    #region BattleStateMusicControl

    private bool oneSwitchBattleTrue;
    private bool oneSwitchBattleFalse;

    private void BattleMusicControl(bool inBattle)
    {
        if (inBattle)
        {
            if (!oneSwitchBattleTrue && AudioManager.Instance.battleMusicAudioSource.volume == 0.0f)
            {
                AudioManager.Instance.BattleSoundChange(true);
                oneSwitchBattleTrue = true;
                oneSwitchBattleFalse = false;
            }
        }
        else
        {
            if (!oneSwitchBattleFalse && AudioManager.Instance.ambientMusicAudioSource.volume == 0.0f)
            {
                AudioManager.Instance.BattleSoundChange(false);
                oneSwitchBattleFalse = true;
                oneSwitchBattleTrue = false;
            }
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
            if (target != null)
                locomotion.Rotate((target.position - localTransform.position).normalized);

            foreach (Transform item in playerView.listObject)
            {
                if (item && !item.GetComponent<Health>().isDead)
                {
                    target = item;
                    locomotion.targetLocomotion = item.GetComponent<Locomotion>();
                    locomotion.typeSpeed = global::Locomotion.TSpeed.Walk;
                    return;
                }
            }

            target = null;
            locomotion.typeSpeed = global::Locomotion.TSpeed.Run;
        }
        else
        {
            target = null;
            locomotion.typeSpeed = global::Locomotion.TSpeed.Run;
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
            UIGamePlay.Instance.DisplayMessage(Messages.messageBlockTrue, Colors.greenMessage, 100f, false);

            // ToDO Add animation
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
            listWeapons[i].SetActive(i == index);
        }
    }

    #endregion
}
