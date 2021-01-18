using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    private void Awake()
    {
        instance = this;
    } 
    public Sprite snakeHeadSprite;
    public Sprite foodSprite;
    public Sprite snakeBody;
    public Sprite snakeBody1;
    public Sprite snakeBody2;
    public Sprite snakeBody3;
    public Sprite snakeBody4;
    public Sprite snakeTail;

}

