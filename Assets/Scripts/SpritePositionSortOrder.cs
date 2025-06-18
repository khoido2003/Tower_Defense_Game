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
        spriteRenderer = GetComponent<SpriteRenderer>();
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
