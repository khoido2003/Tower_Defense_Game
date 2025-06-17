using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    private Camera mainCamera;

    public static BuildingManager Instance { get; private set; }

    [SerializeField]
    private BuildingTypeSO activeBuildingType;

    private BuildingTypeListSO buildingTypeList;

    // Use to initialize internal object, not depend on other external object
    private void Awake()
    {
        Instance = this;
        // Debug.Log(Resources.Load<BuildingTypeListSO>("BuildingTypeList"));

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        // activeBuildingType = buildingTypeList.list[0];
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
                Instantiate(
                    activeBuildingType.prefab,
                    GetMouseWorldPosition(),
                    Quaternion.identity
                );
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = (mainCamera.ScreenToWorldPoint(Input.mousePosition));

        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }
}
