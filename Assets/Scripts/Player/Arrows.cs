using UnityEngine;
using DG.Tweening;

public class Arrows : MonoBehaviour
{
    private Transform[] allArrows = new Transform[2];
    private Vector3 initialScale;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            allArrows[i] = transform.GetChild(i).GetComponent<Transform>();
        }
        initialScale = allArrows[0].transform.localScale;
    }

    public void HideAllArrows()
    {
        foreach (var arrow in allArrows)
        {
            arrow.localScale = Vector3.zero;
        }
    }

    public void DisplayArrows(int playerOrientation) 
    {
        foreach (var arrow in allArrows)
        {
            arrow.LeanScale(initialScale, 0.3f).setEaseInOutBack();
        }
    }
}
