using UnityEngine;
using System.Collections;

public class QLearning : MonoBehaviour
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
        StartCoroutine(QLoop());
    }

    private IEnumerator QLoop()
    {
        

        while (true)
        {
            //Guardamos el estado actual
            int currentState = playerController.currentCell;

            //Encontramos una acción con la política de selección epsilon-greedy
            int bestCurrentAction = EpsilonFGreedySelection(currentState);

            //Movemos al agente en esa dirección
            playerController.Move((GameBoard.Directions)bestCurrentAction);

            //Guardamos el estado resultante de dicha acción
            int newState = playerController.currentCell;

            //Evaluamos la recompensa de la acción tomada
            int reward = Reward(currentState, newState);

            //Guardamos una valoración en el estado-acción elegido en función de la mejor acción posible al que hemos llegado
            stateActions[currentState, bestCurrentAction] += learningRate * (reward + discountRate * stateActions[newState, BestActionForState(newState)]) - stateActions[currentState, bestCurrentAction];

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
