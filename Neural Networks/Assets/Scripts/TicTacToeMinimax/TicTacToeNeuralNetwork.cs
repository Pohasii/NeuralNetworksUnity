using MathExtended.Matrices;
using UnityEngine;

[System.Serializable]
public class TicTacToeNeuralNetwork
{
    [SerializeField]
    private int iterationsCount;

    [SerializeField]
    private int hiddenCount;
    [SerializeField]
    private float learningRate;

    private TicTacToe ticTacToe;

    private NeuralNetwork brain;

    public void Init(TicTacToe ticTacToe)
    {
        this.ticTacToe = ticTacToe;
        brain = new NeuralNetwork(27, hiddenCount, 9, learningRate);
    }

    public void SimpleTrain()
    {
        for (int i = 0; i < iterationsCount; i++)
        {
            var bestMove = ticTacToe.CalculateBestMove();

            Train2(bestMove);
        }
    }

    public void Train()
    {
        ticTacToe.Move(Random.Range(0, 9));

        for (int i = 0; i < iterationsCount; i++)
        {
            var bestMove = ticTacToe.CalculateBestMove();
            ticTacToe.Move(bestMove);

            Train2(bestMove);

            if (ticTacToe.GameOver)
            {
                ticTacToe.ResetGame();

                ticTacToe.Move(Random.Range(0, 9));
            }
        }
    }

    private void Train(int bestMove)
    {
        float[] boardState = ticTacToe.GetBoardStateAsNormolizedFloatArray();

        var inputs = new float[boardState.Length + 1];
        boardState.CopyTo(inputs, 0);
        inputs[inputs.Length - 1] = ticTacToe.GetPlayerNormolized(ticTacToe.CurrentPlayer);

        var target = new Matrix(1);
        target[1, 1] = bestMove / 9f;

        brain.Train(inputs, target);
    }

    private void Train2(int bestMove)
    {
        float[] inputs = ticTacToe.GetBoardStateAsNormolizedFloatArray2();

        //var target = new Matrix(1);
        //target[1, 1] = bestMove / 9f;

        var array = new float[9];

        array[bestMove] = 1;

        var target = Matrix.FromArray(array);

        brain.Train(inputs, target);
    }

    public int Guess()
    {
        float[] boardState = ticTacToe.GetBoardStateAsNormolizedFloatArray();

        var inputs = new float[boardState.Length + 1];
        boardState.CopyTo(inputs, 0);
        inputs[inputs.Length - 1] = ticTacToe.GetPlayerNormolized(ticTacToe.CurrentPlayer);

        var guess = brain.Feedforward(inputs);

        return Mathf.RoundToInt((float)(guess[1, 1] * 9));
    }

    public int Guess2()
    {
        float[] inputs = ticTacToe.GetBoardStateAsNormolizedFloatArray2();

        var guess = brain.Feedforward(inputs);

        var max = float.MinValue;

        var index = -1;

        for (int i = 0; i < guess.Rows; i++)
        {
            if (guess[i] >= max)
            {
                max = (float)guess[i];
                index = i;
            }
        }

        return index;//Mathf.RoundToInt((float)(guess[1, 1] * 9));
    }

    public void PrintWeights()
    {
        brain.PrintWeights();
    }
}