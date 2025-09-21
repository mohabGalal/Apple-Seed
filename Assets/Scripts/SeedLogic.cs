using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SeedLogic : MonoBehaviour
{
    [Header("Tree progression")]
    public List<GameObject> trees;

    public GameObject WinScreen;

    [Header("Platform")]
    public MovingPlatform movingPlatform;

    [Header("Seed Images (UI)")]
    public List<Image> seedImages;

    [Header("Animation Settings")]
    public float seedUIAnimationDuration = 0.8f;
    public float treeGrowDuration = 1.2f;
    public float seedCollectionEffectDuration = 0.5f;

    public static int seedsCollected = 0;

    private bool isTutorialScene;
    PlayerMovement player;

    public GameObject DefaultTree;

    public MP_Trigger MP_trigger;


    private void Awake()
    {
        isTutorialScene = SceneManager.GetActiveScene().name == "TutorialScene";
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = collision.collider.GetComponent<PlayerMovement>();
            StopAllCoroutines();
            StartCoroutine(HandleSeedCollection(collision));
            Debug.Log("seed collides with player", collision.gameObject);
        }
    }

    private IEnumerator HandleSeedCollection(Collision2D collision)
    {
        GetComponent<Collider2D>().enabled = false;

        GetComponent<Animator>().enabled = false;

        yield return StartCoroutine(PlaySeedCollectionEffect());

        ++seedsCollected;

        Debug.Log($"Seed count {seedsCollected}, in scene : {SceneManager.GetActiveScene().name}");

        if(DefaultTree)
            DefaultTree.SetActive(false);


        if (seedsCollected % 2 == 0 && movingPlatform != null)
        {
            MovingPlatform script = movingPlatform.GetComponent<MovingPlatform>();
            script.ChangeTarget();
           /* script.transform.position = script.startPoint.position;
            script.isPlayerOn = false;*/
            
            
        }

        
        if (player != null)
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            player.SeedCollected();
        }

        Debug.Log($"Seeds collected: {seedsCollected}");

        yield return StartCoroutine(AnimateSeedUI());

        yield return StartCoroutine(SetTreeActiveWithAnimation());

        Destroy(gameObject);
    }

    private IEnumerator PlaySeedCollectionEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Transform seedTransform = transform;

        Vector3 originalScale = seedTransform.localScale;
        Color originalColor = spriteRenderer.color;

        Sequence collectSequence = DOTween.Sequence();

        collectSequence.Append(seedTransform.DOScale(originalScale * 1.3f, 0.15f).SetEase(Ease.OutBack));
        collectSequence.Append(seedTransform.DOScale(originalScale * 0.3f, 0.2f).SetEase(Ease.InBack));

        collectSequence.Join(seedTransform.DORotate(new Vector3(0, 0, 360), 0.35f, RotateMode.LocalAxisAdd).SetEase(Ease.OutCubic));
        collectSequence.Join(spriteRenderer.DOFade(0f, 0.35f).SetEase(Ease.InCubic));

        yield return collectSequence.WaitForCompletion();
    }

    private IEnumerator AnimateSeedUI()
    {
        if (seedsCollected - 1 < seedImages.Count)
        {
            Image targetSeedImage = seedImages[seedsCollected - 1];

            Vector3 originalScale = targetSeedImage.transform.localScale;
            Color originalColor = targetSeedImage.color;

            Color tempColor = originalColor;
            tempColor.a = 0f;
            targetSeedImage.color = tempColor;

            targetSeedImage.transform.localScale = Vector3.zero;

            Sequence uiSequence = DOTween.Sequence();

            uiSequence.Append(targetSeedImage.transform.DOScale(originalScale * 1.2f, seedUIAnimationDuration * 0.6f).SetEase(Ease.OutBack));
            uiSequence.Join(targetSeedImage.DOFade(1f, seedUIAnimationDuration * 0.6f).SetEase(Ease.OutCubic));

            uiSequence.Append(targetSeedImage.transform.DOScale(originalScale, seedUIAnimationDuration * 0.4f).SetEase(Ease.InOutSine));

            uiSequence.AppendCallback(() => {
                targetSeedImage.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 2, 0.5f);
            });

            yield return uiSequence.WaitForCompletion();
        }
    }

    private IEnumerator SetTreeActiveWithAnimation()
    {
        Debug.Log("Seed collected, changing tree...");

        yield return new WaitForSeconds(0.3f);

        foreach (var tree in trees)
        {
            tree.SetActive(false);
        }

        if (seedsCollected - 1 < trees.Count)
        {
            GameObject currentTree = trees[seedsCollected - 1];
            currentTree.SetActive(true);
            yield return StartCoroutine(TreeGrowthEffect_Fade(currentTree));
        }

        if (seedsCollected >= seedImages.Count)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(ShowWinScreenWithAnimation());
            //SoundManager.Instance.PlayWinScreen();
            if(SceneManager.GetActiveScene().name == "MainLevel")
            {
                SoundManager.Instance.MainTheme.Stop();
            }

        }
    }

    private IEnumerator TreeGrowthEffect_Fade(GameObject tree)
    {
        Vector3 originalScale = tree.transform.localScale;

        SpriteRenderer[] renderers = tree.GetComponentsInChildren<SpriteRenderer>();

        foreach (var renderer in renderers)
        {
            Color c = renderer.color;
            c.a = 0f;
            renderer.color = c;
        }

        tree.transform.localScale = originalScale * 0.8f;

        Sequence fadeSequence = DOTween.Sequence();

        foreach (var renderer in renderers)
        {
            fadeSequence.Join(renderer.DOFade(1f, treeGrowDuration * 0.8f).SetEase(Ease.OutCubic));
        }
        fadeSequence.Join(tree.transform.DOScale(originalScale, treeGrowDuration).SetEase(Ease.OutBack));

        yield return fadeSequence.WaitForCompletion();
    }

    private IEnumerator ShowWinScreenWithAnimation()
    {
        WinScreen.SetActive(true);

        CanvasGroup winCanvasGroup = WinScreen.GetComponent<CanvasGroup>();
        if (winCanvasGroup == null)
            winCanvasGroup = WinScreen.AddComponent<CanvasGroup>();

        winCanvasGroup.alpha = 0f;
        WinScreen.transform.localScale = Vector3.zero;

        Sequence winSequence = DOTween.Sequence();

        winSequence.Append(WinScreen.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack));
        winSequence.Join(winCanvasGroup.DOFade(1f, 0.8f).SetEase(Ease.OutCubic));

        yield return winSequence.WaitForCompletion();
    }

    public static void ResetSeedCount()
    {
        seedsCollected = 0;
        Debug.Log($"seeds reset {seedsCollected}");
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}