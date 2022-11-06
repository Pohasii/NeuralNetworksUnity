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

    public Perceptron(int size, float learningRate, Func<float, float> activationFunction)
    {
        weights = new float[size];

        this.learningRate = learningRate;
        this.activationFunction = activationFunction;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = UnityEngine.Random.Range(-1f, 1);
        }
    }

    public float Guess(float[] inputs)
    {
        var summ = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            summ += inputs[i] * weights[i];
        }

        return activationFunction(summ);
    }

    public float Train(float[] inputs, int target)
    {
        var guess = Guess(inputs);
        var error = target - guess;

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] += error * inputs[i] * learningRate;
        }

        return guess;
    }
}