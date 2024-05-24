using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    [SerializeField] Transform[] ballHitPos;// position after ball is hitted
    [HideInInspector] public Rigidbody ball_rig;
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 startAngle; // The start position of the camera used to reset
    private Transform target; // Reference to the target (ball) to follow
    public Vector3 offset;
    public bool isMove = false;// Offset of the camera from the target
    //private Camera mainCamera;
    public void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        //offset = transform.position - target.position;
        //mainCamera = Camera.main;
    }
    private void Update()
    {
        if (isMove == true)
        {
             Debug.Log(" ismove truuuuuuuuuuuuuuuuuuuuueeeeeeeee ");
            target = BallControllerScript.instance.ball.transform;
            //transform.LookAt(target);
            Vector3 relativePos = target.position - transform.position;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;

           
        }
    }
    // Update is called once per frame
    public IEnumerator CameraMovement()
    {
        if (BatPointCollider.instance.ishit == true)
        {
            yield return new WaitForSeconds(0.1f);
            //float sharpness = 5f;
            transform.position = ballHitPos[Random.Range(0, ballHitPos.Length)].position;
            isMove = true;
           
        }
    }

    public void ResetCamera()
    {
        isMove = false;
        BatPointCollider.instance.ishit = false;
        transform.position = startPos;
        transform.rotation = Quaternion.Euler(startAngle);  // Quaternion.identity;
        target = null;
        ball_rig = null;
        GetComponent<DollyZoom>().ResetZoom();
    }
}
