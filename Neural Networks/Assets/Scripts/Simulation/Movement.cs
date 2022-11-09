using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 destination;

    private void Awake()
    {
        destination = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, destination) < 0.1f)
        {
            destination = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
        }
    }
}