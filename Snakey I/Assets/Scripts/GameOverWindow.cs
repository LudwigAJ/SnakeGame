using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameOverWindow instance;


    private void Awake()
    {
        instance = this;

        transform.Find("PlayAgainButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.GameScene);
            }
        );

        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
    }



}
