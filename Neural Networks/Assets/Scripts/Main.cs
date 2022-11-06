using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    public SpriteRenderer pointSpritePrefab;

    public LineRenderer line;

    public float learningRate;

    public int pointsCount;
    public Vector2 bounds;

    public bool everyFrame;

    public Color[] pointColors;

    private Point[] points;
    private SpriteRenderer[] pointViews = new SpriteRenderer[0];

    public Perceptron perceptron;

    private void Awake()
    {
        InitPoints();

        perceptron = new Perceptron(3, learningRate, math.sign);
    }

    private void Update()
    {
        var ready = true;
        for (int i = 0; i < points.Length; i++)
        {
            ready = points[i].ready;
        }

        if (ready)
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

                var guess = perceptron.Train(inputs, target);

                if (Mathf.Approximately(guess, target))
                {
                    pointViews[i].color = pointColors[2];
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

        line.SetPosition(0, new Vector2(bounds.x, bounds.y));
        line.SetPosition(1, new Vector2(-bounds.x, -bounds.y));

        points = new Point[pointsCount];
        pointViews = new SpriteRenderer[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            points[i].RandomizePosition(bounds);
            points[i].CalculateTarget(bounds);
            var pointView = Instantiate(pointSpritePrefab);
            pointViews[i] = pointView;

            pointView.color = pointColors[points[i].Target == -1 ? 0 : 1];

            var pointPos = points[i].Position;

            pointView.transform.position = new Vector3(pointPos.x, pointPos.y, 0);
        }
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

    public void CalculateTarget(Vector2 bounds)
    {
        var result = PointTest(new Vector2(Position.x, Position.y), bounds);

        if (result > 1)
        {
            Target = 1;
        }
        else
        {
            Target = -1;
        }
    }

    public static float PointTest(Vector2 point, Vector2 bounds)
    {
        var D = (point.x - bounds.x) * (-bounds.y - bounds.y) - (point.y - bounds.y) * (-bounds.x - bounds.x);

        return D;
    }
}