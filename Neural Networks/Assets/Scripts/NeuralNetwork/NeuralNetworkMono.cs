using MathExtended.Matrices;
using UnityEngine;

[System.Serializable]
public struct TrainingData
{
    public float[] inputs;
    public float[] target;
}

public class NeuralNetworkMono : MonoBehaviour
{
    public int iterationCount;

    public int hiddenCount;

    public float learningRate;

    public bool reset;

    public NeuralNetwork neuralNetwork;

    public TrainingData[] trainingDatas;

    private void Awake()
    {
        neuralNetwork = new NeuralNetwork(2, hiddenCount, 1, learningRate);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Train();
            Test();
        }
    }

    private void Train()
    {
        if(reset)
            neuralNetwork = new NeuralNetwork(2, 2, 1, learningRate);

        for (int i = 0; i < iterationCount; i++)
        {
            foreach (var data in trainingDatas)
            {
                neuralNetwork.Train(data.inputs, Matrix.FromArray(data.target));
            }
        }
        neuralNetwork.PrintWeights();
    }

    private void Test()
    {
        foreach (var data in trainingDatas)
        {
            Debug.Log(neuralNetwork.Feedforward(data.inputs));
        }
    }
}