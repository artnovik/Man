using System.Collections.Generic;
using TDC;
using UnityEngine;

public class PlayerControl : MonoBehaviourSingleton<PlayerControl>
{
    [Header("Data")] public CameraControl cameraControl;

    public Transform dropItemsPoint;
    public bool inBattle;

    public bool isPaused;
    public Locomotion locomotion;

    public float pickUpRadius = 2f;

    [HideInInspector] public Collider playerCollider;

    [Tooltip("Health reference")] [HideInInspector]
    public Health playerHealth;

    public Transform playerTransform;

    [Tooltip("User Interface reference")] [SerializeField]
    private UIGamePlay playerUI;

    public PlayerView playerView;

    #region Unity

    private void Start()
    {
        playerHealth = locomotion.health;
        locomotion.animator.transform.SetParent(null);
        localTransform = transform;

        foreach (GameObject weapon in listWeapons)
        foreach (Collider weaponCollider in weapon.GetComponentsInChildren<Collider>())
            weaponCollider.enabled = false;

        GetCurrentWeaponColliders();

        SwitchWeapon(curIndexWeapon);
    }

    private void Update()
    {
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

        Locomotion();
        LockTargetControl();
    }

    #endregion

    #region Locomotion

    [Header("Locomotion")] [SerializeField]
    private float movementSpeed = 1.2f;

    public bool stateLockTarget;
    public Transform target;
    public Vector3 movementDirection;
    private Transform localTransform;

    private void Locomotion()
    {
        if (playerHealth.isDead)
        {
            return;
        }

        Vector3 keyboardDirection = Vector3.zero;

        keyboardDirection.x = Input.GetAxisRaw("Horizontal");
        keyboardDirection.z = Input.GetAxisRaw("Vertical");

        keyboardDirection += movementDirection;

        locomotion.Movement(cameraControl.parentCamera.TransformDirection(keyboardDirection));
        localTransform.position =
            Vector3.Lerp(localTransform.position, locomotion.animator.transform.position, movementSpeed);

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
            {
                locomotion.Rotate((target.position - localTransform.position).normalized);
            }

            foreach (Transform item in playerView.listObject)
                if (item && !item.GetComponent<Health>().isDead)
                {
                    target = item;
                    locomotion.targetLocomotion = item.GetComponent<Locomotion>();
                    locomotion.typeSpeed = global::Locomotion.TSpeed.Walk;
                    return;
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

    [Header("Weapons")] public int curIndexWeapon;

    public List<GameObject> listWeapons = new List<GameObject>();
    private WeaponOld currentWeapon;

    [HideInInspector] public Collider[] currentWeaponColliders;

    public void NextWeapon()
    {
        curIndexWeapon++;

        if (curIndexWeapon >= listWeapons.Count)
        {
            curIndexWeapon = 0;
        }

        SwitchWeapon(curIndexWeapon);
    }

    private void SwitchWeapon(int index)
    {
        for (var i = 0; i < listWeapons.Count; i++) listWeapons[i].SetActive(i == index);

        GetCurrentWeaponColliders();
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon = listWeapons[curIndexWeapon].GetComponent<Weapon>();
    }

    private Collider[] GetCurrentWeaponColliders()
    {
        return currentWeaponColliders = listWeapons[curIndexWeapon].GetComponentsInChildren<Collider>();
    }

    public void SwitchWeaponColliders()
    {
        foreach (Collider weaponCollider in currentWeaponColliders) weaponCollider.enabled = !weaponCollider.enabled;
    }

    #endregion
}