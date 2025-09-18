using UnityEngine;

public class PatrolZone : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public Vector2 GetPatrolMinMax()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning($"PatrolZone on {gameObject.name} is missing points!");
            return Vector2.zero;
        }

        float minX = Mathf.Min(startPoint.position.x, endPoint.position.x);
        float maxX = Mathf.Max(startPoint.position.x, endPoint.position.x);
        return new Vector2(minX, maxX);
    }

    private void Awake()
    {
        // Auto-find child objects if not set in Inspector
        if (startPoint == null) startPoint = transform.Find("StartPoint");
        if (endPoint == null) endPoint = transform.Find("EndPoint");
    }
}
