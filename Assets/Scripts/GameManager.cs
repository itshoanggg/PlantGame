using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int day = 1;

    public float dayDuration = 5f;
    public float nightDuration = 5f;

    float timer;
    bool isDay = true;

    public PlantController plant;

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI notificationText;

    public DayNightFade dayNightFade;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateDayText();
        dayNightFade.FadeToDay();

        // Play game music
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGameMusic();
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
            return;
        }

        timer += Time.deltaTime;

        if (isDay && timer >= dayDuration)
        {
            StartNight();
        }
        else if (!isDay && timer >= nightDuration)
        {
            StartNewDay();
        }
    }

    void StartNight()
    {
        isDay = false;
        timer = 0;

        if (WeatherSystem.instance != null)
            WeatherSystem.instance.UpdateWeatherVisuals();

        dayNightFade.FadeToNight();
    }

    void StartNewDay()
    {
        isDay = true;
        timer = 0;

        day++;                 // +1 khi KẾT THÚC đêm
        UpdateDayText();

        if (WeatherSystem.instance != null)
            WeatherSystem.instance.UpdateWeatherVisuals();

        if (plant != null)
            plant.DayPassed();

        dayNightFade.FadeToDay();
    }

    void UpdateDayText()
    {
        dayText.text = "Day: " + day;
    }

    public void GameOver(string reason)
    {
        Time.timeScale = 0f;

        if (UIManager.instance != null && UIManager.instance.actionNotificationText != null)
        {
            UIManager.instance.actionNotificationText.text =
                "GAME OVER!\n" + reason + "\n\nPress R to Restart";
            UIManager.instance.actionNotificationText.gameObject.SetActive(true);
        }
        else if (notificationText != null)
        {
            notificationText.text =
                "GAME OVER!\n" + reason + "\n\nPress R to Restart";
            notificationText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("No notification text available for Game Over!");
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        
        // Clear notification before restart
        if (UIManager.instance != null && UIManager.instance.actionNotificationText != null)
        {
            UIManager.instance.actionNotificationText.text = "";
        }
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    // Skip Time button - immediately go to next phase
    public void SkipTime()
    {
        if (Time.timeScale == 0f) return; // Don't skip if game is over

        if (isDay)
        {
            StartNight();
        }
        else
        {
            StartNewDay();
        }
    }
}