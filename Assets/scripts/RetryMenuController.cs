using UnityEngine;
using TMPro; // Required for TextMeshPro
using UnityEngine.SceneManagement; // Required for reloading the scene

public class RetryMenuController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The root GameObject of your Retry Menu Canvas/Panel.")]
    [SerializeField] private GameObject retryUIPanel;

    [Tooltip("The TextMeshPro UI element that will show the final time.")]
    [SerializeField] private TextMeshProUGUI timeDisplayText;

    private float startTime;
    private bool isTimerRunning = false;

    private void Start()
    {
        // Ensure the retry UI is hidden when the game starts
        if (retryUIPanel != null)
        {
            retryUIPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Call this from your Start UI Button's Unity Event.
    /// </summary>
    public void StartTimer()
    {
        startTime = Time.time;
        isTimerRunning = true;
    }

    /// <summary>
    /// Call this when the final fire is extinguished.
    /// </summary>
    public void ShowRetryMenu()
    {
        if (!isTimerRunning) return;
        isTimerRunning = false;

        // Calculate total time taken
        float timeTaken = Time.time - startTime;

        // Format into Minutes and Seconds
        int minutes = Mathf.FloorToInt(timeTaken / 60F);
        int seconds = Mathf.FloorToInt(timeTaken - minutes * 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Update the text UI
        if (timeDisplayText != null)
        {
            timeDisplayText.text = "TIME TAKEN TO COMPLETE THE SIMULATION IS:\n" + timeString;
        }

        // Show the panel
        if (retryUIPanel != null)
        {
            retryUIPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Call this from the Proceed button on your Retry UI.
    /// </summary>
    public void RestartScene()
    {
        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}