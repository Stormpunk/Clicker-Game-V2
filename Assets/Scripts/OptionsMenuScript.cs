using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionsMenuScript : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject gLogicManager;
    public GameObject timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(optionsMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        //pauses the game if the options menu is open
        if(Time.timeScale == 0)
        {
            timeText.SetActive(true);
        }
        //gives an indicator the game is paused
        else
        {
            timeText.SetActive(false);
        }
    }
    public void SaveTGame()
    {
        PlayerPrefs.SetFloat("Currency", gLogicManager.GetComponent<GameLogicManager>().currency);
        PlayerPrefs.SetFloat("Score", gLogicManager.GetComponent<GameLogicManager>().score);
        PlayerPrefs.SetFloat("Click", gLogicManager.GetComponent<GameLogicManager>().scoreClickValue);
        PlayerPrefs.Save();
        //will save these variables when clicked
    }
    public void LoadTGame()
    {
        if (PlayerPrefs.HasKey("Currency"))
        {
            gLogicManager.GetComponent<GameLogicManager>().currency = PlayerPrefs.GetFloat("Currency");
            gLogicManager.GetComponent<GameLogicManager>().score = PlayerPrefs.GetFloat("Score");
            gLogicManager.GetComponent<GameLogicManager>().scoreClickValue = PlayerPrefs.GetFloat("Click");
            //will load these variables when clicked
        }
    }
    public void Exit()
    {
        Application.Quit();
        //exits the game
    }
}
