using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private float startYPos;
    private Camera cam;
    private Bounds targetBounds;
    private float targetRatio;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        startYPos = transform.position.y;
    }
    
    public void ShakeCamera()
    {
        cam.DOShakePosition(0.2f, 0.5f, 15, 90);
    }

    public void SetTargetBounds(Bounds bounds)
    {
        targetBounds = bounds;
    }

    public void SetCameraPositionAndSize(Bounds levelBounds)
    {
        Vector3 center = levelBounds.center;
        float boundsSize = levelBounds.size.x;
        LeanTween.move(gameObject, new Vector3(center.x + 0.5f, startYPos, center.z), 0.5f);
    }
}
