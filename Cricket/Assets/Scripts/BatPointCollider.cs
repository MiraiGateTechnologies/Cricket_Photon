using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BatPointCollider : MonoBehaviour
{
    public static BatPointCollider instance;
    public float powerVlaue;
    public Rigidbody ballrb;
    public Transform ball;
    public bool ishit;
    
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        ishit = false;
    }

    void OnCollisionEnter(Collision collision)
    {
       
        if (collision.collider.CompareTag("ball"))
        {
            ishit = true;
            Vector3 batCenter = transform.position;
            Vector3 ballCenter = collision.collider.bounds.center;
            Vector3 hitDirection = (ballCenter - batCenter).normalized;
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            StartCoroutine(CameraFollow.instance.CameraMovement());

            //CameraFollow.instance.target = ball.transform;
            CameraFollow.instance.ball_rig = ballrb;
            rb.velocity = Vector3.zero; // set the ball's velocity to zero to stop the ball
            float hitSpeed = (BallControllerScript.instance.ballSpeed / 2) + powerVlaue; // calculate the balls return speed based on the bats speed and the balls speed
            rb.AddForce(-hitDirection * hitSpeed, ForceMode.Impulse); // Add an instant force impulse in the negative direction vector multiplied by ballSpeed to the ball considering its mass
            
            ballrb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            Debug.Log("Name ::"+gameObject.name);
            //batcollider1.SetActive(false);
            //batcollider2.SetActive(false);
        }
    }
}
