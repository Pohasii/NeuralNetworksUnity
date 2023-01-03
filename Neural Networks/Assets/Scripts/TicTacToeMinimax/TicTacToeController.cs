using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public struct PlayerData
{
    public char player;
    public bool isBot;
}

[RequireComponent(typeof(TicTacToeView))]
public class TicTacToeController : MonoBehaviour
{
    public TicTacToeNeuralNetwork ticTacToeAI;

    public PlayerData[] players;

    private bool draw = true;

    private TicTacToeView view;

    private TicTacToe ticTacToe;

    private void Awake()
    {
        InitTicTacToe();
    }

    private void OnDestroy()
    {
        ticTacToe.OnMove -= TicTacToe_OnMove;
        ticTacToe.OnWin -= TicTacToe_OnWin;
        Slot.OnClick -= Slot_OnClick;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ticTacToeAI.SimpleTrain();

            var bestMove = ticTacToe.CalculateBestMove();
            var aiMove = ticTacToeAI.Guess2();
            Debug.Log($"Best move: {bestMove}  AIMove: {aiMove}");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetGame();
            draw = false;

            ticTacToeAI.Train();

            draw = true;
            ResetGame();

            Debug.Log("Ready");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            var bestMove = ticTacToe.CalculateBestMove();
            var aiMove = ticTacToeAI.Guess2();
            Debug.Log($"Best move: {bestMove}  AIMove: {aiMove}");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ticTacToe.DoBestMove();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ResetGame()
    {
        ticTacToe.ResetGame();
        view.ResetGame();
    }

    private void InitTicTacToe()
    {
        view = GetComponent<TicTacToeView>();

        ticTacToe = new TicTacToe(players[0].player, players[1].player);

        ticTacToeAI.Init(ticTacToe);

        view.Init(3);

        ticTacToe.OnMove += TicTacToe_OnMove;
        ticTacToe.OnWin += TicTacToe_OnWin;
        Slot.OnClick += Slot_OnClick;

        if (players[0].isBot)
        {
            ticTacToe.Move(4);
        }
    }

    private void Slot_OnClick(int index, PointerEventData.InputButton button)
    {
        if (button == PointerEventData.InputButton.Right)
        {
            var result = ticTacToe.EvaluateMove(index);
            Debug.Log(result);

            return;
        }

        ticTacToe.Move(index);
    }

    private bool IsPlayerBot(char player)
    {
        var currentPlayer = players.Where(x =>
        {
            return x.player == player;

        }).FirstOrDefault();

        return currentPlayer.isBot;
    }

    private void TicTacToe_OnMove(int cell, char playerOnThisTurn)
    {
        if (!draw) return;

        view.Move(cell, playerOnThisTurn);

        if (IsPlayerBot(ticTacToe.CurrentPlayer))
        {
            ticTacToe.DoBestMove();
        }
    }

    private void TicTacToe_OnWin(char winner)
    {
        if (!draw) return;

        view.ShowWinner(winner);
    }
}