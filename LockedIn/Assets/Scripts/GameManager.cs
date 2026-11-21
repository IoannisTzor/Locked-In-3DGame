using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Handles the end of the game: winning (correct keypad code) and losing (timer hits zero).
// Shows the right screen and lets the player retry or quit.
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameTimer timer;
    [SerializeField] private PlayerInteractor player;

    [Header("End Screens")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text winTimeText;
    [Tooltip("Seconds to wait after the door opens before showing the win screen.")]
    [SerializeField] private float winDelay = 2f;

    [Header("Events")]
    public UnityEvent onWin;
    public UnityEvent onLose;

    private bool gameOver;

    private void Awake()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    // Hook to the keypad's On Correct Code event.
    public void Win()
    {
        if (gameOver) return;
        gameOver = true;

        timer.Stop();
        FreezePlayer();
        onWin?.Invoke();
        StartCoroutine(ShowWinScreen());
    }

    // Hook to the timer's On Time Up event.
    public void Lose()
    {
        if (gameOver) return;
        gameOver = true;

        FreezePlayer();
        onLose?.Invoke();
        ShowCursor();
        losePanel.SetActive(true);
    }

    // Hook to the Retry / Play Again buttons.
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Hook to the Quit buttons.
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator ShowWinScreen()
    {
        yield return new WaitForSeconds(winDelay);

        if (winTimeText != null) winTimeText.text = $"Time remaining: {timer.Formatted}";
        ShowCursor();
        winPanel.SetActive(true);
    }

    private void FreezePlayer()
    {
        // SetBusy stops movement and looking, and hides the interaction prompt.
        player.SetBusy(true);
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
