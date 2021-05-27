using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    public GameObject GetObject(GameObject gO)
    {
        if (pool.TryGetValue(gO.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
            {
                return CreateNewObject(gO);
            } else
            {
                GameObject objectInQueue = objectList.Dequeue();
                objectInQueue.SetActive(true);
                return objectInQueue;
            }
        } else
        {
            return CreateNewObject(gO);
        }
    }

    private GameObject CreateNewObject(GameObject gO)
    {
        GameObject newGO = Instantiate(gO);
        newGO.name = gO.name;
        return newGO;
    }

    private IEnumerator ReturnGameObject(GameObject gO, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if (pool.TryGetValue(gO.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gO);
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            newQueue.Enqueue(gO);
            pool.Add(gO.name, newQueue);
        }

        gO.SetActive(false);
    }

    public void ReturnGameObject(GameObject gO)
    {
        if (pool.TryGetValue(gO.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gO);
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            newQueue.Enqueue(gO);
            pool.Add(gO.name, newQueue);
        }

        gO.SetActive(false);
    }

    public void ReturnGameObjectWithDelay(GameObject gO, float delay = 0f)
    {
        StartCoroutine(ReturnGameObject(gO, delay));
    }
}
