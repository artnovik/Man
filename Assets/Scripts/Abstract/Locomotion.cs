using UnityEngine;

public abstract class Locomotion : MonoBehaviour
{
    public enum TLocomotion
    {
        Idle,
        Movement,
        Attack,
        Attack_Hands,
        Dead,
        Block
    }

    public enum TSpeed
    {
        Walk,
        Run
    }

    public Animator animator;

    [Tooltip("Health reference")] public Health health;

    private Transform localTransform;
    private float speedFactor = 1f;

    public Locomotion targetLocomotion;

    public TLocomotion typeLocomotion = TLocomotion.Idle;
    public TSpeed typeSpeed = TSpeed.Walk;

    #region AnimationStates

    protected const string MOVEMENT_STATE = "Movement";
    protected const string MOVEMENT_X = "MovX";
    protected const string MOVEMENT_Y = "MovY";
    protected const string ATTACK_STATE = "Attack";
    protected const string BLOCK_STATE = "Block";
    protected const string ATACK_HANDS_STATE = "Attack_Hands";
    protected const string ATTACK_STATE_SPECIAL = "Attack_Special";

    #endregion

    #region Unity

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        localTransform = transform;
    }

    public virtual void Update()
    {
        ControlState();
    }

    #endregion

    #region Locomotion

    public void Movement(Vector3 direction)
    {
        Vector3 inversDirection = localTransform.InverseTransformDirection(direction);

        switch (typeSpeed)
        {
            case TSpeed.Walk:
                speedFactor = Mathf.Lerp(speedFactor, 1, 0.1f);
                break;
            case TSpeed.Run:
                speedFactor = Mathf.Lerp(speedFactor, 2, 0.1f);
                break;
        }

        if (inversDirection.magnitude > 0)
        {
            animator.SetFloat(MOVEMENT_X,
                Mathf.Lerp(animator.GetFloat(MOVEMENT_X), inversDirection.x * speedFactor, 0.05f));
            animator.SetFloat(MOVEMENT_Y,
                Mathf.Lerp(animator.GetFloat(MOVEMENT_Y), inversDirection.z * speedFactor, 0.05f));
        }

        animator.SetBool(MOVEMENT_STATE, inversDirection.magnitude > 0);
    }

    public void Rotate(Vector3 movDirection)
    {
        if (typeLocomotion == TLocomotion.Dead)
        {
            return;
        }

        var lerpFactor = 0.1f;

        if (typeLocomotion == TLocomotion.Attack || typeLocomotion == TLocomotion.Attack_Hands)
        {
            lerpFactor /= 2;
        }

        Vector3 fixDir = movDirection;
        fixDir.y = 0;

        Quaternion fixRotation = Quaternion.Lerp(localTransform.rotation, Quaternion.LookRotation(fixDir, Vector3.up),
            lerpFactor);
        localTransform.rotation = fixRotation;
    }

    public virtual void AttackControl()
    {
        // This meant to be overridden
    }

    public virtual void SpecialAttack()
    {

    }

    private void ControlState()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            typeLocomotion = TLocomotion.Idle;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
        {
            typeLocomotion = TLocomotion.Movement;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            typeLocomotion = TLocomotion.Attack;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
        {
            typeLocomotion = TLocomotion.Block;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack_Hands"))
        {
            typeLocomotion = TLocomotion.Attack_Hands;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
        {
            typeLocomotion = TLocomotion.Dead;
        }
    }

    public virtual void Attack()
    {
        // This meant to be overridden
    }

    #endregion
}