using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class VideoManager : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RawImage videoImage;
    [SerializeField] Button skipButton;
    [SerializeField] GameObject gameUI;
    [SerializeField] Canvas videoCanvas;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;  // NEW: Auto-skip
        videoPlayer.clip = Resources.Load<VideoClip>("intro") as VideoClip;
        videoPlayer.Prepare();
        skipButton.onClick.AddListener(SkipVideo);
        gameUI.SetActive(false);
        videoCanvas.gameObject.SetActive(true);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SkipVideo();  // Auto-call skip when ends
    }

    public void SkipVideo()
    {
        videoPlayer.Stop();
        videoPlayer.loopPointReached -= OnVideoEnd;  // Clean up
        skipButton.gameObject.SetActive(false);
        videoCanvas.gameObject.SetActive(false);
        gameUI.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SkipVideo();
    }
}
