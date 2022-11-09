using TMPro;
using UnityEngine;

public enum State
{
    Train, Predict
}

public class Arithmetic : MonoBehaviour
{
    public float a, b;

    public float learningRate;
    public int iterationCount;
    public State state;
    public bool everyFrame;

    public int operandsRange;

    public float[] initialWeights;

    public Perceptron brain;

    public TextMeshProUGUI answerText;
    public TextMeshProUGUI operand1;
    public TextMeshProUGUI operand2;

    private void Awake()
    {
        brain = new Perceptron(3, learningRate, (x) => x);
        if (initialWeights.Length == brain.Size)
        {
            brain.SetWeights(initialWeights);
        }
    }

    private void Update()
    {
        if (everyFrame || Input.GetKeyDown(KeyCode.Space))
        {
            RandomizeOperands();
        }

        switch (state)
        {
            case State.Train: Train(); break;
            case State.Predict: Predict(); break;
        }
    }

    private void Train()
    {
        var input = new float[3] { a, b, 1 };

        var target = a + b;

        for (int i = 0; i < iterationCount; i++)
        {
            brain.Train(input, target);
        }
        var prediction = brain.Predict(input);

        if (Mathf.Abs(target - prediction) < 0.1f)
        {
            RandomizeOperands();
        }

        SetAnswerText(prediction);
    }

    private void Predict()
    {
        var input = new float[3] { a, b, 1 };

        var prediction = brain.Predict(input);

        SetAnswerText(prediction);
    }

    private void RandomizeOperands()
    {
        a = Random.Range(-operandsRange, operandsRange);
        b = Random.Range(-operandsRange, operandsRange);
    }

    private void SetAnswerText(float answer)
    {
        answer = Mathf.Round(answer);
        operand1.text = a.ToString();
        operand2.text = b.ToString();
        answerText.text = answer.ToString();
    }
}
