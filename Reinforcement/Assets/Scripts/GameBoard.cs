using UnityEngine;

public class GameBoard : MonoBehaviour
{

    public enum Directions { Up, Right, Down, Left}

    public int maxHorizontalSize = 4;

    public int startingCell = 0;
    public int endCell = 15;

    public int[] coins;
    public int[] enemies;

    public Cell[] cells = new Cell[16];

    [System.Serializable]
    public class Cell
    {
        public bool[] walls = new bool[4];
    }

    public int Move(int currentCell, Directions direction)
    {
        int targetCell = TargetCellFromDirection(currentCell, direction);

        if (direction == Directions.Left && currentCell % maxHorizontalSize == 0)
        {
            return currentCell;
        }
        else if (direction == Directions.Right && (currentCell + 1) % maxHorizontalSize == 0)
        {
            return currentCell;
        }
        else
        {
            if (isCellInRange(targetCell))
            {
                if (cells[currentCell].walls[(int)direction] || cells[targetCell].walls[(int)OppositeDirection(direction)])
                {
                    return currentCell;
                }
                else
                {
                    return targetCell;
                }
            }
            else
            {
                return currentCell;
            }
        }

        return 0;
    }

    private bool isCellInRange(int cell)
    {
        return cell >= 0 && cell < cells.Length;
    }

    private int TargetCellFromDirection(int currentCell, Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                return currentCell + 4;
            case Directions.Down:
                return currentCell - 4;
            case Directions.Right:
                return currentCell + 1;
            case Directions.Left:
                return currentCell - 1;
        }

        return currentCell;
    }

    private Directions OppositeDirection(Directions direction)
    {
        switch (direction)
        {
            case Directions.Up:
                return Directions.Down;
            case Directions.Down:
                return Directions.Up;
            case Directions.Right:
                return Directions.Left;
            case Directions.Left:
                return Directions.Right;
        }

        return Directions.Up;
    }

    public bool CellHasCoin(int cell)
    {
        foreach (var currentCoin in coins)
        {
            if (currentCoin == cell)
            {
                return true;
            }
        }

        return false;
    }

    public bool CellHasEnemy(int cell)
    {
        foreach (var currentEnemy in enemies)
        {
            if (currentEnemy == cell)
            {
                return true;
            }
        }

        return false;
    }
}
