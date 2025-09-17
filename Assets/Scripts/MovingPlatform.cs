using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint;
    public List<Transform> endPoints = new List<Transform>();
<<<<<<< HEAD
    public float speed = 2.5f;
    public float pauseDuration = 1f;

    private Transform currentTarget;
    private Vector2 currentTargetPosition;
    private List<int> availableTargets;
    private bool isMovingToStart = false;
=======
    public float speed = 4f;
    public float pauseDuration = 1f;

    private Transform currentTarget;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private List<int> availableTargets;
>>>>>>> 42f757d60ec214489715faa24452a67a957a151c
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
<<<<<<< HEAD
=======
        startPosition = startPoint.position;
>>>>>>> 42f757d60ec214489715faa24452a67a957a151c
    }

    void Update()
    {
        if (isPaused) return;

<<<<<<< HEAD
        Vector2 destination = isMovingToStart ? startPoint.position : currentTargetPosition;
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, destination) < 0.01f)
        {
            StartCoroutine(PauseAndDecideNextMove());
=======
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            StartCoroutine(PauseAndReverse());
>>>>>>> 42f757d60ec214489715faa24452a67a957a151c
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
<<<<<<< HEAD
=======
    //Call this method to change the target point of the platform (In SeedLogic FindFirstObjectByType<MovingPlatform>().ChangeTarget();)
>>>>>>> 42f757d60ec214489715faa24452a67a957a151c
    public void ChangeTarget()
    {
        if (availableTargets.Count > 0)
        {
            SetNewRandomTarget();
<<<<<<< HEAD
            isMovingToStart = false;
        }
        else
        {
            Debug.Log("All end points have been used. Platform will continue moving between the current points.");
=======
            isPaused = false;
        }
        else
        {
            Debug.Log("All end points have been used Platform will continue moving between the current points");
>>>>>>> 42f757d60ec214489715faa24452a67a957a151c
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
<<<<<<< HEAD
        currentTargetPosition = currentTarget.position;
        availableTargets.RemoveAt(randomAvailableIndex);
    }

    IEnumerator PauseAndDecideNextMove()
=======
        targetPosition = currentTarget.position;

        availableTargets.RemoveAt(randomAvailableIndex);
    }

    IEnumerator PauseAndReverse()
>>>>>>> 42f757d60ec214489715faa24452a67a957a151c
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);

<<<<<<< HEAD
        isMovingToStart = !isMovingToStart;
=======
        Vector2 temp = startPosition;
        startPosition = startPoint.position;
        targetPosition = temp;
>>>>>>> 42f757d60ec214489715faa24452a67a957a151c

        isPaused = false;
    }
}