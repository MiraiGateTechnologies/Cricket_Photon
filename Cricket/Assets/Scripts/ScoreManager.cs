using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Animator umpireAnimator;
    public Image for1To4RunsImg;
    public Image for6RunsImg;
    public Image bowled;
    public List<Sprite> runsSprites;   
    public int currentScore;
    public float velocityThreshold = 0.1f;
    public Vector3 centerPos;

    public bool isBallHit;
   
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        centerPos = Vector3.zero;
    }
   

    public void UpdateScore(GameObject currentBall) 
    {
        if (BallControllerScript.instance.isBatsManOut)
        {
            return;
        }
        if (currentBall != null)
        {
            float distance = Vector3.Distance(currentBall.transform.position, centerPos);
            CalculateRuns(distance);
            Debug.Log("Distance from batsman: " + distance);
        }
         
    }
    void CalculateRuns(float hitDistance)
    {
        if (hitDistance < 35f)
        {
            for1To4RunsImg.sprite = runsSprites[0];
            for1To4RunsImg.enabled = true;
            UIHandler.Instance.GetHitcount(1);
            Debug.Log("Current run 1");
            //return 1;
        }
        else if (hitDistance >= 10f && hitDistance < 45f)
        {
            for1To4RunsImg.sprite = runsSprites[1];
            for1To4RunsImg.enabled = true;
            UIHandler.Instance.GetHitcount(2);
            Debug.Log("Current run 2");
           // return 2;
        }
        else if (hitDistance >= 30f && hitDistance < 90f)
        {

            for1To4RunsImg.sprite = runsSprites[2];
            for1To4RunsImg.enabled = true;
            UIHandler.Instance.GetHitcount(3);
            Debug.Log("Current run 3");

        }else if (hitDistance >= 40f && hitDistance < 120f)
        {

            for1To4RunsImg.sprite = runsSprites[3];
            for1To4RunsImg.enabled = true;
            UIHandler.Instance.GetHitcount(4);
            umpireAnimator.SetTrigger("Four");
            Debug.Log("Current run 4");

        }
        else 
        {
            for1To4RunsImg.sprite = runsSprites[4];
            for1To4RunsImg.enabled = true;
            UIHandler.Instance.GetHitcount(6);
            umpireAnimator.SetTrigger("Six");
            Debug.Log("Current run 4");
            //return 4;
        }
        
    }

    public void SixRun()
    {
        for1To4RunsImg.sprite = runsSprites[6];
        for1To4RunsImg.enabled = true;
        Debug.Log("Current run 6");
    }

    public void ResetSprites()
    {
        for1To4RunsImg.sprite = null;
        for1To4RunsImg.enabled = false;
        bowled.enabled = false;
    }
}
