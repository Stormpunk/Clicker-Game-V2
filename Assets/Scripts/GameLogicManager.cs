using Packages.Rider.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicManager : MonoBehaviour
{
    #region Score Variables
    public float score;
    //players score
    public float currency;
    //converted currency that player buys upgrades with
    public float scoreClickValue;
    //the total amount of points that the player will earn per click
    public float baseClickValue;
    //initial value of the click, this will usually be 1
    public float passiveClickValue;
    //Passive amount of clicks per second
    public float clickMultiplier;
    //a fraction of a click that will be added to the base click
    public int numberOfAutoclickers;
    //total number of Autoclicker upgrade
    public int numberOfClickMultipliers;
    //total number of Click multiplier upgrade
    public float[] costToUpgrade =
    {
        50, 100, 500, 1000
    };
    //how much things cost
    public float costIncrease = 1.5f;
    #endregion
    #region Game Objects
    public Text scoreText;
    public Text cmText;
    public Text ppcText;
    public Text currencyText;
    #endregion
    public
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        baseClickValue = 1;
        passiveClickValue = 0;
        numberOfAutoclickers = 0;
        numberOfClickMultipliers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score += passiveClickValue;
        scoreClickValue = baseClickValue + clickMultiplier;
        scoreText.text = score.ToString() + " Points";
        cmText.text = "Click Multipliers: " + numberOfClickMultipliers.ToString();
        clickMultiplier = numberOfClickMultipliers * 0.25f;
        ppcText.text = "Points per Click: " + scoreClickValue.ToString();
        currencyText.text = "Currency: " + currency.ToString();
    }
    public void ClickToScore()
    {
        score += scoreClickValue;
    }
    public void BuyPassiveClick()
    {
        score -= costToUpgrade[1];
        numberOfAutoclickers++;
        costToUpgrade[1] *= costIncrease;
    }
    public void BuyClickMultiplier()
    {
        if (currency >= costToUpgrade[0])
        {
            currency -= costToUpgrade[0];
            numberOfClickMultipliers++;
            costToUpgrade[0] *= costIncrease;
        }
        
    }
    public void PointsToCurrency10()
    {
        if (score >= 10)
        {
            score -= 10;
            currency += 10;
        }
    }
    public void PointsToCurrency50()
    {
        if(score >= 50)
        {
            score -= 50;
            currency += 50;
        }
    }
    public void PointsToCurrency100()
    {
        if(score >= 100)
        {
            score -= 100;
            currency += 100;
        }
    }
}
