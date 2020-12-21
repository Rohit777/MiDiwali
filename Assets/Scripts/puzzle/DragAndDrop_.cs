using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragAndDrop_ : MonoBehaviour
{
    public GameObject SelectedPiece;
    public int PlacedPieces = 0;
    private float timeRemaining = 30;
    public bool timerIsRunning = false;
    public Text timeText;
    public bool winning = false;
    public GameObject Loose;
    public GameObject Win;
    public GameObject puzzlePice;
    public GameObject popup;
    public GameObject PopupText;
    public GameObject timerUi;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && timerIsRunning)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform.CompareTag("Puzzle"))
            {
                if (!hit.transform.GetComponent<piceseScript>().InRightPosition)
                {
                    SelectedPiece = hit.transform.gameObject;
                    FindObjectOfType<AudioManager>().Play("puzzlePiece");
                    SelectedPiece.GetComponent<piceseScript>().Selected = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.GetComponent<piceseScript>().Selected = false;
                SelectedPiece = null;
            }
        }
        if (SelectedPiece != null)
        {
            SelectedPiece.transform.position = SelectedPiece.GetComponent<piceseScript>().RightPosition;
        }
        Debug.Log(PlacedPieces);

        if (PlacedPieces == 12)
        {
            winning = true;
            Debug.Log(PlacedPieces);
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                if (winning == true)
                {
                    timerIsRunning = false;
                    FindObjectOfType<AudioManager>().stop("timer");
                    FindObjectOfType<AudioManager>().Play("win");
                    Debug.Log("you won");
                    popup.SetActive(false);
                    puzzlePice.SetActive(false);
                    Win.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                FindObjectOfType<AudioManager>().stop("timer");
                FindObjectOfType<AudioManager>().Play("loose");
                timerIsRunning = false;
                popup.SetActive(false);
                puzzlePice.SetActive(false);
                Loose.SetActive(true);
            }
        }
    }

    public void retry()
    {
        timeRemaining = 30;
        PlacedPieces = 0;
        PopupText.SetActive(true);
        Loose.SetActive(false);
        Win.SetActive(false);
        timerUi.SetActive(false);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void startGame()
    {
        timerIsRunning = true;
        FindObjectOfType<AudioManager>().Play("timer");
    }

    public void arview()
    {
        SceneManager.LoadScene("ARview");
    }
}
