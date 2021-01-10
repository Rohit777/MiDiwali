using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Touch touch;
    private Vector2 tuchPosition;
    private Quaternion rotationY;
    bool isOverUI;


    private float roateSpeedModifier = 0.1f;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            var touchPosition = touch.position;
            isOverUI = touchPosition.IsPointOverUIObject();

            if (!isOverUI && touch.phase == TouchPhase.Moved)
            {
                rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * roateSpeedModifier, 0f);
                transform.rotation = rotationY * transform.rotation;
            }
        }
    }
}
