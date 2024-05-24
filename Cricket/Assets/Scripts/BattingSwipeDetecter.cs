using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattingSwipeDetecter : MonoBehaviour
{
    private Vector2 initialInputPosition;
    private Vector2 swipeDirection;
    public TextMeshProUGUI tempText;
    public float minSwipeDistance = 100f;

    void Update()
    {
#if UNITY_EDITOR
        HandleEditorInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleEditorInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialInputPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeDirection = (Vector2)Input.mousePosition - initialInputPosition;
            float swipeDistance = swipeDirection.magnitude;

            if (swipeDistance > minSwipeDistance)
            {
                swipeDirection.Normalize();
                DetermineBattingShot();
            }
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialInputPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeDirection = touch.position - initialInputPosition;
                float swipeDistance = swipeDirection.magnitude;

                if (swipeDistance > minSwipeDistance)
                {
                    swipeDirection.Normalize();
                    DetermineBattingShot();
                }
            }
        }
    }

    void DetermineBattingShot()
    {
        if (swipeDirection.y > 0.5f) // Swipe from bottom to top
        {
            // Straight shot
            ExecuteStraightShot();
        }
        else if (swipeDirection.y < -0.5f) // Swipe from top to bottom
        {
            // Backward shot
            ExecuteBackwardShot();
        }
        else if (swipeDirection.x > 0.5f) // Swipe from left to right
        {
            // Off-side shot
            ExecuteOffSideShot();
        }
        else if (swipeDirection.x < -0.5f) // Swipe from right to left
        {
            // Leg-side shot
            ExecuteLegSideShot();
        }
    }

    // Implement your shot execution methods here
    void ExecuteStraightShot()
    {
        tempText.text = "Executing straight shot";
        Debug.Log("Executing straight shot");
    }

    void ExecuteBackwardShot()
    {
        tempText.text = "Executing backward shot";
        Debug.Log("Executing backward shot");
    }

    void ExecuteOffSideShot()
    {

        tempText.text = "Executing off-side shot";
        Debug.Log("Executing off-side shot");
    }

    void ExecuteLegSideShot()
    {
        tempText.text = "Executing leg-side shot";
        Debug.Log("Executing leg-side shot");
    }
}
