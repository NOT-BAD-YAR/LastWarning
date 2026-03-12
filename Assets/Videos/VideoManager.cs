using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class VideoManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject gameUI;
    [SerializeField] Canvas videoCanvas;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
        PlayIntroVideo();
        gameUI.SetActive(false);
        videoCanvas.gameObject.SetActive(true);
    }

    void PlayIntroVideo()
    {
        videoPlayer.clip = Resources.Load<VideoClip>("intro") as VideoClip;
        videoPlayer.Prepare();
    }

    public void PlayEndVideo()
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.clip = Resources.Load<VideoClip>("end") as VideoClip;
        if (videoPlayer.clip == null)
        {
            Debug.LogError("VideoManager: 'end' video not found in Resources folder. Add it to a Resources folder.");
            return;
        }
        gameUI.SetActive(false);
        videoCanvas.gameObject.SetActive(true);
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnEndVideoPrepared;
    }

    void OnEndVideoPrepared(VideoPlayer source)
    {
        videoPlayer.prepareCompleted -= OnEndVideoPrepared;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        videoCanvas.gameObject.SetActive(false);
        if (!vp.clip.name.Contains("end"))
            gameUI.SetActive(true);  // Game after intro, menu after end?
    }
}
