using UnityEngine;

public class GraphicBoard : MonoBehaviour
{
    [SerializeField] private GameBoard gameboard;
    [SerializeField] private GraphicCell cellPrefab;
    [SerializeField] private GraphicCell[] graphicCells;

    void Awake()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        graphicCells = new GraphicCell[gameboard.cells.Length];

        float cellDistance = cellPrefab.gameObject.transform.localScale.x * 1.2f;
        float xPosition = 0.0f;
        float zPosition = 0.0f;

        int sizeCounter = 0;

        for (int i = 0; i < gameboard.cells.Length; i++)
        {
            sizeCounter++;
            if (sizeCounter > gameboard.maxHorizontalSize)
            {
                zPosition += cellDistance;
                xPosition = 0.0f;
                sizeCounter = 1;
            }

            Vector3 newPosition = Vector3.zero;
            newPosition.x += xPosition;
            newPosition.z = zPosition;

            GraphicCell clone = Instantiate(cellPrefab, newPosition, Quaternion.identity);
            clone.SetupCell(i, gameboard.cells[i].walls);
            graphicCells[i] = clone;

            if (gameboard.CellHasCoin(i))
            {
                graphicCells[i].coinRepresentation.SetActive(true);
            }
            else if (gameboard.CellHasEnemy(i))
            {
                graphicCells[i].enemyRepresentation.SetActive(true);
            }

            xPosition += cellDistance;
        }
    }

    public Vector3 GetCellPosition(int cell)
    {
        return graphicCells[cell].transform.position + (Vector3.up * 0.5f);
    }
}
