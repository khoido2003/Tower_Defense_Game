using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSeletUI : MonoBehaviour
{
    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;

    [SerializeField]
    private List<BuildingTypeSO> ignoreBuildingTypeList;

    [SerializeField]
    private Sprite arrowSprite;

    private Transform arrowBtn;

    private void Awake()
    {
        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(
            typeof(BuildingTypeListSO).Name
        );

        int index = 0;

        arrowBtn = Instantiate(btnTemplate, transform);

        arrowBtn.gameObject.SetActive(true);

        float offsetAmount = +130f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            offsetAmount * index,
            0
        );

        arrowBtn.Find("Image").GetComponent<Image>().sprite = arrowSprite;
        // Make the image smaller
        arrowBtn.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);

        arrowBtn
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(null);
            });

        index++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (ignoreBuildingTypeList.Contains(buildingType))
                continue;

            // Spawn the new object as the child of the current parent transform
            Transform btnTransform = Instantiate(btnTemplate, transform);

            btnTransform.gameObject.SetActive(true);

            offsetAmount = +130f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                offsetAmount * index,
                0
            );

            btnTransform.Find("Image").GetComponent<Image>().sprite = buildingType.sprite;

            btnTransform
                .GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    BuildingManager.Instance.SetActiveBuildingType(buildingType);
                });

            btnTransformDictionary[buildingType] = btnTransform;

            index++;
        }
    }

    private void Update()
    {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {
        arrowBtn.Find("Selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingType in btnTransformDictionary.Keys)
        {
            Transform btnTransform = btnTransformDictionary[buildingType];

            btnTransform.Find("Selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();

        if (activeBuildingType == null)
        {
            arrowBtn.Find("Selected").gameObject.SetActive(true);
        }
        else
            btnTransformDictionary[activeBuildingType].Find("Selected").gameObject.SetActive(true);
    }
}
