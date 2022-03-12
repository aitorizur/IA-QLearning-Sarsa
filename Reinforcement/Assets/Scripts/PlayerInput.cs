using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    void Update()
    {
        bool reachedObjective = false;

        if (Input.GetKeyDown(KeyCode.W))
        {
            reachedObjective = playerController.Move(GameBoard.Directions.Up);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            reachedObjective = playerController.Move(GameBoard.Directions.Right);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            reachedObjective = playerController.Move(GameBoard.Directions.Left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            reachedObjective = playerController.Move(GameBoard.Directions.Down);
        }

        if (reachedObjective)
        {
            playerController.ResetPlayer();
        }
    }
}
