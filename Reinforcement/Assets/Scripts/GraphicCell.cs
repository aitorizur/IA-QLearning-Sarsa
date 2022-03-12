using UnityEngine;

public class GraphicCell : MonoBehaviour
{
    public TextMesh textMesh;
    public GameObject[] graphicWalls = new GameObject[4];
    public GameObject enemyRepresentation;
    public GameObject coinRepresentation;
    public void SetupCell(int cellNumber, bool[] walls)
    {
        for (int i = 0; i < walls.Length; i++)
        {
            graphicWalls[i].SetActive(walls[i]);
        }

        textMesh.text = cellNumber.ToString();
    }
}
