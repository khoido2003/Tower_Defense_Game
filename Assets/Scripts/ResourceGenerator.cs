using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;

    private float timer;
    private float timerMax;

    private void Awake()
    {
        resourceGeneratorData =
            GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(
            transform.position,
            resourceGeneratorData.resourceTypeDetectionRadius
        );

        int nearbyResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                // It's a resouce node
                if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                {
                    // Same resource type!
                    nearbyResourceAmount++;
                }
                else
                {
                    Debug.Log("Unknow re");
                }
            }
        }
        nearbyResourceAmount = Mathf.Clamp(
            nearbyResourceAmount,
            0,
            resourceGeneratorData.maxResourceAmount
        );

        if (nearbyResourceAmount == 0)
        {
            // No response nodes nearby
            // Disable resource generator
            enabled = false;
        }
        else
        {
            // If there are more resource node then it will generate more resource by reduce the timer max to produce
            timerMax =
                (resourceGeneratorData.timerMax / 2f)
                + resourceGeneratorData.timerMax
                    * (1 - (float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount);
        }

        Debug.Log(nearbyResourceAmount + " " + timerMax);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer += timerMax;

            // Debug.Log("PING " + buildingType.resourceGeneratorData.resourceTypeSO.nameString);

            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }
}
