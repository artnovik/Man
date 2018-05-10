using TDC;
using UnityEngine;
using UnityEngine.AI;

public class AIBattle : MonoBehaviour
{
    public NavMeshAgent agent;
    private bool chasedRecently;

    public EnemyUI enemyUI;

    public bool isBattle;
    public Locomotion locomotion;
    public Transform target;
    public CoreTrigger viewTrigger;

    #region Unity

    private void Start()
    {
        locomotion = GetComponent<Locomotion>();
        locomotion.animator.SetBool("EnemyZombie", true);
        SetRagdoll(false);
        enemyUI = GetComponent<EnemyUI>();
    }

    private void Update()
    {
        ViewControl();

        if (target && !target.gameObject.GetComponent<Health>().isDead)
        {
            PlayerData.Instance.inBattle = true;
            chasedRecently = true;

            enemyUI.SetHealthBarStatus(true);
            agent.SetDestination(target.position);
            agent.isStopped = false;
            isBattle = true;
        }
        else
        {
            if (chasedRecently)
            {
                PlayerData.Instance.inBattle = false;
                chasedRecently = false;
            }

            enemyUI.SetHealthBarStatus(false);
            agent.isStopped = true;
            isBattle = false;
        }

        Locomotion();
    }

    #endregion

    #region Core

    private void Locomotion()
    {
        if (!target)
        {
            // ToDo: Patrolling fix related to this.
            locomotion.Movement(Vector3.zero);
            return;
        }

        Vector3 fixDirection = Vector3.zero;

        if (Vector3.Distance(target.position, transform.position) >= agent.stoppingDistance)
        {
            fixDirection = (agent.steeringTarget - transform.position).normalized;
            locomotion.Rotate(fixDirection);
            locomotion.targetLocomotion = null;
        }
        else if (target.GetComponent<Health>().currentHealth > 0)
        {
            locomotion.targetLocomotion = target.GetComponent<Locomotion>();
            locomotion.AttackControl();
        }

        locomotion.Movement(fixDirection);
    }

    private void ViewControl()
    {
        foreach (Transform tar in viewTrigger.listObject)
            if (tar)
            {
                target = tar;
                locomotion.targetLocomotion = target.GetComponent<Locomotion>();
                return;
            }

        locomotion.targetLocomotion = null;
        target = null;
    }

    private void SetKinematic(bool newValue)
    {
        var bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies) rb.isKinematic = newValue;
    }

    public void SetRagdoll(bool value)
    {
        SetKinematic(!value);
        GetComponent<Animator>().enabled = !value;
    }

    #endregion
}