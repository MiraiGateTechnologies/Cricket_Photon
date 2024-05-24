using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
//using static UnityEditor.PlayerSettings;

public class BallControllerScript : MonoBehaviour
{

    public static BallControllerScript instance;
    public Transform batt;
    public Transform currentBall;
    public Vector3 defaultPosition; // ball's default beginning position
    public Animator BowlerAnimator;
    public float ballSpeed; // speed of the ball
    public GameObject ballPrefab; // stores the ball game object
    public float bounceScalar; // the bounce scalar value to scale the bounce angle after the ball hits the ground
    public float spinScalar; // the ball's spin scalar value
    public float realWorldBallSpeed; // the ball's speed to display on the UI which corresponds to the real world units(kmph)
    //public GameObject trajectoryHolder; // the holder game object to parent each trajectory ball object to

    public int ballType; // the balls type; 0 - straight, 1 - leg spin, 2 - off spin

    [SerializeField] Vector2 fixedPosition;
    [SerializeField] Vector2 marker_minPos; 
    [SerializeField] Vector2 marker_maxPos;
    public GameObject markerPrefab;
    private GameObject currentMarker;
    public GameObject ball;
    private float angle; // the bounce angle of the ball after the ball hits the ground for the first time
    private Vector3 startPosition; // ball's startPosition for the lerp function
    private Vector3 targetPosition; // ball's targetPosition for the lerp function
    private Vector3 direction; // the direction vector the ball is going in
    private Rigidbody rb; // rigidbody of the ball
    private float spinBy; // value to spin the ball by
    private Vector3 markerPosition;
    private bool firstBounce; // whether ball's hit the ground once or not
    private bool isBallThrown; // whether the ball is thrown or not
    private bool isBallHit; // whether the bat hitted the ball
    private bool hasCollided = false;
    public bool isInAir = false;
    // private bool isTrajectoryEnabled; // whether the trajectory is enabled or disabled

    public float BallSpeed { set { ballSpeed = value; } }
    public int BallType { set { ballType = value; } }
    public bool IsBallThrown { get { return isBallThrown; } }
    //public bool IsTrajectoryEnabled { set { isTrajectoryEnabled = value; } get { return isTrajectoryEnabled; } }
    public bool IsBallHit { get { return isBallHit; } }

    public bool isBatsManOut;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        defaultPosition = transform.position; // set defaultPosition to the balls beginning position
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = transform.position;  // set the startPosition to the balls beginning position
        StartCoroutine(ThrowBall());
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject, 2f);
        }

        if (hasCollided)
        {
            // Rotate the ball around a circular path
            Vector3 circularMotion = Quaternion.AngleAxis(5f * Time.deltaTime, Vector3.up) * (transform.position - batt.position);
            transform.position = batt.position + circularMotion;

            // Move the ball forward in the direction it's currently facing
            rb.velocity = transform.forward * 5f * Time.deltaTime;
        }
        // if the isTrajectoryEnabled is set to true and the ball's velocity is greater than 0 
        // i.e its in motion then instantiate trajectory balls at each frame
        //if (rb.velocity.magnitude > 0 && isTrajectoryEnabled)
        //{
        //    GameObject trajectoryBall = Instantiate(ball, transform.position, Quaternion.identity) as GameObject;
        //    trajectoryBall.transform.SetParent(trajectoryHolder.transform); // set the instantiated trajectory ball's parent to the trajectoryHolder object
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collider work///////////////???????????");
        if (isInAir && collision.gameObject.CompareTag("SixRun"))
        {
            ScoreManager.instance.SixRun();
        }
        else if (collision.gameObject.CompareTag("bat"))
        {
            isBallHit = true;
            isInAir = true;
            //GameObject bat = collision.gameObject;
            hasCollided = true;
            Debug.Log("batt hit the ballllllllllllllllllllllllllll");
            // Apply a forward force to the ball
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            ballRigidbody.AddForce(transform.forward * 10f, ForceMode.Impulse);
        }

        else
        {
            //CollisionLogic(collision);
            Debug.Log("collider is working or not ___________________________");
        }
       
    }
    void SpawnMarker()
    {
        if (currentMarker != null)
        {
            Destroy(currentMarker);

        }

        float randTargetX = Random.Range(marker_minPos.x, marker_maxPos.x);
        float randTargetY = Random.Range(marker_minPos.y, marker_maxPos.y);
        //float randTargetz = Random.Range(marker_minPos.z, marker_maxPos.z);
        markerPosition = new Vector3(randTargetX, randTargetY, 0);
        //markerPosition = new Vector3(fixedPosition.x, fixedPosition.y, 0);
        Debug.Log("markerPosition::: " + markerPosition);
        BatPointCollider.instance.ishit = false;
        // Instantiate the marker
        currentMarker = Instantiate(markerPrefab, markerPosition, Quaternion.Euler(90, 0, 0));
        
    }


    IEnumerator ThrowBall()
    {
        if (ball != null)
        {
            Destroy(ball);
            CameraFollow.instance.ResetCamera();
        }
        isBallThrown = false;
        isBatsManOut = false;
        ScoreManager.instance.ResetSprites();
        SpawnMarker();
        BowlerAnimator.SetTrigger("throw");
        yield return new WaitForSeconds(3f);
        if (!IsBallThrown)
        { // if the ball is not thrown, throw the ball
            ball = Instantiate(ballPrefab, defaultPosition, Quaternion.identity);
            Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
            currentBall = ball.transform;
            isBallThrown = true;
            targetPosition = markerPosition; // make the balls target position to the markers position
            direction = Vector3.Normalize(targetPosition - startPosition); // calculate the direction vector
            ballRigidbody.AddForce(direction * ballSpeed, ForceMode.Impulse); // Add an instant force impulse in the direction vector multiplied by ballSpeed to the ball considering its mass
        }
        yield return new WaitForSeconds(6f);
        ScoreManager.instance.UpdateScore(ball);
        yield return new WaitForSeconds(3f);
        StartCoroutine(ThrowBall());
    }

    

    public void HitTheBall(Vector3 hitDirection, float batSpeed)
    {
        //CameraControllerScript.instance.IsBallHit = true; // set CameraControllerScript's isBallHit to true
        isBallHit = true; // set the is ball hit to true
        rb.velocity = Vector3.zero; // set the ball's velocity to zero to stop the ball
        direction = Vector3.Normalize(hitDirection); // normalize the hit direction of the bat
        float hitSpeed = (ballSpeed / 2) + batSpeed; // calculate the balls return speed based on the bats speed and the balls speed
        rb.AddForce(-direction * hitSpeed, ForceMode.Impulse); // Add an instant force impulse in the negative direction vector multiplied by ballSpeed to the ball considering its mass
        if (!firstBounce)
        { // if the ball has never hitted the ground then set the ball's rigidbody to be affected by gravity
            rb.useGravity = true;
        }
    }

    public void SwitchSide()
    {
        transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z); // negate the x value of balls position to change the side
        defaultPosition = transform.position; // reset the default position to new balls position
        startPosition = transform.position; // reset the default position to new balls position
    }

    public void ResetBall()
    { // reset the values
        firstBounce = false;
        isBallHit = false;
        isBallThrown = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = defaultPosition;

    }
}
