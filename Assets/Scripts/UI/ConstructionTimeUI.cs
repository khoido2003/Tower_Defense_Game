using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimeUI : MonoBehaviour
{
    [SerializeField]
    private BuildingConstruction buildingConstruction;

    private Image constructionProgressImage;

    private void Awake()
    {
        constructionProgressImage = transform.Find("mask").Find("image").GetComponent<Image>();
    }

    private void Update()
    {
        constructionProgressImage.fillAmount =
            buildingConstruction.GetConstructionTimerNormalized();
    }
}
