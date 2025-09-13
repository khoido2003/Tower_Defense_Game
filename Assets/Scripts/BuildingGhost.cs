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

            if (e.activeBuildingType.hasResourceGenerator)
            {
                nearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            }
            else
            {
                nearbyOverlay.Hide();
            }
        }
    }

    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                // Only update position if not touching UI
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    transform.position = UtilsClass.GetMouseWorldPosition();
                }
            }
        }
        else
        {
            // For PC, only update if not over UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                transform.position = UtilsClass.GetMouseWorldPosition();
            }
        }
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
