using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private BuildingTypeSO buildingType;

    private BuildingTypeListSO buildingTypeList;

    // Use to initialize internal object, not depend on other external object
    private void Awake()
    {
        // Debug.Log(Resources.Load<BuildingTypeListSO>("BuildingTypeList"));

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        buildingType = buildingTypeList.list[0];
    }

    // Use when need to access external object
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(buildingType.prefab, GetMouseWorldPosition(), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            buildingType = buildingTypeList.list[0];
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            buildingType = buildingTypeList.list[1];
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = (mainCamera.ScreenToWorldPoint(Input.mousePosition));

        mouseWorldPosition.z = 0f;
        return mouseWorldPosition;
    }
}
