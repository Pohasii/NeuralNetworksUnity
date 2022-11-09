using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Academy : MonoBehaviour
{
    public Unit unitToTrain;
    public List<Unit> targets;

    public Color correctColor;
    public Color incorrectColor;

    public int iterationCount;

    public float learningRate;

    public Perceptron brain;

    private void Awake()
    {
        brain = new Perceptron(3, learningRate, math.tanh);
        unitToTrain.SetBrain(brain);
        FindTargets();
    }

    private void Update()
    {
        Train();
    }

    private void Train()
    {
        for (int i = 0; i < iterationCount; i++)
        {
            foreach (var target in targets)
            {
                var distanceToTarget = Vector2.Distance(target.transform.position, unitToTrain.transform.position);

                if (unitToTrain.visionRange < distanceToTarget)
                {
                    continue;
                }

                var bestDecision = CalculateBestDecision(distanceToTarget, unitToTrain.visionRange, target.energy);
                var guess = unitToTrain.Train(target, unitToTrain.visionRange, bestDecision);

                GuessCorrection(ref guess);
                target.SetText(guess.ToString());
            }
        }
    }

    private float CalculateBestDecision(float distanceToTarget, float visionRange, float targetEnergy)
    {
        return distanceToTarget / (visionRange * targetEnergy);
    }

    private void GuessCorrection(ref float guess)
    {
        guess = 100 - ((int)(guess * 100));
    }

    private void FindTargets()
    {
        var result = Physics2D.OverlapCircleAll(transform.position, 1000);

        targets = new List<Unit>(result.Length);

        for (int i = 0; i < result.Length; i++)
        {
            if (result[i].gameObject == unitToTrain.gameObject)
                continue;

            var targetInfo = result[i].GetComponent<Unit>();

            targets.Add(targetInfo);
        }
    }
}