using UnityEngine;

public class ParallaxLoop : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxEffectMultiplier = 0.5f;

    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit * transform.localScale.x;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0f, 0f);
        lastCameraPosition = cameraTransform.position;

        float distance = cameraTransform.position.x - transform.position.x;

        if (distance >= textureUnitSizeX)
        {
            transform.position += new Vector3(textureUnitSizeX, 0f, 0f);
        }
        else if (distance <= -textureUnitSizeX)
        {
            transform.position -= new Vector3(textureUnitSizeX, 0f, 0f);
        }
    }
}
