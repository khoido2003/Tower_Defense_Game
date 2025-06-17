using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    // [SerializeField]
    // private Transform reosurceTemplate;

    private ResourceTypeListSO resourceTypeList;
    private Transform resourceTemplate;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDictionary;

    private void Awake()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

        resourceTemplate = transform.Find("ResourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        // Create each resource templates: wood, gold, stone
        int index = 0;
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            Transform resourceTransfrom = Instantiate(resourceTemplate, transform);

            resourceTransfrom.gameObject.SetActive(true);

            float offsetAmount = -160f;

            resourceTransfrom.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                offsetAmount * index,
                0
            );

            resourceTransfrom.Find("Image").GetComponent<Image>().sprite = resourceType.sprite;

            resourceTypeTransformDictionary[resourceType] = resourceTransfrom;

            index++;
        }
    }

    private void Start()
    {
        UpdateResourceAmount();

        // Subscribe to event
        ResourceManager.Instance.onResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e)
    {
        UpdateResourceAmount();
    }

    private void Update() { }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);

            Transform resourceTransfrom = resourceTypeTransformDictionary[resourceType];

            resourceTransfrom
                .Find("Text")
                .GetComponent<TextMeshProUGUI>()
                .SetText(resourceAmount.ToString());
        }
    }
}
