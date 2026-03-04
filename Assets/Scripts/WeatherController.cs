using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller cho 3 buttons điều khiển thời tiết
/// Người chơi chọn thời tiết thay vì random
/// </summary>
public class WeatherController : MonoBehaviour
{
    public static WeatherController instance;

    [Header("UI Buttons")]
    public Button sunnyButton;
    public Button rainyButton;
    public Button cloudyButton;

    [Header("Button Colors")]
    public Color normalColor = new Color(1f, 1f, 1f, 0.5f);
    public Color selectedColor = new Color(1f, 1f, 0f, 1f);

    private WeatherType selectedWeather = WeatherType.Cloudy;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Setup button listeners
        if (sunnyButton != null)
            sunnyButton.onClick.AddListener(() => SelectWeather(WeatherType.Sunny));
        
        if (rainyButton != null)
            rainyButton.onClick.AddListener(() => SelectWeather(WeatherType.Rainy));
        
        if (cloudyButton != null)
            cloudyButton.onClick.AddListener(() => SelectWeather(WeatherType.Cloudy));

        // Default to Cloudy
        SelectWeather(WeatherType.Cloudy);
    }

    public void SelectWeather(WeatherType weather)
    {
        selectedWeather = weather;

        // Play button click sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonClick();
        }

        // Update WeatherSystem
        if (WeatherSystem.instance != null)
        {
            WeatherSystem.instance.SetWeather(weather);
        }

        // Update button visuals
        UpdateButtonVisuals();

        Debug.Log($"Player selected weather: {weather}");
    }

    void UpdateButtonVisuals()
    {
        // Reset all buttons
        if (sunnyButton != null)
            sunnyButton.GetComponent<Image>().color = normalColor;
        if (rainyButton != null)
            rainyButton.GetComponent<Image>().color = normalColor;
        if (cloudyButton != null)
            cloudyButton.GetComponent<Image>().color = normalColor;

        // Highlight selected
        switch (selectedWeather)
        {
            case WeatherType.Sunny:
                if (sunnyButton != null)
                    sunnyButton.GetComponent<Image>().color = selectedColor;
                break;
            case WeatherType.Rainy:
                if (rainyButton != null)
                    rainyButton.GetComponent<Image>().color = selectedColor;
                break;
            case WeatherType.Cloudy:
                if (cloudyButton != null)
                    cloudyButton.GetComponent<Image>().color = selectedColor;
                break;
        }
    }

    public WeatherType GetCurrentWeather()
    {
        return selectedWeather;
    }
}
