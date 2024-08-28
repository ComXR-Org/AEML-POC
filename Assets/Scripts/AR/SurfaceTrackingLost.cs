using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SurfaceTrackingLost : MonoBehaviour
{
    public ARSession ARSession;        
    public GameObject TrackingLostWarning;    

    private void Start()
    {
        //SceneObjectOnOff sceneObjectOnOff = FindObjectOfType<SceneObjectOnOff>();
        //TrackingLostWarning = sceneObjectOnOff.trackingLostWarningUI;
        //TrackingLostWarning.SetActive(false);        

    }

    private void Update()
    {
        
        TrackingCheck();
       // RDC.UITextForDEBUG.text = ARSession.notTrackingReason.ToString();
    }

    public void TrackingCheck()
    {
        switch (ARSession.notTrackingReason)
        {
            case NotTrackingReason.None:
                TrackingLost(false);
                break;
            case NotTrackingReason.InsufficientLight:
                TrackingLost(true);
               /* firebase.LogSurfaceTrackingLost("InsufficientLight");
                Mixpanel.Track("Tracking lost due to InsufficientLight");*/
                break;
            case NotTrackingReason.ExcessiveMotion:
                TrackingLost(true);
               /* firebase.LogSurfaceTrackingLost("ExcessiveMotion");
                Mixpanel.Track("Tracking lost due to ExcessiveMotion");*/
                break;
            case NotTrackingReason.CameraUnavailable:
                TrackingLost(true);
                /*firebase.LogSurfaceTrackingLost("CameraUnavailable");
                Mixpanel.Track("Tracking lost due to CameraUnavailable");*/
                break;
            case NotTrackingReason.InsufficientFeatures:
                TrackingLost(true);
                /*firebase.LogSurfaceTrackingLost("InsufficientFeatures");
                Mixpanel.Track("Tracking lost due to InsufficientFeatures");*/
                break;
        }        
    }
    public void TrackingLost(bool isActive)
    {
        //if(TrackingLostWarning) TrackingLostWarning.SetActive(isActive);
    }

    private void OnDisable()
    {
        TrackingLost(false);
    }
    //float BikeDistance = Vector3.Distance(bike.transform.position, initPosition);
}
