/* 
*   NatCorder
*   Copyright (c) 2021 Yusuf Olokoba
*/

namespace NatSuite.Examples {

    using System.Collections;
    using UnityEngine;
    using Recorders;
    using Recorders.Clocks;
    using Recorders.Inputs;
    using UnityEngine.Video;

    public class ReplayCam : MonoBehaviour {

        [Header(@"Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        public bool recordMicrophone;

        private MP4Recorder recorder;
        private CameraInput cameraInput;
        private AudioInput audioInput;
        private AudioSource microphoneSource;
        public VideoPlayer vPlayer;
        public UIController uIManager;
        private IEnumerator Start () {
            // Start microphone
            microphoneSource = gameObject.AddComponent<AudioSource>();
            microphoneSource.mute = false;
            microphoneSource.loop = true;
            microphoneSource.bypassEffects =
            microphoneSource.bypassListenerEffects = false;
            microphoneSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
            yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
            microphoneSource.Play();
        }

        private void OnDestroy () {
            // Stop microphone
            microphoneSource.Stop();
            Microphone.End(null);
        }

        public void StartRecording () {
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

        public async void StopRecording () {
            // Mute microphone
            microphoneSource.mute = true;
            // Stop recording
            audioInput?.Dispose();
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            // Playback recording via unity player
            Debug.Log($"Saved recording to: {path}");
            uIManager.cameraPanel.SetActive(false);
            uIManager.videoPreviewPanel.SetActive(true);
            vPlayer.gameObject.SetActive(true);
            vPlayer.url = path;
            vPlayer.Play();
            //Handheld.PlayFullScreenMovie($"file://{path}");
        }
    }
}