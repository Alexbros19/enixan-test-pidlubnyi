using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCamera : MonoBehaviour {
    [SerializeField]
    private float speed = 1f;

    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && !GameUIManager.IsMagazineInventoryActive)
        {
            // calculate a delta position since last change
            Vector3 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            // move camera 
            transform.Translate(touchDeltaPosition.x * -speed * Time.deltaTime, touchDeltaPosition.y * -speed * Time.deltaTime, 0);
        }
    }
}
