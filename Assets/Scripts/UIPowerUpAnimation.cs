using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPowerUpAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    public float moveDuration = 1f;
    public float shakeDuration = 0.3f;
    public Vector3 shakeStrength = new Vector3(5f, 5f, 0f);

    private Camera uiCamera;

    private Canvas parentCanvas;

    private bool gameStarted = false;

    private void OnEnable()
    {
        if (gameStarted)
        {
            AnimatePowerUpCollection();
        }
    }

    private void Start()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        if (parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            uiCamera = parentCanvas.worldCamera;
        else
            uiCamera = Camera.main;

        gameObject.SetActive(false);
        gameStarted = true;
    }

    public void AnimatePowerUpCollection()
    {
        StartCoroutine(PlayCollectionAnimation());
    }

    private IEnumerator PlayCollectionAnimation()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;

        yield return StartCoroutine(PlayCollectionAnimation(player.transform.position));
    }

    private IEnumerator PlayCollectionAnimation(Vector3 playerWorldPosition)
    {
        Image powerUpImage = GetComponent<Image>();
        if (powerUpImage == null) yield break;

        gameObject.SetActive(true);

        Vector3 originalPosition = transform.position;

        Vector3 playerScreenPosition = GetUIPositionFromWorldPosition(playerWorldPosition);

        transform.position = playerScreenPosition;

        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 0.3f;

        Color originalColor = powerUpImage.color;
        Color startColor = originalColor;
        startColor.a = 0.8f;
        powerUpImage.color = startColor;

        Sequence moveSequence = DOTween.Sequence();

        moveSequence.Append(transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack));
        moveSequence.Join(powerUpImage.DOColor(originalColor, 0.3f));

        moveSequence.Append(transform.DOMove(originalPosition, moveDuration).SetEase(Ease.OutBack));

        yield return moveSequence.WaitForCompletion();

        yield return StartCoroutine(PlayArrivalEffect());
    }

    private IEnumerator PlayArrivalEffect()
    {
        Sequence arrivalSequence = DOTween.Sequence();

        arrivalSequence.Append(transform.DOShakePosition(shakeDuration, shakeStrength, 10, 90f).SetEase(Ease.OutCubic));

        arrivalSequence.Join(transform.DOPunchScale(Vector3.one * 0.1f, shakeDuration, 3, 0.5f));

        yield return arrivalSequence.WaitForCompletion();

        transform.DOMove(transform.position, 0.1f);
    }

    private Vector3 GetUIPositionFromWorldPosition(Vector3 worldPosition)
    {
        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            return screenPosition;
        }
        else if (parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 screenPosition = uiCamera.WorldToScreenPoint(worldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                screenPosition,
                uiCamera,
                out Vector2 localPoint
            );

            return parentCanvas.transform.TransformPoint(localPoint);
        }
        else
        {
            return worldPosition;
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}