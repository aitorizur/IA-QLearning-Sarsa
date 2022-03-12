using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private GraphicBoard graphicBoard;
    [SerializeField] private TrailRenderer trail;

    public int currentCell = 0;

    void Start()
    {
        ResetPlayer();
        trail.Clear();
    }

    public bool Move(GameBoard.Directions direction)
    {
        currentCell = gameBoard.Move(currentCell, direction);
        transform.position = graphicBoard.GetCellPosition(currentCell);

        if (currentCell == gameBoard.endCell)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetPlayer()
    {
        currentCell = gameBoard.startingCell;
        transform.position = graphicBoard.GetCellPosition(currentCell);
    }
}
