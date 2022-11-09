using MathExtended.Matrices;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class NeuralNetwork
{
    private readonly int inputCount;
    private readonly int hidenCount;
    private readonly int outputCount;

    [SerializeField]
    private float learningRate;

    private Matrix weights_ih;
    private Matrix weights_ho;
    private Matrix bias_h;
    private Matrix bias_o;

    public NeuralNetwork(int inputCount, int hidenCount, int outputCount, float learningRate)
    {
        this.inputCount = inputCount;
        this.hidenCount = hidenCount;
        this.outputCount = outputCount;

        this.learningRate = learningRate;

        weights_ih = new Matrix(hidenCount, inputCount);
        weights_ho = new Matrix(outputCount, hidenCount);

        weights_ih.Randomize(-1, 1);
        weights_ho.Randomize(-1, 1);

        bias_h = new Matrix(hidenCount, 1);
        bias_o = new Matrix(outputCount, 1);

        bias_h.Randomize(-1, 1);
        bias_o.Randomize(-1, 1);
    }

    public Matrix Feedforward(float[] inputArray)
    {
        var inputs = Matrix.FromArray(inputArray);

        var hidden = GenerateHiddenOutputs(inputs);

        var output = GeneratingOutputsOutput(hidden);

        return output;
    }

    private Matrix GenerateHiddenOutputs(Matrix input)
    {
        var hidden = weights_ih * input;
        hidden.Add(bias_h);
        hidden.Map(Sigmoid);

        return hidden;
    }

    private Matrix GeneratingOutputsOutput(Matrix hidden)
    {
        var output = weights_ho * hidden;
        output.Add(bias_o);
        output.Map(Sigmoid);

        return output;
    }

    public void Train(float[] inputArray, Matrix targets)
    {
        var inputs = Matrix.FromArray(inputArray);

        var hidden = GenerateHiddenOutputs(inputs);

        var outputs = GeneratingOutputsOutput(hidden);

        var outputErrors = targets - outputs;

        //Calculate outputs gradient
        var outputGradient = outputs.Duplicate();
        outputGradient.Map(DSigmoid);
        outputGradient.HadamardProduct(outputErrors);
        outputGradient.Multiply(learningRate);

        //Calculate outputs deltas
        var weight_ho_deltas = outputGradient * (~hidden);
        //Addjust the weights by deltas
        weights_ho.Add(weight_ho_deltas);
        //Addjust the bias by its deltas (wich is just the gradients)
        bias_o.Add(outputGradient);

        //Calculate the hidden layer errors
        var hiddenErrors = ~weights_ho;
        hiddenErrors.Multiply(outputErrors);

        //Calculate hidden gradient
        var hiddenGradient = hidden.Duplicate();
        hiddenGradient.Map(DSigmoid);
        hiddenGradient.HadamardProduct(hiddenErrors);
        hiddenGradient.Multiply(learningRate);

        //Calculate input -> hidden deltas
        var weight_hi_deltas = hiddenGradient * (~inputs);
        //Addjust the weights by deltas
        weights_ih.Add(weight_hi_deltas);
        //Addjust the bias by its deltas (wich is just the gradients)
        bias_h.Add(hiddenGradient);
    }

    public void PrintWeights()
    {
        Debug.Log($"weights_ho = {weights_ho}");
        Debug.Log($"bias_o = {bias_o}");
    }

    private double Sigmoid(double x)
    {
        return 1 / (1 + math.exp(-x));
    }

    private double DSigmoid(double y)
    {
        return y * (1 - y);
    }
}