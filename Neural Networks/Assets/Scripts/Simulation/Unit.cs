using TMPro;
using UnityEngine;

[System.Serializable]
public struct UnitData
{
    public float maxEnergy;
    public float currentEnergy;

    public float maxSpeed;

    public float power;
    public float maxPower;

    public float digestion;

    public float energyDecreasePerAction;
}

public class Unit : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TextMeshPro data;

    //public UnitData unitData;

    private Perceptron brain;

    public float visionRange;
    public float energy;

    private void Update()
    {
        SetColor();
    }

    public void SetBrain(Perceptron brain)
    {
        this.brain = brain;
    }

    public float Train(Unit target, float visionRange, float bestDecision)
    {
        var inputs = new float[brain.Size];
        inputs[0] = Vector2.Distance(transform.position, target.transform.position);
        inputs[1] = visionRange;
        inputs[2] = target.energy;
        inputs[2] = 1;

        brain.Train(inputs, bestDecision);

        return brain.Predict(inputs);
    }

    public void SetText(string text)
    {
        data.text = text;
    }

    private void SetColor()
    {
        //spriteRenderer.color = Color.Lerp(Color.green, Color.red, unitData.power / unitData.maxPower);
    }
}