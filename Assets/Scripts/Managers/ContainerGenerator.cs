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

    public GameObject containerCorpsePrefab;
    public GameObject containerJunkPrefab;
    public Transform containersParentTransform;

    // Container Generation with randomized, depended on Type filling with LootGenerator
    public void GenerateAndFillContainer(GameObject containerPrefab, Transform containerSourceTransform,
        ContainerTypeEnum.Enum containerType)
    {
        var containerGO = CreateContainerObject(containerPrefab, containerSourceTransform, containerType);

        // Filling with items
        FillContainer(containerGO.GetComponent<Container>(), containerType);
    }

    // Container Generation overload with manual filling
    public void GenerateAndFillContainer(GameObject containerPrefab, Transform containerSourceTransform,
        ContainerTypeEnum.Enum containerType,
        List<Item> manualItems)
    {
        var containerGO = CreateContainerObject(containerPrefab, containerSourceTransform, containerType);

        // Filling with items
        FillContainer(containerGO.GetComponent<Container>(), containerType, manualItems);
    }

    // Creating container object, using predefined Container prefabs
    public GameObject CreateContainerObject(GameObject containerPrefab, Transform containerSourceTransform,
        ContainerTypeEnum.Enum containerType)
    {
        GameObject containerGO = Instantiate(containerPrefab,
            new Vector3(containerSourceTransform.position.x,
                containerPrefab.transform.position.y, // Same position, but fixed by Y axis
                containerSourceTransform.position.z),
            containerPrefab.transform.rotation, containersParentTransform);

        return containerGO;
    }

    // Filling container using LootGenerator
    public void FillContainer(Container containerToFill, ContainerTypeEnum.Enum containerType)
    {
        var generatedItemsList = LootGenerator.Instance.GenerateItems(containerType);
        containerToFill.containerItems.AddRange(generatedItemsList);
        containerToFill.containerType = containerType;
    }

    // Filling Container manually, passing predefined list
    public void FillContainer(Container containerToFill, ContainerTypeEnum.Enum containerType, List<Item> itemsToFill)
    {
        containerToFill.containerItems.AddRange(itemsToFill);
        containerToFill.containerType = containerType;
    }
}