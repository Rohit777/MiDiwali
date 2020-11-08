using System.Collections;
using DefaultCompany.Core.Singletons;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class ImageUtils : Singleton<ImageUtils>
{
    public IEnumerator CaptureScreenForImage(Canvas ignoredCanvas, string defaultGallery, string defaultScreenshotFileName, GameObject tick, RawImage[] mtexture)
    {
        ignoredCanvas.enabled = false;

        yield return new WaitForEndOfFrame();

        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(screenshotTexture, defaultGallery, defaultScreenshotFileName, (success, path) => Debug.Log("Media save result: " + success + " " + path));
        if (ARTap.Count < mtexture.Length)
        {
            mtexture[ARTap.Count].texture = screenshotTexture;
        }
        ignoredCanvas.enabled = true;
        tick.SetActive(true);
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
