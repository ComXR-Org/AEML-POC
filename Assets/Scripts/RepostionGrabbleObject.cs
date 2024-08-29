using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepostionGrabbleObject : MonoBehaviour
{
    private Vector3 myPosition;
    private Vector3 myRotation;

  

    private void Start()
    {
        this.myPosition = this.transform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(AnimHash.TOOL))
        {
            transform.position = myPosition;
            transform.eulerAngles = myRotation;
            Debug.Log("Debug");
        }
        else
        {
            Debug.Log("Debug 1");
        }
        
    }



}
