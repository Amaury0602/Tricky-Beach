using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private float startYPos;
    private Camera cam;
    private Bounds targetBounds;
    private float targetRatio;

    [SerializeField] private CamMode mode;

    private void Awake()
    {
        mode = CamMode.Game;
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
        LeanTween.move(gameObject, new Vector3(center.x, startYPos, center.z), 0.5f);

        float boundsDiag = Mathf.Sqrt(Mathf.Pow(levelBounds.size.y, 2) + Mathf.Pow(levelBounds.size.x, 2));
        float newOrthSize = mode == CamMode.Game ? boundsDiag * 0.95f : boundsDiag * 0.7f;
        if (mode == CamMode.Game)
        {
            cam.DOOrthoSize(Mathf.Clamp(newOrthSize, 12.5f, 17.5f), 0.2f);
        }
        else
        {
            cam.DOOrthoSize(Mathf.Clamp(newOrthSize, 10f, 12f), 0.2f);
        }
    }

    private enum CamMode
    {
        Video, Game
    }
}
