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
    public GameObject endGameText;
    public GameObject stallText;


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
        maxObjective = 7;
        isEndgameState = false;
        currentDelayTime = maxDelayTime;
        maxDelayTime = 10;
        finishCost = 5000;
    }

    // Update is called once per frame
    void Update()
    {
        scoreClickValue = baseClickValue + clickMultiplier;
        //equation to figure out how many points to award on click
        score += (numberOfAutoclickers * Time.deltaTime);
        //starts at 0, will add points per second with upgrades
        scoreText.text = Mathf.FloorToInt(score) + " Points";
        //updates the text with the score value
        clickMultiplier = numberOfClickMultipliers * 0.25f;
        //updates the value of click multipliers
        cmText.text = "Follower Efficiency: " + numberOfClickMultipliers.ToString();
        ppcText.text = "Follower Efficiency: " + scoreClickValue.ToString();
        currencyText.text = "Currency: " + currency.ToString();
        objectiveText.text = "Objective: " + objectiveStar.ToString() + " / " + maxObjective.ToString() + " Stars";
        passClickText.text = "Word of Mouth: " + numberOfAutoclickers.ToString() + " Per second";
        //updates texts with counters for upgrades or score per seconds.
        #region Item Costs
        cmCost.text = costToUpgrade[0].ToString() + " Currency";
        pcCost.text = costToUpgrade[1].ToString() + " Currency";
        objCost.text = costToUpgrade[2].ToString() + " Currency";
        stCost.text = costToUpgrade[3].ToString() + " Currency";
        //Updates the costs of the store items
        #endregion
        if (objectiveStar >= maxObjective)
        {
            isEndgameState = true;
        }
        //the game will enter the endgame state when the player has reached 7/7 objectives
        if (isEndgameState)
        {
            pointIncreaseTimer += Time.deltaTime;
            oldDecayValue = (pointIncreaseTimer / 10);
            endGameText.SetActive(true);
        }
        //decays the players points at a slowly increasing value, along with creating an indication 
        if(isStalling == true && isStalling)
        {
            currentDecayValue = frozenDecayValue;
            currentDelayTime -= Time.deltaTime;
        }
        //halts the point decay while the stall is active.
        else
        {
            currentDecayValue = oldDecayValue;
        }
        //returjns the decay value to the pre-stalled value
        if(currentDelayTime<= 0)
        {
            currentDelayTime = 0;
        }
        if(currentDelayTime == 0)
        {
            isStalling = false;
            currentDelayTime = maxDelayTime;
        }
        //resets the delay counter and disables the stalling state.
    }
    private void FixedUpdate()
    {
        if (isEndgameState == true)
        {
            score -= (currentDecayValue * Time.deltaTime);
        }
        //decreases the score by the decay value per second if the endgame state is active.
    }
    public void ClickToScore()
    {
        score += scoreClickValue;
    }
    //points go up when the player clicks the button. Simples!
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
    //buying passive clicks
    public void BuyClickMultiplier()
    {
        if (currency >= costToUpgrade[0])
        {
            currency -= costToUpgrade[0];
            numberOfClickMultipliers++;
            costToUpgrade[0] *= costIncrease;
        }
        
    }
    //buying click multipliers
    public void BuyTimeStaller()
    {
        if(currency >= costToUpgrade[3] && isEndgameState)
        {
            currency -= costToUpgrade[3];
            isStalling = true;
            // As the stall is intended to be used often and is a requirement for the endgame, it will not have a cost increase
        }
    }
    //buying the decay stall.
    public void BuyObjective()
    {
        if (currency >= costToUpgrade[2] && objectiveStar != maxObjective)
        {
            currency -= costToUpgrade[2];
            objectiveStar++;
            costToUpgrade[2] *= costIncrease;
        }
    }
    //adds 1 to the objectives that trigger the endgame state
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
    //these functions all convert points to a set amount of currency 
    #endregion
    public void EndGame()
    {
        if(currency >= finishCost)
        {
            winScreen.SetActive(true);
        }
    }

    public void DebugAdd100()
    {
        currency += 100;
        CurrencyCheck();
    }
    public void DebugAdd1000()
    {
        currency += 1000;
        CurrencyCheck();
    }
    public void DebugAdd10000()
    {
        currency += 10000;
        CurrencyCheck();
    }
    //ends the game if the conditions are met
}
