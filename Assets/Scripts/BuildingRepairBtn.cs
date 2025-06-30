using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour
{
    [SerializeField]
    private HealthSystem healthSystem;

    [SerializeField]
    private ResourceTypeSO goldResourceType;

    private void Awake()
    {
        transform
            .Find("Button")
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                int missingHealth =
                    healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
                int repairCost = missingHealth / 2;

                ResourceAmount[] resourceAmountCost = new ResourceAmount[]
                {
                    new ResourceAmount { resourceType = goldResourceType, amount = repairCost },
                };

                if (ResourceManager.Instance.CanAfford(resourceAmountCost))
                {
                    // Can afford the resources
                    ResourceManager.Instance.SpendResource(resourceAmountCost);
                    healthSystem.HealFull();
                }
                else
                {
                    // Can not afford the repairs
                    TooltipUI.Instance.Show(
                        "Can not afford repair cost!",
                        new TooltipUI.TooltipTimer { timer = 2f }
                    );
                }
            });
    }
}
