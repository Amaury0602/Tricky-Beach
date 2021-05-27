using UnityEngine;

public class FallDetection : MonoBehaviour
{
    private PlayerInput player;
    [SerializeField] private Transform[] detectors = new Transform[2];
    [SerializeField] private LayerMask groundLayer;
    private bool isDetecting;
    private bool isFalling;


    void Start()
    {
        player = GetComponent<PlayerInput>();
        isDetecting = false;
        isFalling = false;
    }

    void LateUpdate()
    {
        if (!isDetecting) return;

        bool detectorOneTriggered = Physics.Raycast(detectors[0].position, Vector3.down, 5f, groundLayer);
        bool detectorTwoTriggered = Physics.Raycast(detectors[1].position, Vector3.down, 5f, groundLayer);
        if (!detectorOneTriggered && !detectorTwoTriggered && !isFalling)
        {
            isFalling = true;
            StartCoroutine(player.Lose());
        }
    }

    public void StartDetecting(bool detect)
    {
        isDetecting = detect;
        isFalling = !detect;
    }
}
