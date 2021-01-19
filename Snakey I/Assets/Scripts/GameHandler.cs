using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Snake snake;
    [SerializeField] private LevelGrid levelGrid;

    // Start is called before the first frame update
    private void Start()
    {
        
        levelGrid = new LevelGrid(14, 14);
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
        
    }
    // Update is called once per frame
    private void Update()
    {
       
    }
}
