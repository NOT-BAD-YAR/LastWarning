using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] VideoManager videoManager;  // DRAG VIDEO MANAGERS HERE

    private int coinCount = 0;
    private int gemCount = 0;
    private bool isGameOver = false;
    private bool levelCompleted = false;  // Prevent double call
    private Vector3 playerPosition;

    //Level Complete
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] TMP_Text leveCompletePanelTitle;
    [SerializeField] TMP_Text levelCompleteCoins;

    private int totalCoins = 0;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        UpdateGUI();
        if (UIManager.instance != null)
        {
            UIManager.instance.fadeFromBlack = true;
        }
        playerPosition = playerController.transform.position;
        FindTotalPickups();
    }

    public void IncrementCoinCount()
    {
        coinCount++;
        UpdateGUI();
    }

    public void IncrementGemCount()
    {
        gemCount++;
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        coinText.text = coinCount.ToString();
    }

    // Called when player reaches the level exit
    public void HandleExitReached()
    {
        // If player collected all coins, complete level and play end video
        if (coinCount >= totalCoins && totalCoins > 0)
        {
            if (!levelCompleted)
            {
                levelCompleted = true;
                LevelComplete();
            }
        }
        else
        {
            // Not all coins collected: reload level scene (build index 1)
            SceneManager.LoadScene(1);
        }
    }

    public void Death()
    {
        if (!isGameOver)
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.DisableMobileControls();
                UIManager.instance.fadeToBlack = true;
            }
            playerController.gameObject.SetActive(false);
            StartCoroutine(DeathCoroutine());
            isGameOver = true;
            Debug.Log("Died");
        }
    }

    public void FindTotalPickups()
    {
        pickup[] pickups = Object.FindObjectsByType<pickup>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        totalCoins = 0;

        foreach (pickup pickupObject in pickups)
        {
            if (pickupObject.pt == pickup.pickupType.coin)
            {
                totalCoins += 1;
            }
        }
        Debug.Log($"Total coins: {totalCoins}");
    }

    public void LevelComplete()
    {
        Debug.Log("LEVEL COMPLETE CALLED!");
        levelCompletePanel.SetActive(true);
        leveCompletePanelTitle.text = "LEVEL COMPLETE";
        levelCompleteCoins.text = $"COINS: {coinCount} / {totalCoins}";
        StartCoroutine(CompleteWithEndVideo());
    }

    private IEnumerator CompleteWithEndVideo()
    {
        yield return new WaitForSeconds(2f);
        levelCompletePanel.SetActive(false);
        Debug.Log("Starting end video...");

        if (videoManager != null)
        {
            videoManager.PlayEndVideo();
            Debug.Log("End video triggered!");
        }
        else
        {
            Debug.LogError("Drag VideoManager to GameManager VIDEO MANAGER field!");
        }
    }

    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1f);
        if (playerController != null && isGameOver)
        {
            playerController.gameObject.SetActive(true);
            playerController.transform.position = playerPosition;
        }
        yield return new WaitForSeconds(1f);
        if (isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
