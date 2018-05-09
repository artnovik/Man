using System.Collections.Generic;
using TDC;
using UnityEngine;

public class PlayerData : MonoBehaviourSingleton<PlayerData>
{
    [Header("Data")] public CameraControl cameraControl;
    public bool inBattle;

    public bool isPaused;
    public Locomotion locomotion;

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

        /*foreach (GameObject weapon in listWeapons)
        foreach (Collider weaponCollider in weapon.GetComponentsInChildren<Collider>())
            weaponCollider.enabled = false;

        GetCurrentWeaponColliders();

        SwitchWeapon(curIndexWeapon);*/
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
        if (playerHealth.isDead || isPaused)
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

    [Header("Weapons")] public int currentWeaponIndex;

    [SerializeField] private Transform weaponParent;
    public List<GameObject> weaponsList = new List<GameObject>(2);
    [SerializeField] private WeaponData currentWeaponData;
    [SerializeField] private GameObject currentWeaponGO;

    [HideInInspector] public Collider[] currentWeaponColliders;

    public void AddToEquipSlot(GameObject weaponGO, int slotIndex)
    {
        // Add copy of Weapon into weaponsList
        weaponsList[slotIndex] = Instantiate(weaponGO, weaponParent);

        // Disable it instantly
        weaponsList[slotIndex].SetActive(false);

        // If hands are empty, equipping this Weapon
        if (currentWeaponGO == null)
        {
            EquipWeapon(slotIndex);
        }
    }

    /*private void UnEquipWeapon(){}*/

    public void SwitchWeapon()
    {
        int prevIndex = currentWeaponIndex;
        
        currentWeaponIndex++;

        if (currentWeaponIndex >= weaponsList.Count)
        {
            currentWeaponIndex = 0;
        }

        if (prevIndex != currentWeaponIndex)
        {
            EquipWeapon(currentWeaponIndex);
        }
    }

    private void EquipWeapon(int index)
    {
        // Disabling "Previous", if it was, and Enabling "New"
        if (currentWeaponGO != null)
        {
            currentWeaponGO.SetActive(false);
        }

        // Assign new currentWeapon Index
        currentWeaponIndex = index;

        // Assign current weaponGameObject to selected one
        currentWeaponGO = weaponsList[currentWeaponIndex];
        // Doing the same with WeaponData
        currentWeaponData = currentWeaponGO.GetComponent<WeaponData>();

        // Instantly enabling Weapon object
        currentWeaponGO.SetActive(true);

        // Disabling it's colliders, to prevent alwaysDamage on Enemies
        DisableCurrentWeaponColliders();

        // Setting number to current index in UI and Play short sound
        UIGamePlay.Instance.SetWeaponNumberText(currentWeaponIndex + 1);
        AudioManager.Instance.WeaponChangeSound();
    }

    private Collider[] GetCurrentWeaponColliders()
    {
        return currentWeaponColliders = currentWeaponGO.GetComponentsInChildren<Collider>();
    }

    private void DisableCurrentWeaponColliders()
    {
        foreach (var coll in GetCurrentWeaponColliders())
        {
            coll.enabled = false;
        }
    }

    public void SwitchWeaponColliders()
    {
        foreach (Collider weaponCollider in GetCurrentWeaponColliders())
            weaponCollider.enabled = !weaponCollider.enabled;
    }

    public GameObject GetCurrentWeaponGO()
    {
        return currentWeaponGO;
    }

    public int GetCurrentWeaponDamage()
    {
        return currentWeaponData.weaponData.GetDamage();
    }

    #endregion
}