using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    private ObjectPool pool;
    private AudioSource source;
    [SerializeField] private VFXAsset effect;
    [SerializeField] private float effectDuration;

    void Start()
    {
        source = GetComponent<AudioSource>();
        pool = FindObjectOfType<ObjectPool>();
    }

    public void PlayEffect(Vector3 position)
    {
        effect.Play(pool, source, position, effectDuration);
    }
}
