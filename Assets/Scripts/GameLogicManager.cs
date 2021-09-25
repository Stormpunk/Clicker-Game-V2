using Packages.Rider.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
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
    public float finishCost;
    //The cost of ending the game
    public float costIncrease = 1.1f;
    //increases the cost of an item
    public bool isEndgameState;
    //figures out whether or not the decay needs to be triggered
    public bool isDelaying;
    //the game will count down a delay when this bool is active
    public float currentDelayTime;
    public float maxDelayTime;
    //the number that the delay timer will reset to
    public GameObject endGameButton;
    public float[] isUnlockRequirement =
    {
        100, 500, 1000
    };
    public float currentDecayValue;
    //the value that the points decay from at the moment.
    public float oldDecayValue;
    public float frozenDecayValue;
    public float pointIncreaseTimer;
    public bool isStalling;
    int x = 0;
    #endregion
    #region Game Objects
    public Text scoreText;
    public Text cmText;
    public Text ppcText;
    public Text currencyText;
    public Text objectiveText;
    public Text passClickText;
    public GameObject upgradeButtonPC;
    public GameObject upgradeButtonBO;
    public GameObject upgradeButtonEG;
    public GameObject upgradeButtonST;
    public GameObject winScreen;
    public Text cmCost;
    public Text pcCost;
    public Text objCost;
    public Text stCost;


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
        currentDelayTime = maxDelayTime;
        maxDelayTime = 10;
        finishCost = 5000;
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
        objectiveText.text = "Objective: " + objectiveStar.ToString() + " / " + maxObjective.ToString() + " Stars";
        passClickText.text = "Passive Clicks: " + numberOfAutoclickers.ToString() + " Per second";
        #region Item Costs
        cmCost.text = costToUpgrade[0].ToString() + " Gold";
        pcCost.text = costToUpgrade[1].ToString() + " Gold";
        objCost.text = costToUpgrade[2].ToString() + " Gold";
        stCost.text = costToUpgrade[3].ToString() + " Gold";
        #endregion
        if (objectiveStar >= maxObjective)
        {
            isEndgameState = true;
        }
        if (isEndgameState)
        {
            pointIncreaseTimer += Time.deltaTime;
            oldDecayValue = (pointIncreaseTimer / 10);
        }
        if(isStalling == true && isStalling)
        {
            currentDecayValue = frozenDecayValue;
            currentDelayTime -= Time.deltaTime;
        }
        else
        {
            currentDecayValue = oldDecayValue;
        }
        if(currentDelayTime<= 0)
        {
            currentDelayTime = 0;
        }
        if(currentDelayTime == 0)
        {
            isStalling = false;
            currentDelayTime = maxDelayTime;
        }
    }
    private void FixedUpdate()
    {
        if (isEndgameState == true)
        {
            score -= (currentDecayValue * Time.deltaTime);
        }
    }
    public void ClickToScore()
    {
        score += scoreClickValue;
    }
    #region Buying
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
    public void BuyTimeStaller()
    {
        if(currency >= costToUpgrade[3] && isEndgameState)
        {
            currency -= costToUpgrade[3];
            isStalling = true;
            // As the stall is intended to be used often and is a requirement for the endgame, it will not have a cost increase
        }
    }
    public void BuyObjective()
    {
        if (currency >= costToUpgrade[2] && objectiveStar != maxObjective)
        {
            currency -= costToUpgrade[2];
            objectiveStar++;
            costToUpgrade[2] *= costIncrease;
        }
    }
    #endregion
    #region Points To Currency
    public void PointsToCurrency10()
    {
        if (score >= 10)
        {
            score -= 10;
            currency += 10;
        }
        CurrencyCheck();
    }
    public void PointsToCurrency50()
    {
        if(score >= 50)
        {
            score -= 50;
            currency += 50;
        }
        CurrencyCheck();
    }
    public void PointsToCurrency100()
    {
        if(score >= 100)
        {
            score -= 100;
            currency += 100;
        }
        CurrencyCheck();
    }
    public void CurrencyCheck()
    {
        if (currency >= isUnlockRequirement[0])
        {
            upgradeButtonPC.SetActive(true);
        }
        if (currency >= isUnlockRequirement[1])
        {
            upgradeButtonBO.SetActive(true);
        }
        if (currency >= isUnlockRequirement[2] && isEndgameState)
        {
            upgradeButtonST.SetActive(true);
        }
        if(currency >= finishCost && isEndgameState)
        {
            upgradeButtonEG.SetActive(true);
        }
    }
    #endregion
    public void EndGame()
    {
        if(currency >= finishCost)
        {
            winScreen.SetActive(true);
        }
    }
}
