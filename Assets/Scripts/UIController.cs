using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIController : MonoBehaviour
{
    public GameObject cameraPanel;
    public GameObject videoPreviewPanel;
    public GameObject imagePreviewPanel;
    public VideoPlayer previewVideoPlayer;
    public Image previewImagePlayer;
    public RawImage imagePreview;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void CloseVideoPreview() {
        previewVideoPlayer.gameObject.SetActive(false);
        videoPreviewPanel.SetActive(false);
        cameraPanel.SetActive(true);
    }
    public void CloseImagePreview()
    {
        imagePreviewPanel.gameObject.SetActive(false);
        cameraPanel.SetActive(true);
    }
    //write sharing code here
    public void Share() {
        Debug.Log("Pass Video To Firebase Manager Here");
    }
    public void ShowImagePreview(string path) {
        StartCoroutine(path);
    }

    [System.Obsolete]
    IEnumerator LoadingImages(string path)
    {
        //yield return new WaitForSeconds(1f);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                Texture2D nexTexture = new Texture2D(texture.width, texture.height);
                nexTexture.LoadImage(uwr.downloadHandler.data);
                previewImagePlayer.sprite = Sprite.Create(nexTexture, new Rect(0, 0, nexTexture.width, nexTexture.height), new Vector2(0, 0));
                previewImagePlayer.color = Color.white;
            }
        }
    }
    //It will capture the image
    public void CaputureImage()
    {
        //turning off the UI so that i won't visible in image.
        cameraPanel.SetActive(false);
        StartCoroutine(RecordFrame());

    }
    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        // do something with texture
        //Show the image captured
        imagePreview.texture = texture;
        imagePreviewPanel.SetActive(true);
        videoPreviewPanel.SetActive(false);

        // cleanup
        //Object.Destroy(texture);
    }
    
}
