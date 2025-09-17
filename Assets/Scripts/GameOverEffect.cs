using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverEffect : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float uiAnimDuration = 0.8f;

    [Header("Cinemachine Impulse")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float impulseForce = 1f;

    private CanvasGroup cg;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShowLossScreenWithAnimation());

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulseWithForce(impulseForce);
        }
    }

    private IEnumerator ShowLossScreenWithAnimation()
    {
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        transform.localScale = Vector3.zero;

        Sequence lossSequence = DOTween.Sequence();
        lossSequence.Append(transform.DOScale(Vector3.one, uiAnimDuration).SetEase(Ease.OutBack));
        lossSequence.Join(cg.DOFade(1f, uiAnimDuration).SetEase(Ease.OutCubic));

        yield return lossSequence.WaitForCompletion();
    }
}
