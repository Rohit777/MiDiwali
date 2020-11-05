using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiSceneManager : MonoBehaviour
{
    public void starGame()
    {
        SceneManager.LoadScene("puzzle");
    }
}
