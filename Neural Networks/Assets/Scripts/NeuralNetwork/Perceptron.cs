using System;
using UnityEngine;

[System.Serializable]
public class Perceptron
{
    [field: SerializeField]
    private float[] weights;

    [field: SerializeField]
    private float learningRate;

    private readonly Func<float, float> activationFunction;

    public int Size { get; private set; }

    public float this[int i]
    {
        get => weights[i];
    }

    public Perceptron(int size, float learningRate, Func<float, float> activationFunction)
    {
        weights = new float[size];

        Size = size;

        this.learningRate = learningRate;
        this.activationFunction = activationFunction;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = UnityEngine.Random.Range(-1f, 1);
        }
    }

    public void SetWeights(float[] weights)
    {
        this.weights = weights;
    }

    public float Predict(float[] inputs)
    {
        var summ = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            summ += inputs[i] * weights[i];
        }

        return activationFunction(summ);
    }

    public void Train(float[] inputs, float target)
    {
        var guess = Predict(inputs);
        var error = target - guess;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] += error * inputs[i] * learningRate;
        }
    }
}