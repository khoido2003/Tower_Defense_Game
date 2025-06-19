using UnityEngine;

public class SpritePositionSortOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private bool runOnce;

    // Make sure the bushes don't stay behind the trunk of the tree
    [SerializeField]
    private float positionOffsetY;

    private void Awake()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Transform spriteTransform = transform.Find("sprite");

            if (spriteTransform != null)
            {
                spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
            }
            else
            {
                Debug.LogWarning(
                    $"{name}: No SpriteRenderer found on 'sprite' child. Disabling SpritePositionSortOrder."
                );
            }
        }
    }

    private void LateUpdate()
    {
        float precisionMultiplier = 5f;
        spriteRenderer.sortingOrder = -(int)(
            (transform.position.y + positionOffsetY) * precisionMultiplier
        );

        // Only run one time then delete the script
        if (runOnce)
        {
            Destroy(this);
        }
    }
}
