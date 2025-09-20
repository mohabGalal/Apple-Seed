using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    //[SerializeField] private Transform cameraTransform;
    //[SerializeField] private float parallaxEffectMultiplier = 0.5f;

    //private Vector3 lastCameraPosition;
    //private float textureUnitSizeX;

    //private void Start()
    //{
    //    if (cameraTransform == null)
    //        cameraTransform = Camera.main.transform;
    //    lastCameraPosition = cameraTransform.position;

    //    Sprite sprite = GetComponent<SpriteRenderer>().sprite;
    //    Texture2D texture = sprite.texture;
    //    textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
    //}

    //private void LateUpdate()
    //{
    //    Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
    //    transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0f, 0f);
    //    lastCameraPosition = cameraTransform.position;

    //    float distance = cameraTransform.position.x - transform.position.x;

    //    if (distance >= textureUnitSizeX)
    //    {
    //        transform.position += new Vector3(textureUnitSizeX, 0f, 0f);
    //    }
    //    else if (distance <= -textureUnitSizeX)
    //    {
    //        transform.position -= new Vector3(textureUnitSizeX, 0f, 0f);
    //    }
    //}
    [Header("Parallax Settings")]
    [Range(0f, 1f)]
    public float parallaxSpeedX = 0.5f;
    [Range(0f, 1f)]
    public float parallaxSpeedY = 0.5f;

    [Header("References")]
    public Transform cameraTransform;

    private Vector3 lastCameraPosition;

    void Start()
    {

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 cameraMovement = cameraTransform.position - lastCameraPosition;

        float deltaX = cameraMovement.x * parallaxSpeedX;
        float deltaY = cameraMovement.y * parallaxSpeedY;

        Vector3 newPosition = transform.position + new Vector3(deltaX, deltaY, 0);

        transform.position = newPosition;
        lastCameraPosition = cameraTransform.position;
    }
}
