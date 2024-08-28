using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public GameObject plane;
    //SceneObjectOnOff sceneObjectOnOff;

    public bool isTracking;
    public GameObject spawnedObject;
   // public CountDownForARStart Countdown;
    public PlacementIndicator placementIndicator;
    public GameObject VehicleDetailsContainer;
    //  public BikeZoneInteraction zoneScript;
    public bool isTutorial;
    public ARPlaneManager aRPlaneManager;
    public TextMeshProUGUI Text;
    private void OnEnable()
    {
        //Countdown.gameObject.SetActive(true);
    }
    void Start()
    {
       // objectToSpawn = GameObject.FindGameObjectWithTag("BikeParent");
       // sceneObjectOnOff = FindObjectOfType<SceneObjectOnOff>();
        isTracking = true;
        
       Invoke("NewARPlaneUpdater", 2f);
        // zoneScript = FindObjectOfType<BikeZoneInteraction>();
    /*    Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1120, 480, true, 60);
        Application.targetFrameRate = 30;*/
    }

   void Update()
    {
        /*if (sceneObjectOnOff.surfacetutorial.activeInHierarchy)
        {
            isTutorial = true;
        }
        else
        {
            isTutorial = false;
        }
        if (placementIndicator == null)
        {
            placementIndicator = FindObjectOfType<PlacementIndicator>(true);
        }*/
        //Instantiating Object on touch here


        if (Input.GetMouseButton(0) && plane.activeSelf)
        {
            Activate();
            Text.text = "Touched";
        }

       /* if (isTutorial == false *//*&& Countdown.isCounDownOver == false*//*)
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended || Input.GetMouseButton(0))
            {
                
                *//*Debug.Log("Touch Screen");
                if (EventSystem.current.currentSelectedGameObject)
                { //Return if touching UI
                    return;
                }
                else
                {
                   *//* foreach (GameObject g in sceneObjectOnOff.arScalar)
                    {
                        if (!g.activeInHierarchy)
                            g.SetActive(true);
                        else
                            return;
                    }*//*
                    Debug.Log(" Activate Object ");
                    Activate();
                }
*//*
            }
        }*/
        //Enable if using a reSpawn button
        /*if (respawnButton == null) {
            respawnButton = GameObject.Find("Button_ReSpawn");
            //Debug.Log(respawnButton.name);
            respawnButton.GetComponent<Button>().onClick.AddListener(() => { ReSpawn(); });
        }*/

    }
    public void offsurfacetracking()
    {
        Destroy(spawnedObject);
    }
    public void Activate() {

        if (isTracking)
        {
            if (!objectToSpawn.activeSelf)
            {
                Debug.Log(" spawnOnject null ");
                Text.text += "\n Object Spwanned";
                ToggelPlaneEnableDisable();
                objectToSpawn.SetActive(true);
                objectToSpawn.transform.position = placementIndicator.transform.position;
                objectToSpawn.transform.rotation = placementIndicator.transform.rotation;
                placementIndicator.gameObject.SetActive(false);
                isTracking = false;
            }
            else
            {
                objectToSpawn.transform.position = placementIndicator.transform.position;
                objectToSpawn.transform.rotation = placementIndicator.transform.rotation;
                placementIndicator.gameObject.SetActive(false);
                Text.text += "\n Position Adjusted";
                isTracking = false;
            }

        }


       /* if (isTracking) {  //isTracking will only be true in the start or if Respawn button is pressed
            if (!objectToSpawn.activeSelf)
            {    //spawnedObject will be null only if the object is not instantiated before.
                Debug.Log(" spawnOnject null ");
                //Instantiating the object here
              //  GameObject obj = Instantiate(objectToSpawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
                
                //objectToSpawn.transform.position = placementIndicator.transform.position;
                //objectToSpawn.transform.rotation = placementIndicator.transform.rotation;
                ToggelPlaneEnableDisable();


                // GameObject obj = objectToSpawn;
                *//*     obj.transform.position = placementIndicator.transform.position;
                     obj.transform.rotation = placementIndicator.transform.rotation;*//*
                objectToSpawn.SetActive(true);
                
             //Set isTracking to false so it doesn't instantiate multiple object
                placementIndicator.gameObject.SetActive(false);
                isTracking = false;
              *//*  foreach (GameObject g in sceneObjectOnOff.arScalar)
                {
                    g.SetActive(true);
                }*//*
                //Code to add Onclick event to enable edit panel.
                *//*   if (settingsButton == null) {
                       settingsButton = GameObject.Find("Button_Edit");
                       settingsButton.GetComponent<Button>().onClick.AddListener(() => { FindObjectOfType<PlacementEditor>().EditPanelSwitch(); });
                   }*//*

                //Adding all the labels in Zone selection scene to the list in BikeZoneInteraction script
                //zoneScript.AddZonesToList();
                // Debug.Log(zoneScript.sceneToLoad);
                //respawnButton.SetActive(true);
                *//* settingsButton.SetActive(true);*//* //Settings button is hidden and used to edit scale rotation and position of the object
            } else {

                //Change the position to placement incdicator posion if the object is already spawned
              
                objectToSpawn.transform.position = placementIndicator.transform.position;
                objectToSpawn.transform.rotation = placementIndicator.transform.rotation;
                *//*objectToSpawn.SetActive(true);*//*
                placementIndicator.gameObject.SetActive(false);
                isTracking = false;
              *//*  foreach (GameObject g in sceneObjectOnOff.arScalar)
                {
                    g.SetActive(true);
                }*/
                /*  respawnButton.SetActive(true);*//*
            }
            
            
            
        }*/
        
    }
    public void ReSpawn() {
        /*foreach (GameObject g in sceneObjectOnOff.arScalar)
        {
            g.SetActive(false);
        }*/
        /* respawnButton.SetActive(false);*/
        isTracking = true;
        ToggelPlaneEnableDisable();
        objectToSpawn.SetActive(false);
        placementIndicator.gameObject.SetActive(true);
      

    }
    
    public void ToggelPlaneEnableDisable()
    {
        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        foreach (ARPlane plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(aRPlaneManager.enabled);
        }
    }
   

    public void NewARPlaneUpdater()
    {
        aRPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // Disable all planes except the first one
        bool firstPlaneFound = false;

        foreach (var plane in args.added)
        {
            if (!firstPlaneFound)
            {
                firstPlaneFound = true;
                continue;
            }

            plane.gameObject.SetActive(false);
        }

        foreach (var plane in args.updated)
        {
            if (!firstPlaneFound)
            {
                firstPlaneFound = true;
                continue;
            }

            plane.gameObject.SetActive(false);
        }

    }
}
