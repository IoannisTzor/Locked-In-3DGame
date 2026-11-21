using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// The door keypad. Pressing E opens an on-screen keypad (click the buttons).
// It stays dead until PowerOn() is called by the fuse box.
// Esc closes it. The correct code fires onCorrectCode (open door, win game).
public class Keypad : Interactable
{
    [Header("Code")]
    [SerializeField] private string correctCode = "1847";
    [SerializeField] private int maxDigits = 4;

    [Header("Power")]
    [SerializeField] private bool startsPowered = false;
    [SerializeField] private string noPowerMessage = "The keypad has no power.";

    [Header("UI")]
    [SerializeField] private GameObject keypadPanel;
    [SerializeField] private TMP_Text display;

    [Header("Audio (optional)")]
    [SerializeField] private AudioSource beepSound;
    [SerializeField] private AudioSource errorSound;
    [SerializeField] private AudioSource successSound;

    [Header("Events")]
    public UnityEvent onCorrectCode;

    private PlayerInteractor player;
    private string entered = "";
    private bool powered;
    private bool solved;
    private bool isOpen;
    private int openFrame;

    public override string PromptText => solved ? "" : base.PromptText;

    private void Awake()
    {
        powered = startsPowered;
        keypadPanel.SetActive(false);
    }

    // Call this from the fuse box's On Unlocked event.
    public void PowerOn()
    {
        powered = true;
    }

    public override void Interact(PlayerInteractor interactor)
    {
        if (solved) return;

        if (!powered)
        {
            MessageDisplay.Show(noPowerMessage);
            return;
        }

        player = interactor;
        isOpen = true;
        openFrame = Time.frameCount;
        entered = "";
        UpdateDisplay();

        keypadPanel.SetActive(true);
        player.SetBusy(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!isOpen || Time.frameCount == openFrame) return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Close();
        }
    }

    // Hook each number button's OnClick to this, with its number as the parameter.
    public void PressDigit(int digit)
    {
        if (!isOpen || entered.Length >= maxDigits) return;

        entered += digit.ToString();
        if (beepSound != null) beepSound.Play();
        UpdateDisplay();
    }

    public void PressClear()
    {
        if (!isOpen) return;

        entered = "";
        if (beepSound != null) beepSound.Play();
        UpdateDisplay();
    }

    public void PressEnter()
    {
        if (!isOpen) return;

        if (entered == correctCode)
        {
            solved = true;
            if (successSound != null) successSound.Play();
            Close();
            onCorrectCode?.Invoke();
        }
        else
        {
            entered = "";
            display.text = "ERROR";
            if (errorSound != null) errorSound.Play();
        }
    }

    private void Close()
    {
        isOpen = false;
        keypadPanel.SetActive(false);
        player.SetBusy(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdateDisplay()
    {
        // Show entered digits, with dashes for the remaining ones (e.g. "18--").
        display.text = entered.PadRight(maxDigits, '-');
    }
}
