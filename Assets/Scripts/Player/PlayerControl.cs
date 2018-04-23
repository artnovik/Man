using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDC;
using TDC.InputSystem;

public class PlayerControl : MonoBehaviourSingleton<PlayerControl>
{
    [Header("Data")]
    public CameraControl cameraControl;
    public Locomotion locomotion;
    public PlayerView playerView;
    public Transform playerTransform;
    public Transform dropItemsPoint;

    public float pickUpRadius = 2f;

    public bool isPaused;
    public bool inBattle;

    [Tooltip("Health reference")]
    [HideInInspector]
    public Health playerHealth;

    [HideInInspector]
    public Collider playerCollider;

    [Tooltip("User Interface reference")]
    [SerializeField]
    private UIGamePlay playerUI;

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
            foreach (var weaponCollider in weapon.GetComponentsInChildren<Collider>())
            {
                weaponCollider.enabled = false;
            }
        }

        GetCurrentWeaponColliders();
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

    [Header("Locomotion")]
    [SerializeField]
    private float movementSpeed = 1.2f;
    public bool stateLockTarget = false;
    public Transform target;
    public Vector3 movementDirection;
    private Transform localTransform;

    private void Locomotion()
    {
        if (playerHealth.isDead)
            return;

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

        if (inBattle)
        {
            AudioManager.Instance.BattleMusicControl(true);
            playerUI.SetPlayerBarsStatus(true);
        }
        else
        {
            AudioManager.Instance.BattleMusicControl(false);
            playerUI.SetPlayerBarsStatus(false);
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

    [Header("Weapons")]
    public int curIndexWeapon = 0;
    public List<GameObject> listWeapons = new List<GameObject>();
    private Weapon currentWeapon;
    [HideInInspector]
    public Collider[] currentWeaponColliders;

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

        GetCurrentWeaponColliders();
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon = listWeapons[curIndexWeapon].GetComponent<Weapon>();
    }

    public Collider[] GetCurrentWeaponColliders()
    {
        return currentWeaponColliders = listWeapons[curIndexWeapon].GetComponentsInChildren<Collider>();
    }

    public void SwitchWeaponColliders()
    {
        foreach (var weaponCollider in currentWeaponColliders)
        {
            weaponCollider.enabled = !weaponCollider.enabled;
        }
    }

    #endregion
}
