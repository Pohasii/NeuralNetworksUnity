using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LinearRegression : MonoBehaviour
{
    public SpriteRenderer pointSpritePrefab;

    public LineRenderer line;
    public LineRenderer predictionLine;

    public float learningRate;

    public int pointsCount;
    public Vector2 bounds;

    public bool everyFrame;

    public Color[] pointColors;

    public float a, b;

    public Perceptron brain;

    private Point[] points;
    private SpriteRenderer[] pointViews = new SpriteRenderer[0];

    private void Awake()
    {
        brain = new Perceptron(3, learningRate, math.sign);

        InitPoints();
    }

    private void Update()
    {
        var ready = true;
        for (int i = 0; i < points.Length; i++)
        {
            ready = points[i].ready;
        }

        if (ready && Input.GetKeyDown(KeyCode.Space))
        {
            InitPoints();
        }

        if (Input.GetKeyDown(KeyCode.Space) || everyFrame)
        {
            for (int i = 0; i < points.Length; i++)
            {
                var point = points[i];

                var inputs = point.GetInputs();
                var target = point.Target;

                brain.Train(inputs, target);

                var prediction = brain.Predict(inputs);

                DrawInitialLine();
                DrawPredictionLine();
                points[i].CalculateTarget(F);

                var colorIndex = ((int)prediction + 1) / 2;
                pointViews[i].color = pointColors[colorIndex];

                if (Mathf.Approximately(prediction, target))
                {
                    points[i].ready = true;
                }
            }
        }
    }

    private void InitPoints()
    {
        foreach (var point in pointViews)
        {
            Destroy(point.gameObject);
        }

        points = new Point[pointsCount];
        pointViews = new SpriteRenderer[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            points[i].RandomizePosition(bounds);
            points[i].CalculateTarget(F);
            var pointView = Instantiate(pointSpritePrefab);
            pointViews[i] = pointView;

            pointView.color = pointColors[points[i].Target == -1 ? 0 : 1];

            var pointPos = points[i].Position;

            pointView.transform.position = new Vector3(pointPos.x, pointPos.y, 0);
        }
    }

    private void DrawInitialLine()
    {
        var x1 = -bounds.x;
        var y1 = F(x1);

        var x2 = bounds.x;
        var y2 = F(x2);

        line.SetPosition(0, new Vector2(x1, y1));
        line.SetPosition(1, new Vector2(x2, y2));
    }

    private void DrawPredictionLine()
    {
        var p1 = new Vector2(-bounds.x, GuessY(-bounds.x));
        var p2 = new Vector2(bounds.x, GuessY(bounds.x));

        predictionLine.SetPosition(0, p1);
        predictionLine.SetPosition(1, p2);
    }

    private float GuessY(float x)
    {
        var w0 = brain[0];
        var w1 = brain[1];
        var w2 = brain[2];

        return -(w2 / w1) - (w0 / w1) * x;
    }

    public float F(float x)
    {
        return a * x + b;
    }
}

public struct Point
{
    public Vector2 Position { get; private set; }

    public int Target;

    public bool ready;

    public float[] GetInputs()
    {
        return new float[] { Position.x, Position.y, 1 };
    }

    public void RandomizePosition(Vector2 bounds)
    {
        var x = Random.Range(bounds.x, -bounds.x);
        var y = Random.Range(bounds.y, -bounds.y);

        Position = new Vector2(x, y);
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }

    public void CalculateTarget(Func<float, float> func)
    {
        var result = func(Position.x);

        if (Position.y > result)
        {
            Target = 1;
        }
        else
        {
            Target = -1;
        }
    }
}