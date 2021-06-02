using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    private ObjectPool pool;

    private void Start()
    {
        pool = FindObjectOfType<ObjectPool>();
    }

    public void GoToPool()
    {
        pool.ReturnGameObject(gameObject);
    }
}
