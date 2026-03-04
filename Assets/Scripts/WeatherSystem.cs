using UnityEngine;
using System.Collections;

public enum WeatherType
{
    Sunny,    //  Nắng - cần tưới 2 lần/ngày
    Rainy,    // Mưa - tự động tưới
    Cloudy    //  Mây - tưới bình thường
}

public class WeatherSystem : MonoBehaviour
{
    public static WeatherSystem instance;

    [Header("Weather Settings")]
    public WeatherType currentWeather = WeatherType.Cloudy;

    [Header("Visual Effects")]
    public GameObject rainEffect;          // Particle effect cho mưa
    public GameObject sunEffect;           // Sprite/effect cho nắng
    public SpriteRenderer weatherIcon;     // Icon hiển thị thời tiết

    [Header("Weather Sprites (Optional)")]
    public Sprite sunnySprite;
    public Sprite rainySprite;
    public Sprite cloudySprite;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Bắt đầu với thời tiết mặc định (người chơi sẽ chọn)
        UpdateWeatherVisuals();
    }

    void Update()
    {
        // Test controls - keyboard shortcuts
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeather(WeatherType.Rainy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeather(WeatherType.Sunny);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeather(WeatherType.Cloudy);
        }
    }


    public void UpdateWeatherVisuals()
    {
        // Tắt tất cả effects
        if (rainEffect != null)
        {
            ParticleSystem rainPS = rainEffect.GetComponent<ParticleSystem>();
            if (rainPS != null)
            {
                rainPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            rainEffect.SetActive(false);
        }
        
        // SUN EFFECT removed - chỉ hiển thị text thôi
        if (sunEffect != null)
            sunEffect.SetActive(false);

        // Bật effect tương ứng
        switch (currentWeather)
        {
            case WeatherType.Rainy:
                if (rainEffect != null)
                {
                    rainEffect.SetActive(true);
                    ParticleSystem rainPS = rainEffect.GetComponent<ParticleSystem>();
                    if (rainPS != null)
                    {
                        rainPS.Clear();
                        rainPS.Play(true);
                        Debug.Log("Rain effect activated");
                    }
                }
                if (weatherIcon != null && rainySprite != null)
                    weatherIcon.sprite = rainySprite;
                break;

            case WeatherType.Sunny:
                // Chỉ hiển thị text "☀️ Nắng gắt", không có visual effect
                if (weatherIcon != null && sunnySprite != null)
                    weatherIcon.sprite = sunnySprite;
                break;

            case WeatherType.Cloudy:
                // Chỉ hiển thị text "☁️ Mây", không có visual effect
                if (weatherIcon != null && cloudySprite != null)
                    weatherIcon.sprite = cloudySprite;
                break;
        }

        // Update UI text
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateWeatherDisplay(GetWeatherNameVN());
        }
    }

    void AutoWaterPlants()
    {
        PlantController plant = GameManager.instance?.plant;
        if (plant != null)
        {
            // Gọi hàm tưới nước tự động (không qua button)
            plant.AutoWaterFromRain();
            
            if (UIManager.instance != null)
            {
                UIManager.instance.ShowNotification("Troi mua! Cay duoc tuoi tu dong.", 3f);
            }
        }
    }


    // Lấy tên thời tiết bằng tiếng Việt
    public string GetWeatherNameVN()
    {
        switch (currentWeather)
        {
            case WeatherType.Sunny: return "NANG GAT";
            case WeatherType.Rainy: return "TROI MUA";
            case WeatherType.Cloudy: return "TROI MAY";
            default: return "Unknown";
        }
    }

    // Force change weather (để test)
    public void SetWeather(WeatherType weather)
    {
        currentWeather = weather;
        UpdateWeatherVisuals();
    }

}
