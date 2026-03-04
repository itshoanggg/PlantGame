using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public Button playButton;
    public string gameSceneName = "SampleScene";

    void Start()
    {
        // Setup button listener
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("Play Button is not assigned in MenuManager!");
        }

        // Play menu music
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMenuMusic();
        }
    }

    void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked! Loading game scene...");
        
        // Play button click sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonClick();
        }
        
        LoadGameScene();
    }

    void LoadGameScene()
    {
        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }


}
