using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolisher : MonoBehaviour
{
    [SerializeField]
    private Building building;

    private void Awake()
    {
        transform
            .Find("Button")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                BuildingTypeSO buildingType = building
                    .GetComponent<BuildingTypeHolder>()
                    .buildingType;

                foreach (
                    ResourceAmount resourceAmount in buildingType.constructionResourceAmountArray
                )
                {
                    ResourceManager.Instance.AddResource(
                        resourceAmount.resourceType,
                        Mathf.FloorToInt(resourceAmount.amount * .6f)
                    );
                }

                Destroy(building.gameObject);
            });
    }
}
