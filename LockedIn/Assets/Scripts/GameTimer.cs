using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Counts down from the start time and shows it as mm:ss.
// Turns red and speeds up the clock ticking near the end, then fires onTimeUp at zero.
public class GameTimer : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float startSeconds = 900f; // 15 minutes

    [Header("UI")]
    [SerializeField] private TMP_Text label;

    [Header("Warning")]
    [SerializeField] private float warningAt = 60f;
    [SerializeField] private Color warningColor = Color.red;
    [SerializeField] private string warningMessage = "Hurry, time is running out!";
    [SerializeField] private AudioSource tickSound;
    [SerializeField] private float warningTickPitch = 1.5f;

    [Header("Events")]
    public UnityEvent onWarning;
    public UnityEvent onTimeUp;

    public float TimeRemaining { get; private set; }

    private bool running = true;
    private bool warned;

    // Time as "mm:ss", used by the timer label and the win screen.
    public string Formatted
    {
        get
        {
            int total = Mathf.CeilToInt(TimeRemaining);
            return $"{total / 60:00}:{total % 60:00}";
        }
    }

    private void Awake()
    {
        TimeRemaining = startSeconds;
        UpdateLabel();
    }

    private void Update()
    {
        if (!running) return;

        TimeRemaining -= Time.deltaTime;

        if (!warned && TimeRemaining <= warningAt)
        {
            warned = true;
            if (label != null) label.color = warningColor;
            if (tickSound != null) tickSound.pitch = warningTickPitch;
            MessageDisplay.Show(warningMessage);
            onWarning?.Invoke();
        }

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            running = false;
            UpdateLabel();
            onTimeUp?.Invoke();
            return;
        }

        UpdateLabel();
    }

    public void Stop()
    {
        running = false;
    }

    private void UpdateLabel()
    {
        if (label != null) label.text = Formatted;
    }
}
