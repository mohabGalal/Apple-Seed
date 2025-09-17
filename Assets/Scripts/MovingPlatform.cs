using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint;
    public List<Transform> endPoints = new List<Transform>();
    public float speed = 4f;
    public float pauseDuration = 1f;

    private Transform currentTarget;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private List<int> availableTargets;
    private bool isPaused = false;

    private const string playerTag = "Player";

    void Start()
    {
        if (startPoint == null)
        {
            enabled = false;
            return;
        }

        if (endPoints.Count == 0)
        {
            enabled = false;
            return;
        }

        InitializeAvailableTargets();
        SetNewRandomTarget();
        startPosition = startPoint.position;
    }

    void Update()
    {
        if (isPaused) return;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            StartCoroutine(PauseAndReverse());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    [ContextMenu("Change Target")]
    //Call this method to change the target point of the platform (In SeedLogic FindFirstObjectByType<MovingPlatform>().ChangeTarget();)
    public void ChangeTarget()
    {
        if (availableTargets.Count > 0)
        {
            SetNewRandomTarget();
            isPaused = false;
        }
        else
        {
            Debug.Log("All end points have been used Platform will continue moving between the current points");
        }
    }

    private void InitializeAvailableTargets()
    {
        availableTargets = new List<int>();
        for (int i = 0; i < endPoints.Count; i++)
        {
            availableTargets.Add(i);
        }
    }

    private void SetNewRandomTarget()
    {
        if (availableTargets.Count == 0) return;

        int randomAvailableIndex = Random.Range(0, availableTargets.Count);
        int targetIndex = availableTargets[randomAvailableIndex];

        currentTarget = endPoints[targetIndex];
        targetPosition = currentTarget.position;

        availableTargets.RemoveAt(randomAvailableIndex);
    }

    IEnumerator PauseAndReverse()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);

        Vector2 temp = startPosition;
        startPosition = startPoint.position;
        targetPosition = temp;

        isPaused = false;
    }
}