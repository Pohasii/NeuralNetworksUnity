using UnityEngine;

public class SimulationInitializator : MonoBehaviour
{
    public Unit unitPrefab;
    public int count;
    public Vector2 bounds;

    public UnitData initialStats;

    public float radius;

    private float angle;

    private void Awake()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < count; i++)
        {
            angle += 2 * Mathf.PI / count;

            var x = Mathf.Cos(angle) * radius;
            var y = Mathf.Sin(angle) * radius;

            var position = new Vector3(x, y, 0);

            var unit = Instantiate(unitPrefab, position, Quaternion.identity);

            //var x = Random.Range(-bounds.x, bounds.x);
            //var y = Random.Range(-bounds.y, bounds.y);

            //var position = new Vector3(x, y, 0);

            //var unit = Instantiate(unitPrefab, position, Quaternion.identity);
            //unit.unitData = initialStats;
        }
    }
}