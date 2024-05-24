using UnityEngine;

public class CricketBowler : MonoBehaviour
{
    public enum BowlingType { Straight, Yorker, OffSpin, LegSpin }

    public GameObject ballPrefab; 
    public Transform throwPoint; 

    [Range(15f, 50f)]
    public float straightForce = 10f;

    [Range(15f, 25f)]
    public float yorkerForce = 12f;

    [Range(8f,150f)]
    public float spinForce = 8f;
   
    [Range(0.1f, 5.0f)]
    public float spinTorque = 8f;

    [Range(0.1f, 5.0f)]
    public float angularDrag = 8f;

    public BowlingType currentDeliveryType; 

    GameObject currentBall;
    // Function to bowl the ball
    public void BowlBall()
    {
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();

        
        Vector3 force = Vector3.zero;
        switch (currentDeliveryType)
        {
            case BowlingType.Straight:
                force = throwPoint.forward * straightForce;
                break;
            case BowlingType.Yorker:
                force = throwPoint.forward * yorkerForce + throwPoint.up * -2f;
                break;
            case BowlingType.OffSpin:
                force = (throwPoint.forward + throwPoint.right * 0.5f).normalized * spinForce;
                break;
            case BowlingType.LegSpin:
                force = (throwPoint.forward - throwPoint.right * 0.5f).normalized * spinForce;
                break;
        }
        currentBall = ball;
       
        ballRigidbody.AddForce(force, ForceMode.Impulse);
        Vector3 torque = Vector3.zero;
        switch (currentDeliveryType)
        {
            case BowlingType.OffSpin:
                torque = throwPoint.up * spinTorque;
                break;
            case BowlingType.LegSpin:
                torque = -throwPoint.right * spinTorque;
                break;
        }

        ballRigidbody.AddTorque(torque, ForceMode.Impulse);

        // Adjust angular drag for realistic spin behavior
        ballRigidbody.angularDrag = angularDrag;
    }

    public void SetDeliveryType(BowlingType type)
    {
        currentDeliveryType = type;
    }
    
    
}
