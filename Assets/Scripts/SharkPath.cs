using UnityEngine;
using DG.Tweening;

public class SharkPath : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speed;
    private Vector3[] pathValues;
    private PathType pt = PathType.Linear;

    private void Start()
    {
        if (wayPoints.Length <= 0) return;
        pathValues = new Vector3[wayPoints.Length];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            pathValues[i] = wayPoints[i].position;
        }
        transform.DOPath(pathValues, speed, pt).SetLookAt(0.01f)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }
}
