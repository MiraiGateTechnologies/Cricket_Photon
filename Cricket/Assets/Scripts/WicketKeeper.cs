using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicketKeeper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ball"))
        {
            // Destroy the ball when it collides with the "wk" tag
            Destroy(other.gameObject);
           // CameraFollow.instance.ResetCamera();
        }
    }
}
