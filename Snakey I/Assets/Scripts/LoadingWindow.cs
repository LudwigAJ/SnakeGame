using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{

    private Text text;
    private string dots;
    private float timeMax;
    private float timeCurrent;
    // Start is called before the first frame update
    private void Awake()
    {
        text = transform.Find("LoadingText").GetComponent<Text>(); //Pretty wonky lol.
        dots = "";
        timeMax = 0.25f;
        timeCurrent = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        timeCurrent += Time.deltaTime;
        if (timeCurrent >= timeMax)
        {
            timeCurrent = 0;
            switch (dots)
            {
                case "":
                    dots = ".";
                    break;
                case ".":
                    dots = "..";
                    break;
                case "..":
                    dots = "...";
                    break;
                case "...":
                    dots = "";
                    break;
                default:
                    dots = "";
                    break;
            }
        }
        text.text = "Loading" + dots;
    }
}
