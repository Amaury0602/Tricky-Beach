using UnityEngine;

[CreateAssetMenu(fileName = "New VFX", menuName = "SO/VFX")]
public class VFXAsset : ScriptableObject
{
    public GameObject effect;
    public AudioClip sound;
    public bool limitedDuration = true;
    
    public void Play(ObjectPool pool, AudioSource source, Vector3 position, float delay = 0.5f)
    {
        if (effect != null)
        {
            GameObject newEffect = pool.GetObject(effect);
            newEffect.transform.position = position;
            if (limitedDuration) pool.ReturnGameObjectWithDelay(newEffect, delay);
        }

        if (sound != null)
        {
            source.clip = sound;
            source.Play();
        }
        
    }
}
