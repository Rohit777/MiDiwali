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

    private bool objectPlaced = false;

    ARRaycastManager m_RaycastManager;


    private PlaneDetectionController planeDetectionController;

    public static int Count = 0;

    [SerializeField]
    Animator m_MoveDeviceAnimation;
    public Animator moveDeviceAnimation
    {
        get { return m_MoveDeviceAnimation; }
        set { m_MoveDeviceAnimation = value; }
    }

    [SerializeField]
    Animator m_TapToPlaceAnimation;
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


    private float timeRemaining = 300;
    public bool timerIsRunning = false;
    public Text timeText;

    private bool winning = false;
    public Text wordCloudText;
    public GameObject wordcloud;

    [SerializeField]
    ARPlaneManager m_PlaneManager;
    public ARPlaneManager planeManager
    {
        get { return m_PlaneManager; }
        set { m_PlaneManager = value; }
    }

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
    }

    private void Update()
    {
        Debug.Log(objectPlaced);
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
                        Pose hitPose = s_Hits[0].pose;

                        spawnedObject = Instantiate(prefabs[Count].prefab, hitPose.position, hitPose.rotation);

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

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                if (Count == 9)
                {
                    Debug.Log("you won");
                    NonARview.SetActive(true);
                    win.SetActive(true);
                    ARview.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
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
        Destroy(spawnedObject);
        wordcloud.SetActive(true);
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

}
