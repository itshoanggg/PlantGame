using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Action Buttons")]
    public Button waterButton;
    public Button fertilizerButton;

    [Header("Notification")]
    public TextMeshProUGUI actionNotificationText;

    [Header("Weather Display")]
    public TextMeshProUGUI weatherText;

    [Header("Button Colors")]
    public Color normalColor = Color.white;
    public Color requiredColor = Color.yellow;
    public Color disabledColor = Color.gray;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Disable buttons initially
        SetWaterButtonState(false, false);
        SetFertilizerButtonState(false, false);
        
        if (actionNotificationText != null)
            actionNotificationText.text = "";
    }

    // Water button control
    public void SetWaterButtonState(bool interactable, bool isRequired)
    {
        if (waterButton == null) return;

        waterButton.interactable = interactable;
        
        Image buttonImage = waterButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (!interactable)
                buttonImage.color = disabledColor;
            else if (isRequired)
                buttonImage.color = requiredColor;
            else
                buttonImage.color = normalColor;
        }
    }

    // Fertilizer button control
    public void SetFertilizerButtonState(bool interactable, bool isRequired)
    {
        if (fertilizerButton == null) return;

        fertilizerButton.interactable = interactable;
        
        Image buttonImage = fertilizerButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (!interactable)
                buttonImage.color = disabledColor;
            else if (isRequired)
                buttonImage.color = requiredColor;
            else
                buttonImage.color = normalColor;
        }
    }

    // Show notification message
    public void ShowNotification(string message, float duration = 3f)
    {
        if (actionNotificationText == null) return;

        actionNotificationText.text = message;
        CancelInvoke(nameof(ClearNotification));
        
        // Tự động ẩn sau duration giây (duration = 0 nghĩa là không tự động ẩn)
        if (duration > 0)
            Invoke(nameof(ClearNotification), duration);
    }

    void ClearNotification()
    {
        if (actionNotificationText != null)
            actionNotificationText.text = "";
    }
    
    // Clear notification immediately
    public void ClearNotificationNow()
    {
        CancelInvoke(nameof(ClearNotification));
        ClearNotification();
    }

    // Update weather display
    public void UpdateWeatherDisplay(string weatherName)
    {
        if (weatherText != null)
            weatherText.text = "Thời tiết: " + weatherName;
    }
}
