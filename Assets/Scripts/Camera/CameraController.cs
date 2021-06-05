using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private float startYPos;
    private Camera cam;
    private Bounds targetBounds;
    private float targetRatio;

    private float minCamDistance = 12.5f;
    private float maxCamDistance = 17.5f;

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
        LeanTween.move(gameObject, new Vector3(center.x + 0.5f, startYPos, center.z), 0.5f);

        float boundsHeight = levelBounds.size.y;
        float boundsWidth = levelBounds.size.x;

        float camDistance;

        // y = y1 + ((x – x1) / (x2 – x1)) * (y2 – y1)
        // camHeight = minCam + ((14-10) / ()) * (17,5 - 12,5)

        if (boundsWidth <= 10)
        {
            camDistance = minCamDistance;
        }
        else if (boundsWidth >= 14.5f)
        {
            camDistance = maxCamDistance;
        } else
        {
            camDistance = 15f;
            //camDistance = minCamDistance + ((14-10) / (boundsWidth - 10)) * (maxCamDistance - minCamDistance);
        }
        cam.DOOrthoSize(camDistance, 0.5f);
    }
}
