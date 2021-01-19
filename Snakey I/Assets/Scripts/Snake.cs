using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private enum State
    {
        Alive,
        Dead
    }

    private State state;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private Direction lastGridMoveDirection;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private SnakeTail snakeTail;

    private GameObject snakeBodyGameObject;

    public void Setup(LevelGrid levelGrid) //A reference to LevelGrid so that we can access its methods.
    {
        this.levelGrid = levelGrid;
    }

    // Awake is called before the first render of Snake.
    private void Awake()
    {
        gridPosition = new Vector2Int(5, 5);
        gridMoveTimerMax = 0.2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        lastGridMoveDirection = gridMoveDirection;

        snakeTail = new SnakeTail(new Vector2Int(4, 5), Direction.Right);

        snakeMovePositionList = new List<SnakeMovePosition>(); //Where we keep where body of snake should be.
        snakeBodySize = 1;

        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
    }

    private void Update() //Update each frame using these.
    {
        switch (state)
        {
            case State.Alive:
                InputHandler();
                GridMovementHandler();
                break;
            case State.Dead:
                break;
            default:
                break;
        }
        
        
        
    }

    private void InputHandler()
    {

        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (lastGridMoveDirection != Direction.Down)
            {
                Debug.Log("Pressed Up");
                gridMoveDirection = Direction.Up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (lastGridMoveDirection != Direction.Up)
            {
                Debug.Log("Pressed Down");
                gridMoveDirection = Direction.Down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (lastGridMoveDirection != Direction.Left)
            {
                Debug.Log("Pressed Right");
                gridMoveDirection = Direction.Right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (lastGridMoveDirection != Direction.Right)
            {
                Debug.Log("Pressed Left");
                gridMoveDirection = Direction.Left;
            }
        }


    }

    private void GridMovementHandler()
    {


        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax; //Check so that the snake only moves once every gridMoveTimerMax.

            SnakeMovePosition previousSnakePosition = null;

            if (snakeMovePositionList.Count > 0)
            {
                previousSnakePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;

            /*
            * Convert our enum of direction to an actual vector we can use.
            */

            switch (gridMoveDirection)
            {
                case Direction.Up:
                    gridMoveDirectionVector = new Vector2Int(0, 1);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case Direction.Down:
                    gridMoveDirectionVector = new Vector2Int(0, -1);
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case Direction.Left:
                    gridMoveDirectionVector = new Vector2Int(-1, 0);
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case Direction.Right:
                    gridMoveDirectionVector = new Vector2Int(1, 0);
                    transform.eulerAngles = new Vector3(0, 0, 270);
                    break;
                default:
                    gridMoveDirectionVector = new Vector2Int(0, 1);
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
            }

            gridPosition += gridMoveDirectionVector; //Move snake to new position.
            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            /*
            * Handle our growing of the snake and eating of food.
            */

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition); //Check if snake ate food and grow body.
            if (snakeAteFood)
            {
                snakeBodySize += 1;
                CreateSnakeBodyPart();
            }

            if (snakeMovePositionList.Count > snakeBodySize)
            { 
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1); //Remove from list old body parts
            }

            lastGridMoveDirection = gridMoveDirection; //Save so we can check for not moving 180degrees.

            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0); // Update the snake's position.


            /*
            * Handles the new rendering of the snake.
            */

            UpdateSnakeBodyPart();

            /*
             * Check that the head hasn't hit the snakeBody.
             */

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    /*
                     * GAME OVER!
                     */
                    Debug.Log("GAME OVER!");
                    state = State.Dead;
                }
            }

            if (gridPosition == snakeTail.GetGridPosition())
            {
                Debug.Log("GAME OVER!");
                state = State.Dead;
            }


           
            
            
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));   
    }

    private void UpdateSnakeBodyPart()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
        snakeTail.SetSnakeTailMovePosition(snakeMovePositionList[snakeMovePositionList.Count-1], snakeBodyPartList.Count+1);
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };

        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }

        return gridPositionList;
    }

    /*
     * Handle snake tail.
     */

    private class SnakeTail
    {
        private SnakeMovePosition snakeMovePosition;
        private GameObject snakeTailGameObject;
        private Transform transform;

        public SnakeTail(Vector2Int gridPosition, Direction direction)
        {
            snakeTailGameObject = new GameObject("SnakeTail", typeof(SpriteRenderer));
            snakeTailGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeTail;
            snakeTailGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            transform = snakeTailGameObject.transform;
            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);

            switch (direction)
            {
                case Direction.Up:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case Direction.Down:
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case Direction.Left:
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case Direction.Right:
                    transform.eulerAngles = new Vector3(0, 0, 270);
                    break;
                default:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
            }
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }

        public void SetSnakeTailMovePosition(SnakeMovePosition snakeMovePosition, int bodyIndex)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y, 0);
            snakeTailGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;

            switch (snakeMovePosition.GetDirection())
            {
                case Direction.Up:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case Direction.Down:
                    transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case Direction.Left:
                    transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case Direction.Right:
                    transform.eulerAngles = new Vector3(0, 0, 270);
                    break;
                default:
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
            }
        }


    }

    /*
     * Handles 1 snake body part and its rendering.
     */

    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private GameObject snakeBodyGameObject;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y, 0);

            switch (snakeMovePosition.GetDirection())
            {
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        case Direction.Right:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody2;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Right to Up");
                            break;
                        case Direction.Left:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody4;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Left to Up");
                            break;
                        default:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        case Direction.Right:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody1;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Right to Down");
                            break;
                        case Direction.Left:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody3;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Left to Down");
                            break;
                        default:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody;
                            transform.eulerAngles = new Vector3(0, 0, 180);
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        case Direction.Down:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody2;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Down to Left");
                            break;
                        case Direction.Up:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody1;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Up to Left");
                            break;
                        default:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody;
                            transform.eulerAngles = new Vector3(0, 0, 90);
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        case Direction.Down:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody4;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Down to Right");
                            break;
                        case Direction.Up:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody3;
                            transform.eulerAngles = new Vector3(0, 0, 0);
                            Debug.Log("Went from Up to Right");
                            break;
                        default:
                            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody;
                            transform.eulerAngles = new Vector3(0, 0, 270);
                            break;
                    }
                    break;
                default:
                    snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBody;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
            }
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }

    /*
    * Handles the move direction from the snake to its body parts.
    */

    class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }
    }
}