using System.Collections;
using DefaultCompany.Core.Singletons;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class ImageUtils : Singleton<ImageUtils>
{
    public Texture2D ScreenshotTexture;
    public IEnumerator CaptureScreenForImage(Canvas ignoredCanvas, GameObject tick, GameObject cross, RawImage[] mtexture)
    {
        ignoredCanvas.enabled = false;

        yield return new WaitForEndOfFrame();

        ScreenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ScreenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ScreenshotTexture.Apply();

        // Save the screenshot to Gallery/Photos
        if (ARTap.Count < mtexture.Length)
        {
            mtexture[ARTap.Count].texture = ScreenshotTexture;
        }
        ignoredCanvas.enabled = true;
        tick.SetActive(true);
        cross.SetActive(true);

    }

    public void saveImage(string defaultGallery, string defaultScreenshotFileName)
    {
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ScreenshotTexture, defaultGallery, defaultScreenshotFileName, (success, path) => Debug.Log("Media save result: " + success + " " + path));

    }

    public IEnumerator ShareCollageScreenForImage(GameObject ignoredCanvas, string defaultScreenshotFileName)
    { 
        ignoredCanvas.SetActive(false);

        yield return new WaitForEndOfFrame();

        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, defaultScreenshotFileName);
        File.WriteAllBytes(filePath, screenshotTexture.EncodeToPNG());
        new NativeShare().AddFile(filePath).Share();
        ignoredCanvas.SetActive(true);
    }
}
