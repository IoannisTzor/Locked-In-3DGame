using TMPro;
using UnityEngine;

// Shows short feedback messages ("It's locked.", "Picked up Brass Key") for a few seconds.
// Put this on the Canvas. Any script can call MessageDisplay.Show("...").
public class MessageDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private float duration = 2.5f;

    private static MessageDisplay instance;
    private float hideTime;

    private void Awake()
    {
        instance = this;
        label.gameObject.SetActive(false);
    }

    public static void Show(string text)
    {
        if (instance == null || string.IsNullOrEmpty(text)) return;

        instance.label.text = text;
        instance.label.gameObject.SetActive(true);
        instance.hideTime = Time.time + instance.duration;
    }

    private void Update()
    {
        if (label.gameObject.activeSelf && Time.time >= hideTime)
        {
            label.gameObject.SetActive(false);
        }
    }
}
