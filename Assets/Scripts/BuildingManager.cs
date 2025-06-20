using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    private Camera mainCamera;

    ///////////////////////////

    // Event to spawn ghost building
    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    /////////////////////////////

    public static BuildingManager Instance { get; private set; }

    [SerializeField]
    private BuildingTypeSO activeBuildingType;

    private BuildingTypeListSO buildingTypeList;

    // Use to initialize internal object, not depend on other external object
    private void Awake()
    {
        Instance = this;

        // Put the scriptable object inside Resources folder then can load it like this
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    // Use when need to access external object
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if the mouse is clicked and not on the card UI
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null)
            {
                if (
                    CanSpawnBuilding(
                        activeBuildingType,
                        UtilsClass.GetMouseWorldPosition(),
                        out string errorMessage
                    )
                )
                {
                    if (
                        ResourceManager.Instance.CanAfford(
                            activeBuildingType.constructionResourceAmountArray
                        )
                    )
                    {
                        ResourceManager.Instance.SpendResource(
                            activeBuildingType.constructionResourceAmountArray
                        );

                        Instantiate(
                            activeBuildingType.prefab,
                            UtilsClass.GetMouseWorldPosition(),
                            Quaternion.identity
                        );
                    }
                    else
                    {
                        TooltipUI.Instance.Show(
                            "Can't afford "
                                + activeBuildingType.GetConstructionResourceCostString(),
                            new TooltipUI.TooltipTimer { timer = 2f }
                        );
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(
                        errorMessage,
                        new TooltipUI.TooltipTimer { timer = 2f }
                    );
                }
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;

        // Trigger even to spawn ghost building
        OnActiveBuildingTypeChanged?.Invoke(
            this,
            new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType }
        );
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(
        BuildingTypeSO buildingType,
        Vector3 position,
        out string errorMessage
    )
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();
        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(
            position + (Vector3)boxCollider2D.offset,
            boxCollider2D.size,
            0
        );

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        // MIN radius allow to spawn new building
        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            // Collider on top of building position
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            {
                // Has a BuildingTypeHolder
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    errorMessage = "Too close to another building with the same type!";

                    // There already a building this type within the construction radius
                    return false;
                }
            }
        }

        // MAX radius to spawn new building
        float maxConstructionRadius = 25;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            // Collider on top of building position
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            {
                errorMessage = "";
                // Valid position to spawn new building
                return true;
            }
        }
        errorMessage = "Too far from any other building!";
        return false;
    }
}
