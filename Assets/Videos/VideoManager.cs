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
        videoPlayer.Prepare();
        gameUI.SetActive(false);
        videoCanvas.gameObject.SetActive(true);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.gameObject.SetActive(false);
        videoCanvas.gameObject.SetActive(false);
        if (!vp.clip.name.Contains("end"))
            gameUI.SetActive(true);  // Game after intro, menu after end?
    }
}
