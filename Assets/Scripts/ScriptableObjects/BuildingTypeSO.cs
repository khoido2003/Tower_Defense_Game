using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Sprite sprite;
    public bool hasResourceGenerator;

    public float minConstructionRadius;
    public ResourceGeneratorData resourceGeneratorData;
    public ResourceAmount[] constructionResourceAmountArray;
    public int healthAmountMax;
    public float constructionTimerMax;

    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach (ResourceAmount resourceAmount in constructionResourceAmountArray)
        {
            str +=
                "<color=#"
                + resourceAmount.resourceType.colorHex
                + ">"
                + resourceAmount.resourceType.nameShort
                + ": "
                + resourceAmount.amount
                + "</color>\n";
        }

        return str;
    }
}
