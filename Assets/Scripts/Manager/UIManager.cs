using System.Collections;
using System.Collections.Generic;
using DefaultCompany.Core.Singletons;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private string defaultGallery = "Screenshots";

    [SerializeField]
    private string defaultScreenshotFileName = "Screenshot.png";

    [SerializeField]
    private GameObject tick;

    [SerializeField]
    private GameObject cross;

    private CaptureType lastCapturedType;

    private Texture2D[] lastCapturedImages;

    [SerializeField]
    private RawImage[] imagePreviewTexture;

    public RawImage[] ImagePreviewTexture
    {
        get { return imagePreviewTexture; }
    }

    public void TakeScreenshot() => StartCoroutine(ImageUtils.Instance
        .CaptureScreenForImage(canvas, defaultGallery, defaultScreenshotFileName, tick, cross, imagePreviewTexture));

}
