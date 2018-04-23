using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    protected float radius = 2f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnMouseDown()
    {
        var distanceBetweenPlayerAndObject = Vector3.Distance(gameObject.transform.position,
            PlayerControl.Instance.playerTransform.position);

        if ((distanceBetweenPlayerAndObject <= radius))
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        // Debug.Log("Interacting with: " + transform.name);
    }
}
