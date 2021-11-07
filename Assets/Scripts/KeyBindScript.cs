using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyBindScript : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public Text forward, backward, left, right, jump;
    private GameObject currentKey;
    // Start is called before the first frame update
    void Start()
    {
        keys.Add("forward", KeyCode.W);
        keys.Add("backward", KeyCode.S);
        keys.Add("left", KeyCode.A);
        keys.Add("right", KeyCode.D);
        keys.Add("jump", KeyCode.Space);

        forward.text = keys["forward"].ToString();
        backward.text = keys["backward"].ToString();
        left.text = keys["left"].ToString();
        right.text = keys["right"].ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keys["forward"]))
        {
            Debug.Log("forward");
        }//displays the action being done
        if (Input.GetKeyDown(keys["backward"]))
        {
            Debug.Log("backward");
        }//displays the action being done
        if (Input.GetKeyDown(keys["left"]))
        {
            Debug.Log("left");
        }//displays the action being done
        if (Input.GetKeyDown(keys["right"]))
        {
            Debug.Log("right");
        }//displays the action being done
    }

    void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }
}