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
    public int objectiveStar;
    //a number of objective items the player can purchase with points instead of coins.
    public int maxObjective;
    //maximum number of the objective items to trigger an endgame state
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
    //increases the cost of an item
    public float passivePointDecay;
    //at the endgame, the player will need to take steps to prevent points from receding. This is that decay.
    public bool isEndgameState;
    #endregion
    #region Game Objects
    public Text scoreText;
    public Text cmText;
    public Text ppcText;
    public Text currencyText;
    public Text objectiveText;
    public Text passClickText;
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
        objectiveStar = 0;
        maxObjective = 5;
        isEndgameState = false;
        passivePointDecay = score * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        score += passiveClickValue;
        scoreClickValue = baseClickValue + clickMultiplier;
        score += (numberOfAutoclickers * Time.deltaTime);
        scoreText.text = Mathf.FloorToInt(score) + " Points";
        cmText.text = "Click Multipliers: " + numberOfClickMultipliers.ToString();
        clickMultiplier = numberOfClickMultipliers * 0.25f;
        ppcText.text = "Points per Click: " + scoreClickValue.ToString();
        currencyText.text = "Currency: " + currency.ToString();
        objectiveText.text = objectiveStar.ToString() + "Stars / " + maxObjective.ToString();
        passClickText.text = "Passive Clicks: " + numberOfAutoclickers.ToString() + " Per second";
        if(objectiveStar >= maxObjective)
        {
            isEndgameState = true;
        }
        if(isEndgameState == true)
        {
            score -= passivePointDecay;
        }
    }
    public void ClickToScore()
    {
        score += scoreClickValue;
    }
    public void BuyPassiveClick()
    {
        if (currency >= costToUpgrade[1])
        {
            currency -= costToUpgrade[1];
            numberOfAutoclickers++;
            costToUpgrade[1] *= costIncrease;
        }
        
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
    public void BuyObjective()
    {
        if(score >= 1000)
        {
            objectiveStar++;
        }
    }
}
