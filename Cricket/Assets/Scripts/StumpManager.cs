using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StumpBailManager : MonoBehaviour
{
    public GameObject[] stumps; 
    public GameObject[] bails; 

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> originalRotations = new Dictionary<GameObject, Quaternion>();

    void Start()
    {
        foreach (var stump in stumps)
        {
            originalPositions[stump] = stump.transform.position;
            originalRotations[stump] = stump.transform.rotation;
        }

        foreach (var bail in bails)
        {
            originalPositions[bail] = bail.transform.position;
            originalRotations[bail] = bail.transform.rotation;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            Debug.Log("Wicket");
            BallControllerScript.instance.isBatsManOut = true;
            ScoreManager.instance.bowled.enabled = true;
            ScoreManager.instance.umpireAnimator.SetTrigger("Out");
            int index = UIHandler.Instance.currentBallIndex;
            UIHandler.Instance.wikcetCount += 1;
            UIHandler.Instance.UpdateBallsText(index+1, "W");
            UIHandler.Instance.currentBallIndex += 1;

            foreach (var stump in stumps)
            { 
                Rigidbody rb = stump.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                ApplyForceToStump(rb, collision);
            }

            foreach(var bail in bails)
            {
                Rigidbody rb = bail.GetComponent<Rigidbody>();
                rb.isKinematic = false;
            }
            
            StartCoroutine(ResetStumpsAndBails());
        }
    }

    private void ApplyForceToStump(Rigidbody stumpRigidbody, Collision collision)
    {
        float impactForce = collision.rigidbody.velocity.magnitude;
        Vector3 direction = collision.contacts[0].point - stumpRigidbody.transform.position;
        direction = -direction.normalized;

        float forceMagnitude = impactForce * 0.5f; // Adjust this multiplier as needed
        stumpRigidbody.AddForceAtPosition(direction * forceMagnitude, collision.contacts[0].point, ForceMode.Impulse);
    }

    private IEnumerator ResetStumpsAndBails()
    {
        yield return new WaitForSeconds(3f);
        
        foreach (var stump in stumps)
        {
            Rigidbody rb = stump.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            stump.transform.position = originalPositions[stump];
            stump.transform.rotation = originalRotations[stump];
        }

        foreach (var bail in bails)
        {
            Rigidbody rb = bail.GetComponent<Rigidbody>();
            rb.isKinematic = true; 
            bail.transform.position = originalPositions[bail];
            bail.transform.rotation = originalRotations[bail];
        }
    }
}
