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
    public float[] isDecayTime =
    {
        60,120,180,300
    };
    public float[] isDecayValue =
    {
        0.01f, 0.05f, 0.2f, 1f
    };
    public float currentDecayValue = 0;
    private bool hasPauseValue;
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
        passivePointDecay = 0.01f;
        currentDelayTime = 0;
        maxDelayTime = 10;
        hasPauseValue = false;
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
        if (objectiveStar >= maxObjective)
        {
            isEndgameState = true;
        }
        if (hasPauseValue == true)
        {
            Stall();
        }
    }
    private void FixedUpdate()
    {
        if(isEndgameState == true)
        {
            score -= (Time.deltaTime);
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
    public void BuyTimeStaller()
    {
        if(currency >= costToUpgrade[3])
        {
            currency -= costToUpgrade[3];
            hasPauseValue = true;
            // As the stall is intended to be used often and is a requirement for the endgame, it will not have a cost increase
        }
    }
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
    public void BuyObjective()
    {
        if(currency >= costToUpgrade[2] && objectiveStar != maxObjective)
        {
            currency -= costToUpgrade[2];
            objectiveStar++;
            costToUpgrade[2] *= costIncrease;
        }
    }
    public void CurrencyCheck()
    {
        if(currency >= isUnlockRequirement[0])
        {
            upgradeButtonPC.SetActive(true);
        }
        if(currency >= isUnlockRequirement[1])
        {
            upgradeButtonBO.SetActive(true);
        }
        if(currency >= isUnlockRequirement[2])
        {
            upgradeButtonST.SetActive(true);
        }
    }
    public void EndGame()
    {
        winScreen.SetActive(true);
    }
   IEnumerator Stall()
    {
        Debug.Log("10 Second Pause");
        yield return new WaitForSeconds(10);
        Debug.Log("Resuming Decay");
        hasPauseValue = false;
    }
    IEnumerator DecayIncrease()
    {
        for (int i = 0; i < isDecayTime.Length; i++)
        {
            //Slowly will ramp up the point decay over time
            yield return new WaitForSeconds(isDecayTime[i]);
            currentDecayValue = isDecayValue[i];
        }

    }
}
