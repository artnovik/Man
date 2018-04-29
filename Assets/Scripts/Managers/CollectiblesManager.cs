using UnityEngine;

public class CollectiblesManager : MonoBehaviour
{
    [SerializeField] private GameObject collectiblesParentGO;
    public Transform collectiblesParentTransform;

    public void SetParentAsCollectible(GameObject objectToSet)
    {
        objectToSet.transform.SetParent(collectiblesParentTransform);
    }

    #region Singleton

    public static CollectiblesManager Instance;

    private void Awake()
    {
        Instance = this;

        collectiblesParentTransform = collectiblesParentGO.transform;
    }

    #endregion
}