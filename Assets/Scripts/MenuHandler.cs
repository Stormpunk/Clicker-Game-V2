using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuHandler: MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    //audio volume variables
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    //resolution variables
    bool isOnAnyKey;
    //for the any key screen, will make it so that the menu is not made active after that screen
    public GameObject AnyKeyScreen;
    public GameObject MenuScreen;
    public GameObject loadingScreen;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        isOnAnyKey = true;
        //ensures that the transition from the any key panel to game will happen
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) 
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnAnyKey && Input.anyKeyDown)
        {
            AnyKeyPressed();
            //triggers the transition when keys are pressed
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);
        //changes the volume to the values of the volume slider
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); 
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        if (!isFullscreen)
        {
            Debug.Log("IS WINDOWED");
        }
        else if(isFullscreen == true)
        {
            Debug.Log("IS FULLSCREEN");                                                                                  
        }
    }
    public void AudioMute()
    {
        audioSource.mute = !audioSource.mute;
        //mutes the audio when pressed
    }
    public void AnyKeyPressed()
    {
        MenuScreen.SetActive(true);
        AnyKeyScreen.SetActive(false);
        isOnAnyKey = false;
        //enables the main menu and stops the any key function from triggering again
    }
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //on testing it seems the load screen passes too quickly for the game to register it... hmm
            slider.value = progress;
            Debug.Log(operation.progress);
            yield return null;
        }
    }
}
