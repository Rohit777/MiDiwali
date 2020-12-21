﻿using System.Collections;
using System.Collections.Generic;
using DefaultCompany.Core.Singletons;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NonARController : Singleton<UIManager>
{
    [SerializeField]
    private GameObject Button;

    [SerializeField]
    private string defaultScreenshotFileName = "Screenshot.png";

    private string selectedBtn;



    public VideoPlayer vid;
    public VideoClip[] myclip;
    private VideoClip selectedClip;

    public void ShareCollage() => StartCoroutine(ImageUtils.Instance.ShareCollageScreenForImage(Button, defaultScreenshotFileName));


    public void OnClicked(Button button)
    {
        switch (button.name)
        {
            case "Mi_CameraBtn":
                selectedClip = myclip[0];
                break;
            case "Mi_WaterPurifierBtn":
                selectedClip = myclip[1];
                break;
            case "MI_AirPurifierBtn":
                selectedClip = myclip[2];
                break;
            case "Mi_BulbBtn":
                selectedClip = myclip[3];
                break;
            case "Mi_LampBtn":
                selectedClip = myclip[4];
                break;
            case "Mi_SmartLampBtn":
                selectedClip = myclip[5];
                break;
        }

        vid.clip = selectedClip;
    }

    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        renderTexture.Release();
    }

}
