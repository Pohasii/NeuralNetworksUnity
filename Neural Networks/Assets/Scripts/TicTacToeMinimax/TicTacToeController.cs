using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PlayerData
{
    public char player;
    public bool isBot;
}

[RequireComponent(typeof(TicTacToeView))]
public class TicTacToeController : MonoBehaviour
{
    public PlayerData[] players;

    private TicTacToeView view;

    private TicTacToe ticTacToe;

    private void Awake()
    {
        view = GetComponent<TicTacToeView>();

        ticTacToe = new TicTacToe(players[0].player, players[1].player);

        view.Init(3);

        ticTacToe.OnMove += TicTacToe_OnMove;
        ticTacToe.OnWin += TicTacToe_OnWin;
        Slot.OnClick += Slot_OnClick;

        if (players[0].isBot)
        {
            ticTacToe.Move(4);
        }
    }

    private void OnDestroy()
    {
        ticTacToe.OnMove -= TicTacToe_OnMove;
        ticTacToe.OnWin -= TicTacToe_OnWin;
        Slot.OnClick -= Slot_OnClick;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ticTacToe.BestMove();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        view.Move(cell, playerOnThisTurn);

        if (IsPlayerBot(ticTacToe.CurrentPlayer))
        {
            ticTacToe.BestMove();
        }
    }

    private void TicTacToe_OnWin(char winner)
    {
        view.ShowWinner(winner);
    }
}