using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    //removed start and update

    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private int width;
    private int height;
    private Snake snake;

    public LevelGrid(int _width, int _height)
    {
        this.width = _width;
        this.height = _height;
    }

    public void Setup(Snake snake)
    {
        this.snake = snake;

        SpawnFood();
    }

    public void SpawnFood()
    {

        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width+1), Random.Range(0, height+1));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y, 0);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            Debug.Log("Snake ate food");
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = height;
        }
        if (gridPosition.x > width)
        {
            gridPosition.x = 0;
        }
        if (gridPosition.y > height)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }

}
