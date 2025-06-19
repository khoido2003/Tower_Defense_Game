using System;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObject;

    [SerializeField]
    private NearbyOverlay nearbyOverlay;

    private void Awake()
    {
        spriteGameObject = transform.Find("sprite").gameObject;
    }

    private void Start()
    {
        Hide();
        BuildingManager.Instance.OnActiveBuildingTypeChanged +=
            BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(
        object sender,
        BuildingManager.OnActiveBuildingTypeChangedEventArgs e
    )
    {
        if (e.activeBuildingType == null)
        {
            Hide();
            nearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            nearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        spriteGameObject.SetActive(false);
    }
}
