using UnityEngine;
using System.Collections;

public class SARSA : MonoBehaviour
{
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private PlayerController playerController;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float discountRate = 1.0f;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float learningRate = 1.0f;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float epsilon = 0.05f;

    [SerializeField] private float waitTime = 0.1f;

    [SerializeField] private int rewardPerMovement = -1;
    [SerializeField] private int rewardForReachingGoal = 100;
    [SerializeField] private int rewardForNotMoving = -100;
    [SerializeField] private int rewardForCoin = 10;
    [SerializeField] private int rewardForDying = -1000;

    private float[,] stateActions;

    //Iniciamos el bucle de selección
    void Awake()
    {
        stateActions = new float[gameBoard.cells.Length, 4];
        StartCoroutine(SARSALoop());
    }

    private IEnumerator SARSALoop()
    {
        while (true)
        {
            //Guardamos el estado actual
            int state1 = playerController.currentCell;

            //Encontramos una acción con la política de selección epsilon-greedy
            int action1 = EpsilonFGreedySelection(state1);

            //Movemos al agente en esa dirección
            playerController.Move((GameBoard.Directions)action1);

            //Guardamos el estado resultante de dicha acción
            int state2 = playerController.currentCell;

            //Evaluamos la recompensa de la acción tomada
            int reward = Reward(state1, state2);

            //Calculamos la siguiente mejor acción con la política epsilon greedy de nuevo
            int action2 = EpsilonFGreedySelection(state2);

            //Actualizamos el valor Q pero a diferencia de Qlearning, lo hacemos con la valoración epsilon-greedy y no con la mejor acción posible
            stateActions[state1, action1] += learningRate * (reward + discountRate * stateActions[state2, action2]) - stateActions[state1, action1];

            yield return new WaitForSeconds(waitTime);
        }
    }

    //Devuelve una recompensa en función del movimiento hecho
    private int Reward(int initialState, int endState)
    {
        if (endState == gameBoard.endCell)
        {
            playerController.ResetPlayer();
            return rewardForReachingGoal;
        }
        else if (initialState == endState)
        {
            return rewardForNotMoving;
        }
        else if (gameBoard.CellHasCoin(endState))
        {
            return rewardForCoin;
        }
        else if (gameBoard.CellHasEnemy(endState))
        {
            return rewardForDying;
        }
        else
        {
            return rewardPerMovement;
        }
    }

    //Escoge entre coger la mejor opción posible o coger uan acción aleatoria para favorecer la exploración. Lo hacer en función de epsilon, que deberia favorecer coger la mejor opción la mayora de las veces
    private int EpsilonFGreedySelection(int state)
    {
        float randomFloat = Random.value;

        if (randomFloat > epsilon)
        {
            return BestActionForState(state);
        }
        else
        {
            return Random.Range(0, 4);
        }
    }

    //Devuelve la mejor acción para un estado
    private int BestActionForState(int state)
    {
        float bestScore = stateActions[state, 0];
        int bestAction = 0;

        for (int i = 0; i < 4; i++)
        {
            float currentScore = stateActions[state, i];
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestAction = i;
            }
        }

        return bestAction;
    }
}

