using TMPro;
using UnityEngine;

public class ResourceOverlay : MonoBehaviour
{
    [SerializeField]
    private ResourceGenerator resourceGenerator;

    private Transform barTransform;

    private void Start()
    {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData
            .resourceType
            .sprite;

        barTransform = transform.Find("bar");

        barTransform.localScale = new Vector3(resourceGenerator.GetTimerNormalized(), 1, 1);

        // If the generator actually can generate some resources
        if (resourceGenerator.enabled)
        {
            transform
                .Find("text")
                .GetComponent<TextMeshPro>()
                .SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
        }
        else
        {
            transform.Find("text").GetComponent<TextMeshPro>().SetText("0");
        }
    }

    private void Update()
    {
        if (resourceGenerator.enabled)
        {
            barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1, 1);
        }
        else
        {
            barTransform.localScale = new Vector3(0, 1, 1); // or hide the bar
        }
    }
}
