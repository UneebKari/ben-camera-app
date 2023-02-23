/* 
*   NatCorder
*   Copyright (c) 2021 Yusuf Olokoba
*/

namespace NatSuite.Examples {
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using System.Collections;
    using UnityEngine;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;
    using UnityEngine.Video;
    using UnityEngine.UI;
    public class ReplayCam : MonoBehaviour {

        [Header(@"Recording")]
        public int videoWidth = 720;
        public int videoHeight = 1280;

        [Header("Microphone")]
        public bool recordMicrophone;
        private AudioSource microphoneSource;

        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;
        private AudioInput audioInput;
        public UIController uIManager;


        //        public void StartRecording()
        //        {
        //            // Start recording
        //            recordingClock = new RealtimeClock();
        //            videoRecorder = new MP4Recorder(
        //                videoWidth,
        //                videoHeight,
        //                30,
        //                recordMicrophone ? AudioSettings.outputSampleRate : 0,
        //                recordMicrophone ? (int)AudioSettings.speakerMode : 0,
        //                OnReplay
        //            );
        //            // Create recording inputs
        //            cameraInput = new CameraInput(videoRecorder, recordingClock, Camera.main);
        //            if (recordMicrophone)
        //            {
        //                StartMicrophone();
        //                audioInput = new AudioInput(videoRecorder, recordingClock, microphoneSource, true);
        //            }

        //        }
        //        private void StartMicrophone()
        //        {
        //#if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
        //            // Create a microphone clip
        //            microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
        //            while (Microphone.GetPosition(null) <= 0) ;
        //            // Play through audio source
        //            microphoneSource.timeSamples = Microphone.GetPosition(null);
        //            microphoneSource.loop = true;
        //            microphoneSource.Play();
        //#endif
        //        }
        //        public void StopRecording()
        //        {
        //            // Stop the recording inputs
        //            if (recordMicrophone)
        //            {
        //                StopMicrophone();
        //                audioInput.Dispose();
        //            }
        //            cameraInput.Dispose();
        //            // Stop recording
        //            videoRecorder.Dispose();
        //        }
        //        private void StopMicrophone()
        //        {
        //#if !UNITY_WEBGL || UNITY_EDITOR
        //            Microphone.End(null);
        //            microphoneSource.Stop();
        //#endif
        //        }

        //        private void OnReplay(string path)
        //        {
        //            Debug.Log("Saved recording to: " + path);

        //            // Playback the video
        //#if UNITY_EDITOR
        //            EditorUtility.OpenWithDefaultApp(path);
        //            uic.ShowVideoPreview(path);
        //#elif UNITY_IOS
        //           // Handheld.PlayFullScreenMovie("file://" + path);
        //            ShareMediaManager.filePath = path;
        //            uic.ShowVideoPreview(path);
        //#elif UNITY_ANDROID
        //           // Handheld.PlayFullScreenMovie(path);
        //            ShareMediaManager.filePath = path;
        //            uic.ShowVideoPreview(path);
        //#endif
        //        }

        private MP4Recorder recorder;
        //private CameraInput cameraInput;
        //private AudioInput audioInput;
        public VideoPlayer vPlayer;
        //public UIController uIManager;
        Color32[] pixelArray;

        private IEnumerator Start()
        {
            // Start microphone
            microphoneSource = gameObject.GetComponent<AudioSource>();
            microphoneSource.mute = false;
            microphoneSource.loop = true;
            microphoneSource.bypassEffects =
            microphoneSource.bypassListenerEffects = false;
            microphoneSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
            yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
            microphoneSource.Play();
        }

        private void OnDestroy()
        {
            // Stop microphone
            if (microphoneSource != null)
            {
                microphoneSource.Stop();
                Microphone.End(null);
            }
        }
        void Update()
        {
            //// GOOD // Copy into the same array first
            //cameraInput..GetPixels32(pixelArray);
            //// Commit that array
            //recorder.CommitFrame(pixelArray);
        }
        public void StartRecording()
        {
            // Start recording
            var frameRate = 30;
            var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
            var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
            var clock = new RealtimeClock();
            recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount, audioBitRate: 96_000);
            // Create recording inputs
            cameraInput = new CameraInput(recorder, clock, Camera.main);
            audioInput = recordMicrophone ? new AudioInput(recorder, clock, microphoneSource, true) : null;
            // Unmute microphone
            microphoneSource.mute = audioInput == null;

        }

        public async void StopRecording()
        {
            // Mute microphone
            microphoneSource.mute = true;
            // Stop recording
            audioInput?.Dispose();
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            // Playback recording via unity player
            Debug.Log($"Saved recording to: {path}");

            //string imgName = "VID_" + System.DateTime.Now.ToString("yyyymmdd_HHmmss") + ".mp4";
            //NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(path, "TraceVideo", imgName, null);
            //Debug.Log("Permission result: " + permission);
            uIManager.cameraPanel.SetActive(false);
            uIManager.videoPreviewPanel.SetActive(true);
            vPlayer.gameObject.SetActive(true);
            vPlayer.url = path;
            vPlayer.Play();


            //NativeGallery.SaveVideoToGallery(path,"Trace",)
            //Handheld.PlayFullScreenMovie($"file://{path}");





        }
    }
}