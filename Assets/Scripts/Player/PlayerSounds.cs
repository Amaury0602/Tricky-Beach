using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource aSource;
    [SerializeField] private AudioClip[] footSteps;
    [SerializeField] private AudioClip slideSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landJumpSound;

    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        aSource.clip = footSteps[Random.Range(0, footSteps.Length)];
        aSource.Play();
    }

    public void PlaySlidingSound(bool play)
    {
        if (play)
        {
            aSource.clip = slideSound;
            aSource.loop = true;
            aSource.Play();
        } else
        {
            aSource.loop = false;
        }
    }
}
