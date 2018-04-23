using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Locomotion : MonoBehaviour
{
    [Tooltip("Health reference")]
    public Health health;

    public enum TLocomotion
    {
        Idle,
        Movement,
        Attack,
        Dead
    }

    public enum TSpeed
    {
        Walk,
        Run
    }

    public TLocomotion typeLocomotion = TLocomotion.Idle;
    public TSpeed typeSpeed = TSpeed.Walk;

    public Locomotion targetLocomotion;
    public Animator animator;

    private Transform localTransform;
    private float speedFactor = 1f;

    #region AnimationStates

    protected const string MOVEMENT_STATE = "Movement";
    protected const string MOVEMENT_X = "MovX";
    protected const string MOVEMENT_Y = "MovY";
    protected const string ATTACK_STATE = "Attack";

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
            animator.SetFloat(MOVEMENT_X, Mathf.Lerp(animator.GetFloat(MOVEMENT_X), inversDirection.x * speedFactor, 0.05f));
            animator.SetFloat(MOVEMENT_Y, Mathf.Lerp(animator.GetFloat(MOVEMENT_Y), inversDirection.z * speedFactor, 0.05f));
        }

        animator.SetBool(MOVEMENT_STATE, inversDirection.magnitude > 0);
    }

    public void Rotate(Vector3 movDirection)
    {
        if (typeLocomotion == TLocomotion.Dead) { return; }

        float lerpFactor = 0.1f;

        if (typeLocomotion == TLocomotion.Attack) { lerpFactor /= 2; }

        Vector3 fixDir = movDirection;
        fixDir.y = 0;

        Quaternion fixRotation = Quaternion.Lerp(localTransform.rotation, Quaternion.LookRotation(fixDir, Vector3.up), lerpFactor);
        localTransform.rotation = fixRotation;
    }

    public void AttackControl()
    {
        animator.SetTrigger(ATTACK_STATE);
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
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
        {
            typeLocomotion = TLocomotion.Dead;
        }
    }

    public virtual void Attack()
    {

    }

    #endregion
}
