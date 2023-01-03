using System;
using UnityEngine;

public class TicTacToe
{
    private const char TIE = '-', Empty = ' ';

    public char CurrentPlayer { get; private set; }
    private char player1;
    private char player2;

    private char[] board;

    public char[] Board => board;

    public bool GameOver { get; private set; }

    public int Size { get; private set; }

    public event Action<int, char> OnMove;
    public event Action<char> OnWin;

    public char this[int row, int indexInRow]
    {
        get
        {
            var index = ToIndex(row, indexInRow);

            return board[index];
        }

        set
        {
            var index = ToIndex(row, indexInRow);

            board[index] = value;
        }
    }

    public TicTacToe(char player1, char player2)
    {
        Size = 3;

        CurrentPlayer = player1;
        this.player1 = player1;
        this.player2 = player2;

        board = new char[Size * Size];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = Empty;
        }
    }

    public void ResetGame()
    {
        GameOver = false;

        for (int i = 0; i < board.Length; i++)
        {
            board[i] = Empty;
        }
    }

    public void Move(int index)
    {
        if (GameOver) return;

        board[index] = CurrentPlayer;

        CurrentPlayer = GetOtherPlayer();

        var winner = CheckWinner();

        if (winner != Empty)
        {
            GameOver = true;
            OnWin?.Invoke(winner);
        }

        OnMove?.Invoke(index, board[index]);
    }

    public void NextAvailableSlotMove()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (this[i, j] == Empty)
                {
                    var index = ToIndex(i, j);

                    Move(index);
                    return;
                }
            }
        }
    }

    public int CalculateBestMove()
    {
        var bestScore = int.MinValue;

        var bestMove = -1;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == Empty)
            {
                board[i] = CurrentPlayer;

                var score = Minimax(false);

                board[i] = Empty;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        return bestMove;
    }

    public void DoBestMove()
    {
        var bestMove = CalculateBestMove();

        Move(bestMove);
    }

    private int Minimax(bool isMaximizing)
    {
        var winner = CheckWinner();

        if (winner != Empty)
        {
            if (winner == CurrentPlayer)
            {
                return 1;
            }
            else if (winner == TIE)
            {
                return 0;
            }

            return -1;
        }

        var player = isMaximizing ? CurrentPlayer : GetOtherPlayer();
        var bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == Empty)
            {
                board[i] = player;

                var score = Minimax(!isMaximizing);

                board[i] = Empty;

                bestScore = isMaximizing
                    ? Math.Max(score, bestScore)
                    : Math.Min(score, bestScore);
            }
        }

        return bestScore;
    }

    public int EvaluateMove(int index)
    {
        var prevState = board[index];

        board[index] = CurrentPlayer;

        var score = Minimax(false);

        board[index] = prevState;

        return score;
    }

    public char GetOtherPlayer()
    {
        return CurrentPlayer == player1 ? player2 : player1;
    }

    public float GetPlayerNormolized(char player)
    {
        var currentPlayer = 0f;

        if (player == 'X')
        {
            currentPlayer = 0.5f;
        }
        else if (player == 'O')
        {
            currentPlayer = 1f;
        }

        return currentPlayer;
    }

    public float[] GetBoardStateAsNormolizedFloatArray()
    {
        float[] boardState = new float[board.Length];

        for (int i = 0; i < board.Length; i++)
        {
            var slotState = board[i] == Empty
                ? 0f
                : GetPlayerNormolized(board[i]);

            boardState[i] = slotState;
        }

        return boardState;
    }

    public float[] GetBoardStateAsNormolizedFloatArray2()
    {
        float[] boardState = new float[board.Length * 3];

        for (int i = 0; i < board.Length; i++)
        {
            boardState[i] = board[i] == 'X' ? 1 : 0;
            boardState[i + board.Length] = board[i] == 'O' ? 1 : 0;
            boardState[i + board.Length * 2] = board[i] == Empty ? 1 : 0;
        }

        return boardState;
    }

    private char CheckWinner()
    {
        var winner = Empty;

        for (int i = 0; i < Size; i++)
        {
            if (Equals3(this[0, i], this[1, i], this[2, i]))
            {
                winner = this[0, i];
            }

            if (Equals3(this[i, 0], this[i, 1], this[i, 2]))
            {
                winner = this[i, 0];
            }
        }

        if (Equals3(this[0, 0], this[1, 1], this[2, 2]))
        {
            winner = this[0, 0];
        }

        if (Equals3(this[2, 0], this[1, 1], this[0, 2]))
        {
            winner = this[2, 0];
        }

        var openSpots = 0;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == Empty)
            {
                openSpots++;
            }
        }

        if (winner == Empty && openSpots == 0)
        {
            winner = TIE;
        }

        return winner;
    }

    private bool Equals3(char a, char b, char c)
    {
        return (a != Empty) && (a == b) && (b == c);
    }

    private int ToIndex(int row, int indexInRow)
    {
        return Array2DUtils.ToIndex(row, indexInRow, Size);
    }
}