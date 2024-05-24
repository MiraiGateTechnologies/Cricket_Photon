using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Net.Http.Headers;
using TMPro;
//using UnityEditor.Animations;
using UnityEngine;

public class BatControllerScript : MonoBehaviour
{
    public static BatControllerScript instance;
    public GameObject ball;
    public float batPower;
    public float batsmanReachLimitMax;
    public float leftMoveLimit;
    public float rightMoveLimit;
    private bool hasReachedBall = false;
    public float desiredDistance;
    private Vector3 resetPosition;
    private Vector2 initialInputPosition;
    private Vector2 swipeDirection;
    public float minSwipeDistance = 100f;
    public List<BoxCollider> batColliders;
    Animator batsmanAnimator;
    public Action<string> getPower;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        resetPosition = transform.position;
        batsmanAnimator = GetComponent<Animator>();
        //foreach (BoxCollider collider in batColliders) { collider.enabled = false; }
    }

    // Update is called once per frame
    void Update()
    {
        //HandleEditorInput();
        HandleEditorInput();
        if (BallControllerScript.instance.IsBallThrown)
        {
           // Invoke("SetBatsmanPosition", 0.5f);
           

        }
        else
        {
            ResetBatsman();
        }

    }
    void SetBatsmanPosition()
    {
        if (BallControllerScript.instance.currentBall != null)
            if (!hasReachedBall && BallControllerScript.instance.currentBall.transform.position.z <= batsmanReachLimitMax)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, leftMoveLimit, rightMoveLimit));

                transform.position = new Vector3(transform.position.x,
                    transform.transform.position.y,
                    BallControllerScript.instance.currentBall.transform.position.z);
                if (Vector3.Distance(transform.position, BallControllerScript.instance.currentBall.position) <= desiredDistance)
                {
                    hasReachedBall = true;
                }

            }
    }
    void HandleEditorInput()
    {
        //foreach (BoxCollider collider in batColliders) { collider.enabled = true; }
        if (Input.GetMouseButtonDown(0))
        {
            initialInputPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeDirection = (Vector2)Input.mousePosition - initialInputPosition;
            float swipeDistance = swipeDirection.magnitude;
           

            //Debug.Log(swipeDistance);
            if (swipeDistance > minSwipeDistance)
            {
                swipeDirection.Normalize();
                DetermineBattingShot();
            }
        }
    }
    void DetermineBattingShot()
    {
        if (swipeDirection.y > 0.5f) 
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
        batsmanAnimator.SetTrigger("Stright");
    }

    void ExecuteBackwardShot()
    {
        Debug.Log("Executing backward shot");
    }

    void ExecuteOffSideShot()
    {
        batsmanAnimator.SetTrigger("offSide");
        Debug.Log("Executing off-side shot");
    }

    void ExecuteLegSideShot()
    {
        batsmanAnimator.SetTrigger("onSide");
        Debug.Log("Executing leg-side shot");
    }
    void ResetBatsman()
    {
        transform.position = resetPosition;
        hasReachedBall = false;
       // foreach (BoxCollider collider in batColliders) { collider.enabled = false; }
    }
    
    


}
