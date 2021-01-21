using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    private static GameHandler instance;
    [SerializeField] private Snake snake;
    [SerializeField] private LevelGrid levelGrid;

    private static int score;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        
        levelGrid = new LevelGrid(14, 14);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
        score = 0;
        
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 1;
    }
}
