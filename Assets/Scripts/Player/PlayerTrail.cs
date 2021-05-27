using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    private ParticleSystem trail;

    private void Start()
    {
        trail = GetComponent<ParticleSystem>();
    }

    public void PlaySlideTrail(bool play)
    {
        if (play)
        {
            trail.Play();
        } else
        {
            trail.Stop();
        }
    }
}
