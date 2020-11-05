using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

public class ARTapToPlace : MonoBehaviour
{
    [Serializable]
    public struct prefabNAme
    {
        public string name;
        public GameObject prefab;
        public string mssg;
    }
    public prefabNAme[] prefabs;
    bool winning = false;
    
   
    private ARRaycastManager arMangaer;
    ARSession ARSession;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private PlaneDetectionController planeDetectionController;
    public GameObject spawnedObject { get; private set; }


    public static int count = 0;
    private bool objectPlaced = false;

    const string k_FadeOffAnim = "FadeOff";
    const string k_FadeOnAnim = "FadeOn";

    public Text wordCloudText;
    public GameObject worldCoud;
    public GameObject tick;


    private float timeRemaining = 300;
    public bool timerIsRunning = false;
    public Text timeText;

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

    bool m_ShowingTapToPlace = false;
    bool m_ShowingMoveDevice = true;

    void Awake()
    {
        arMangaer = FindObjectOfType<ARRaycastManager>();
    }

    private void Start()
    {
        wordCloudText.text = prefabs[count].mssg;
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacemntIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var touchPosition = Input.GetTouch(0).position;
            bool isOverUI = touchPosition.IsPointOverUIObject();
            if (!isOverUI && !objectPlaced)
            {
                PlaceObject();
                objectPlaced = true;
            }
        }

        if(count < prefabs.Length)
        {
            wordCloudText.text = prefabs[count].mssg;
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                if(winning == true)
                {
                    Debug.Log("you won");
                    worldCoud.SetActive(true);
                    wordCloudText.text = "YOU WON";

                }
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                worldCoud.SetActive(true);
                wordCloudText.text = "YOU LOOSE";
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void onTouch(GameObject objectToHide)
    {
        objectToHide.SetActive(false);
    }

    public void onCapture()
    {
        count++;
        if (count< prefabs.Length)
        {
            objectPlaced = false;
            if (moveDeviceAnimation)
                moveDeviceAnimation.SetTrigger(k_FadeOnAnim);
            m_ShowingMoveDevice = true;
            worldCoud.SetActive(true);
            Destroy(spawnedObject);
        }
        else
        {
            winning = true;
        }
    }

    public void onclick()
    {
        planeDetectionController.SetAllPlanesActive(true);

    }
    private void PlaceObject()
    {
        Pose hitPose = hits[0].pose;
        spawnedObject = Instantiate(prefabs[count].prefab, hitPose.position, hitPose.rotation);
        planeDetectionController.SetAllPlanesActive(true);
        if (moveDeviceAnimation)
            moveDeviceAnimation.SetTrigger(k_FadeOffAnim);

        if (tapToPlaceAnimation)
            tapToPlaceAnimation.SetTrigger(k_FadeOffAnim);

        m_ShowingTapToPlace = false;
        m_ShowingMoveDevice = false;
    }

    private void UpdatePlacemntIndicator()
    {
        if (placementPoseIsValid)
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
                //placementIndicator.SetActive(false);
            }
        }
        else
        {
            //placementIndicator.SetActive(false);
            if (!objectPlaced)
            {
                timerIsRunning = false;
            }
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

    private void UpdatePlacementPose()
    {
        //var screenCentre = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        //arMangaer.Raycast(screenCentre, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}
