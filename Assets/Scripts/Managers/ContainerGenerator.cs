using System.Collections.Generic;
using UnityEngine;

public class ContainerGenerator : MonoBehaviour
{
    #region Singleton

    public static ContainerGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] private GameObject containerPrefab;
    [SerializeField] public Transform containersParentTransform;

    public void GenerateContainerObject(Transform containerSourceTransform, ContainerTypeEnum.Enum containerType)
    {
        GameObject containerGO = Instantiate(containerPrefab,
            new Vector3(containerSourceTransform.position.x,
                containerPrefab.transform.position.y, // Same position, but fixed by Y axis
                containerSourceTransform.position.z),
            containerPrefab.transform.rotation, containersParentTransform);

        // Filling with items
        FillContainer(containerGO.GetComponent<Container>(), containerType);
    }

    public void FillContainer(Container containerToBeFilled, ContainerTypeEnum.Enum containerType)
    {
        var generatedItemsList = LootGenerator.Instance.GenerateItems(containerType);
        containerToBeFilled.containerItems.AddRange(generatedItemsList);
    }
}