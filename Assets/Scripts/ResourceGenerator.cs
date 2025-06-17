using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private float timer;
    private float timerMax;

    private void Awake()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        timerMax = buildingType.resourceGeneratorData.timerMax;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer += timerMax;

            // Debug.Log("PING " + buildingType.resourceGeneratorData.resourceTypeSO.nameString);

            ResourceManager.Instance.AddResource(
                buildingType.resourceGeneratorData.resourceTypeSO,
                1
            );
        }
    }
}
