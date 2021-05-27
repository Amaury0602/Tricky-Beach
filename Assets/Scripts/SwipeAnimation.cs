using UnityEngine;
using DG.Tweening;

public class SwipeAnimation : MonoBehaviour
{
    private Vector2 initialPosition;
    private Tween moveLeft = null;
    private Tween moveUp = null;
    private Tween moveDown = null;
    private Tween moveRight = null;

    private float offset = 200f;
    private float animSpeed = 0.5f;

    private Sequence seq;

    void Start()
    {
        initialPosition = transform.position;
        seq = DOTween.Sequence();
        PlayTween();
    }

    private void PlayTween()
    {
        moveRight = transform.DOMoveX(initialPosition.x + offset, animSpeed).SetEase(Ease.OutExpo).SetDelay(1f)
            .OnComplete(() => transform.position = initialPosition);

        moveDown = transform.DOMoveY(initialPosition.y - offset, animSpeed).SetEase(Ease.OutExpo).SetDelay(1f)
            .OnComplete(() => transform.position = initialPosition);

        moveLeft = transform.DOMoveX(initialPosition.x - offset, animSpeed).SetEase(Ease.OutExpo).SetDelay(1f)
            .OnComplete(() => transform.position = initialPosition);

        moveUp = transform.DOMoveY(initialPosition.y + offset, animSpeed).SetEase(Ease.OutExpo).SetDelay(1f)
            .OnComplete(() => transform.position = initialPosition);

        seq
            .Append(moveRight)
            .Append(moveDown)
            .Append(moveLeft)
            .Append(moveUp)
            .SetLoops(-1);
    }

    private void OnDisable()
    {
        seq.TogglePause();
    }
}
