using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTap : MonoBehaviour
{
    [Serializable]
    public struct prefabNAme
    {
        public string name;
        public GameObject prefab;
        public string mssg;
    }
    public prefabNAme[] prefabs;

    public GameObject spawnedObject { get; private set; }
    public GameObject objectToPlace;

    private bool objectPlaced = false;

    ARRaycastManager m_RaycastManager;
    private Pose placementPose;
    public GameObject placementIndicator;


    private PlaneDetectionController planeDetectionController;

    public static int Count = 0;

    [SerializeField]
    Animator m_MoveDeviceAnimation;
    public GameObject MoveDevice;
    public Animator moveDeviceAnimation
    {
        get { return m_MoveDeviceAnimation; }
        set { m_MoveDeviceAnimation = value; }
    }

    [SerializeField]
    Animator m_TapToPlaceAnimation;
    public GameObject TapDevice;
    public Animator tapToPlaceAnimation
    {
        get { return m_TapToPlaceAnimation; }
        set { m_TapToPlaceAnimation = value; }
    }
    const string k_FadeOffAnim = "FadeOff";
    const string k_FadeOnAnim = "FadeOn";
    bool m_ShowingTapToPlace = false;
    bool m_ShowingMoveDevice = true;

    private bool placementPoseIsValid = false;


    private float timeRemaining = 600;
    public bool timerIsRunning = false;
    public Text timeText;

    private bool winning = false;
    public Text wordCloudText;
    public Text wordCloudInstruction;
    public GameObject wordcloud;

    [SerializeField]
    ARPlaneManager m_PlaneManager;
    public ARPlaneManager planeManager
    {
        get { return m_PlaneManager; }
        set { m_PlaneManager = value; }
    }
    [SerializeField]
    private GameObject tick;
    [SerializeField]
    private GameObject crosss;

    public GameObject ARview;
    public GameObject NonARview;
    public GameObject lose;
    public GameObject win;

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        planeDetectionController = FindObjectOfType<PlaneDetectionController>();

    }

    private void OnEnable()
    {
        Count = 0;
        timeRemaining = 600;
        if (spawnedObject != null)
        {
            tick.SetActive(false);
            crosss.SetActive(false);
            objectPlaced = false;
            planeDetectionController.SetAllPlanesActive(true);
            Destroy(spawnedObject);
        }
    }

    private void Update()
    {
        Debug.Log(objectPlaced);
        UpdatePlacementPose();
        UpdatePlacemntIndicator();
        animationUpdate();
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    var touchPosition = Input.GetTouch(0).position;
                    bool isOverUI = touchPosition.IsPointOverUIObject();
                    if (!objectPlaced && !isOverUI)
                    {
                        spawnedObject = Instantiate(prefabs[Count].prefab, objectToPlace.transform.position, objectToPlace.transform.rotation);
                        spawnedObject.transform.parent = objectToPlace.transform;
                        objectPlaced = true;
                        planeDetectionController.SetAllPlanesActive(false);
                    }
                }
            }
        }


        if (Count < prefabs.Length)
        {
            wordCloudText.text = prefabs[Count].mssg;
        }

        if(Count > 0)
        {
            wordCloudInstruction.text = "Tap To Continue";
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                if (Count == 8)
                {
                    Debug.Log("you won");
                    NonARview.SetActive(true);
                    FindObjectOfType<AudioManager>().Play("win");
                    win.SetActive(true);
                    ARview.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                FindObjectOfType<AudioManager>().Play("loose");
                NonARview.SetActive(true);
                lose.SetActive(true);
                ARview.SetActive(false);
            }
        }
    }

   
    private void animationUpdate()
    {
        if(PlanesFound())
        {
            if (!objectPlaced)
            {
                timerIsRunning = true;
                if (m_ShowingMoveDevice)
                {
                    if (moveDeviceAnimation)
                        moveDeviceAnimation.SetTrigger(k_FadeOffAnim);

                    if (tapToPlaceAnimation)
                        tapToPlaceAnimation.SetTrigger(k_FadeOnAnim);

                    m_ShowingTapToPlace = true;
                    m_ShowingMoveDevice = false;
                }
            }
            else
            {
                MoveDevice.SetActive(false);
                TapDevice.SetActive(false);
            }
        }
        else
        {
           timerIsRunning = false;
            if (m_ShowingTapToPlace)
            {
                if (moveDeviceAnimation)
                    moveDeviceAnimation.SetTrigger(k_FadeOnAnim);

                if (tapToPlaceAnimation)
                    tapToPlaceAnimation.SetTrigger(k_FadeOffAnim);

                m_ShowingTapToPlace = false;
                m_ShowingMoveDevice = true;
            }
        }
    }

    private void UpdatePlacemntIndicator()
    {
        if (placementPoseIsValid)
        {
            if (!objectPlaced)
            {
                placementIndicator.SetActive(true);
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
                objectToPlace.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

            }
            else
            {
                placementIndicator.SetActive(false);
            }
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    bool PlanesFound()
    {
        if (planeManager == null)
            return false;

        return planeManager.trackables.count > 0;
    }



    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public void reset()
    {
        Count++;
        Debug.Log(Count);

        objectPlaced = false;
        planeDetectionController.SetAllPlanesActive(true);
        Destroy(objectToPlace.transform.GetChild(0).gameObject);
        wordcloud.SetActive(true);
    }

    public void cross()
    {
        objectPlaced = false;
        planeDetectionController.SetAllPlanesActive(true);
        Destroy(objectToPlace.transform.GetChild(0).gameObject);
        wordcloud.SetActive(true);
    }

    private void UpdatePlacementPose()
    {
        var screenCentre = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        m_RaycastManager.Raycast(screenCentre, s_Hits, TrackableType.Planes);

        placementPoseIsValid = s_Hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = s_Hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

}
