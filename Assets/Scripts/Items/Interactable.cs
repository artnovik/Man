using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected float radius = 3f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnMouseDown()
    {
        var distanceBetweenPlayerAndObject = Vector3.Distance(gameObject.transform.position,
            PlayerData.Instance.playerTransform.position);

        //UIGamePlay.Instance.PickUpVisibility(distanceBetweenPlayerAndObject <= radius);

        if (distanceBetweenPlayerAndObject <= radius)
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        //Debug.Log(string.Format("Interacting with: {0}", transform.name));
    }
}