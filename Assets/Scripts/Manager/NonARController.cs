using System.Collections;
using System.Collections.Generic;
using DefaultCompany.Core.Singletons;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NonARController : Singleton<UIManager>
{
    [SerializeField]
    private GameObject Button;
    public GameObject CollageBtn;
    [SerializeField]
    private string defaultScreenshotFileName = "Screenshot.png";

    private string selectedBtn;

    //public string[] ButtonName = new string[] { "Mi_CameraBtn", "Mi_WaterPurifierBtn", "MI_AirPurifierBtn", "Mi_BulbBtn", "Mi_LampBtn", "Mi_SmartLampBtn" };

    public List<string> ButtonName = new List<string> { "Mi_CameraBtn", "Mi_WaterPurifierBtn", "MI_AirPurifierBtn", "Mi_BulbBtn", "Mi_LampBtn", "Mi_SmartLampBtn" };

    int buttonCount = 0;

    public VideoPlayer vid;
    public VideoClip[] myclip;
    private VideoClip selectedClip;

    public void ShareCollage() => StartCoroutine(ImageUtils.Instance.ShareCollageScreenForImage(Button, defaultScreenshotFileName));

    private void Update()
    {
        if(buttonCount == 3)
        {
            CollageBtn.SetActive(true);
        }
    }
    public void OnClicked(Button button)
    {
        switch (button.name)
        {
            case "Mi_CameraBtn":
                cheackButtonPressed("Mi_CameraBtn");
                selectedClip = myclip[0];
                break;
            case "Mi_WaterPurifierBtn":
                cheackButtonPressed("Mi_WaterPurifierBtn");
                selectedClip = myclip[1];
                break;
            case "MI_AirPurifierBtn":
                cheackButtonPressed("MI_AirPurifierBtn");
                selectedClip = myclip[2];
                break;
            case "Mi_BulbBtn":
                cheackButtonPressed("Mi_BulbBtn");
                selectedClip = myclip[3];
                break;
            case "Mi_LampBtn":
                cheackButtonPressed("Mi_LampBtn");
                selectedClip = myclip[4];
                break;
            case "Mi_SmartLampBtn":
                cheackButtonPressed("Mi_SmartLampBtn");
                selectedClip = myclip[5];
                break;
        }
        vid.clip = selectedClip;
    }

    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        renderTexture.Release();
    }

    private void cheackButtonPressed( string stringToCheck)
    {
        for(int i = 0; i<ButtonName.Count; i++)
        {
            if(ButtonName[i] == stringToCheck)
            {
                buttonCount++;
                ButtonName.RemoveAt(i);
            }
        }
    }

}
