//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.UI;



//public class ScreenshotToGallery : MonoBehaviour
//{
//    public GameObject MainCanvas;
//    public UIController uic;
//    public void SaveScreenshot(string title)
//    {
//        SaveScreenshotToGallery(System.DateTime.Today.ToString(), title);
//    }
//    public void TakeScreenshot()
//    {
//        // Take a screenshot and save it to Gallery/Photos
//        MainCanvas.SetActive(false);
//        StartCoroutine(TakeScreenshotAndSave());
//    }
//    public void SaveScreenshotToGallery(string title, string description)
//    {
//        StartCoroutine(TakeScreenshotAndSave(title, description));
//    }

//    private IEnumerator TakeScreenshotAndSave(string title, string description)
//    {
//        yield return new WaitForEndOfFrame();

//        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
//        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
//        ss.Apply();

//        // Save the screenshot to Gallery/Photos
//        // Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, title, description));
//        if (NativeGallery.SaveImageToGallery(ss, title, description) == NativeGallery.Permission.Granted)
//        {
//#if UNITY_ANDROID
//            _ShowAndroidToastMessage("Image Saved");
//#endif
//        }
//        // _ShowAndroidToastMessage("Permission result: " + NativeGallery.SaveImageToGallery(ss, title, description));
//        // To avoid memory leaks
//        Destroy(ss);
//    }
    
//    private IEnumerator TakeScreenshotAndSave()
//    {
//        yield return new WaitForEndOfFrame();

//        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
//        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
//        ss.Apply();

//        // Save the screenshot to Gallery/Photos
//        string imgName = "IMG_" + System.DateTime.Now.ToString("yyyymmdd_HHmmss") + ".png";
//        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "PlonkImages", imgName);
//        Debug.Log("Permission result: " + permission);

//        // Save the screenshot to the temporary filder
//        string tempFilePath = System.IO.Path.Combine(Application.temporaryCachePath, imgName);
//        byte[] bytes = ss.EncodeToPNG();
//        System.IO.File.WriteAllBytes(tempFilePath, bytes);

//        //_ShowAndroidToastMessage(""+ NativeGallery.mediaPath);
//        //MainCanvas.SetActive(true);

//        //StartCoroutine(LoadingImages("http://qnimate.com/wp-content/uploads/2014/03/images2.jpg"));

//        // To avoid memory leaks
//        Destroy(ss);


//        if (permission == NativeGallery.Permission.Granted)
//        {
//#if UNITY_ANDROID
//            //_ShowAndroidToastMessage("file://" + Application.persistentDataPath + "/");
//            _ShowAndroidToastMessage("Saved");
//            uic.ShowImagePreview("file://" + Application.persistentDataPath + "/" + imgName);
//#elif UNITY_IOS
//            //uneebkari
//            //uic.imagePreviewPanel.SetActive(true);
//            //uic.ShowImagePreview(tempFilePath);
//#else
//            uic.ShowImagePreview("file://" + Application.persistentDataPath + "/" + imgName);
//#endif

//            // ShareMediaManager.filePath = "file://" + Application.persistentDataPath + "/" + imgName;
//#if UNITY_IOS
//            //ShareMediaManager.filePath = "file://" + Application.persistentDataPath + "/" + imgName;
//            //uneebkari
//            //ShareMediaManager.filePath = tempFilePath;
//#elif UNITY_ANDROID
//            ShareMediaManager.filePath = Application.persistentDataPath + "/" + imgName;
//#endif
//        }
//    }



//    public void PickImage(int maxSize)
//    {
//        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
//        {
//            Debug.Log("Image path: " + path);
//            if (path != null)
//            {
//                // Create Texture from selected image
//                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
//                if (texture == null)
//                {
//                    Debug.Log("Couldn't load texture from " + path);
//                    return;
//                }

//                // Assign texture to a temporary quad and destroy it after 5 seconds
//                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
//                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
//                quad.transform.forward = Camera.main.transform.forward;
//                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

//                Material material = quad.GetComponent<Renderer>().material;
//                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
//                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

//                material.mainTexture = texture;

//                Destroy(quad, 5f);

//                // If a procedural texture is not destroyed manually, 
//                // it will only be freed after a scene change
//                Destroy(texture, 5f);
//            }
//        }, "Select a PNG image", "image/png");

//        Debug.Log("Permission result: " + permission);
//    }

//    public void PickVideo()
//    {
//        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((path) =>
//        {
//            Debug.Log("Video path: " + path);
//            if (path != null)
//            {
//                // Play the selected video
//                Handheld.PlayFullScreenMovie("file://" + path);
//            }
//        }, "Select a video");

//        Debug.Log("Permission result: " + permission);
//    }
//    private void _ShowAndroidToastMessage(string message)
//    {
//        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

//        if (unityActivity != null)
//        {
//            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
//            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
//            {
//                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
//                    message, 0);
//                toastObject.Call("show");
//            }));
//        }
//    }
//}
