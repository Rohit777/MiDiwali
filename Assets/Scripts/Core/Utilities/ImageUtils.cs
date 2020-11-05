using System.Collections;
using DefaultCompany.Core.Singletons;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ImageUtils : Singleton<ImageUtils>
{
    public IEnumerator CaptureScreenForImage(Canvas ignoredCanvas, string defaultGallery, string defaultScreenshotFileName, GameObject tick, RawImage[] mtexture, Action<string, Texture2D, CaptureType> callback = null)
    {
        ignoredCanvas.enabled = false;

        yield return new WaitForEndOfFrame();

        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshotTexture.Apply();

        // Save the screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(screenshotTexture, defaultGallery, defaultScreenshotFileName, (success, path) => Debug.Log("Media save result: " + success + " " + path));
        mtexture[ARTap.Count].texture = screenshotTexture;
        ignoredCanvas.enabled = true;
        tick.SetActive(true);
    }
}
