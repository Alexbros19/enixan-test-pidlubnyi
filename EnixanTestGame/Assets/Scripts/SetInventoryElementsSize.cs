using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* this class is using for set inventory elements size when its created
 * its will work properly for different resolution of screen */
public class SetInventoryElementsSize : MonoBehaviour {
    private const float ROW_COUNT = 2f;
    private const float SPACING_SIZE_CONST = 12f;
    private const float PADDING_SIZE_CONST = 24f;

    private void Awake()
    {
        // get rect transform and grid layout group components of content object
        RectTransform parentRect = gameObject.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        // set parameters for grid layout group
        gridLayout.cellSize = new Vector2(parentRect.rect.height / ROW_COUNT - parentRect.rect.height / SPACING_SIZE_CONST, parentRect.rect.height / ROW_COUNT - parentRect.rect.height / SPACING_SIZE_CONST);
        gridLayout.spacing = new Vector2(parentRect.rect.height / SPACING_SIZE_CONST, parentRect.rect.height / SPACING_SIZE_CONST);
        gridLayout.padding.left = Mathf.RoundToInt(parentRect.rect.height / PADDING_SIZE_CONST);
        gridLayout.padding.right = Mathf.RoundToInt(parentRect.rect.height / PADDING_SIZE_CONST);
    }
}
