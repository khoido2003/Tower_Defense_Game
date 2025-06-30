using System;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO buildingType;
    private HealthSystem healthSystem;
    private Transform buildingDemolishBtn;
    private Transform buildingRepairBtn;

    private void Awake()
    {
        buildingDemolishBtn = transform.Find("pfBuildingDemolisherBtn");

        buildingRepairBtn = transform.Find("pfBuildingRepairBtn");

        HideBuildingDemolishBtn();
        HideBuildingRepairhBtn();
    }

    private void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);

        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        ShowBuildingRepairBtn();

        // Playing sound
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        if (healthSystem.IsFullHealth())
        {
            HideBuildingRepairhBtn();
        }
    }

    private void Update() { }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        Destroy(gameObject);

        // Playing sound
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
    }

    private void OnMouseEnter()
    {
        ShowBuildingDemolishBtn();
    }

    private void OnMouseExit()
    {
        HideBuildingDemolishBtn();
    }

    private void HideBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingDemolishBtn()
    {
        if (buildingDemolishBtn != null)
        {
            buildingDemolishBtn.gameObject.SetActive(true);
        }
    }

    private void HideBuildingRepairhBtn()
    {
        if (buildingRepairBtn != null)
        {
            buildingRepairBtn.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingRepairBtn()
    {
        if (buildingRepairBtn != null)
        {
            buildingRepairBtn.gameObject.SetActive(true);
        }
    }
}
