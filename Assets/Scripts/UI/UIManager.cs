using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject restartLevelButton;
    [SerializeField] private RectTransform buttonStartPosition;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private RectTransform buttonEndPosition;
    [SerializeField] private TMP_Text greetText;

    private Color greetColor;

    [SerializeField] private string[] greetMessages;

    private int scaleTweenId;

    private void Awake()
    {
        greetColor = greetText.color;
        greetText.text = "";
    }

    public void TweenIn(int index) // 0 = restartLevel and 1 = nextlevel
    {
        GameObject button = index == 0 ? restartLevelButton : nextLevelButton;
        LeanTween.moveLocalY(button, buttonEndPosition.localPosition.y, .2f)
            .setEaseSpring();
    }

    public void TweenOut(int index)
    {
        GameObject button = index == 0 ? restartLevelButton : nextLevelButton;
        LeanTween.moveLocalY(button, buttonStartPosition.localPosition.y, 0.5f)
            .setEaseSpring();
    }

    public void DisplayGreetMessage()
    {
        greetText.color = greetColor;
        string message;
        if (greetMessages.Length <= 0)
        {
            message = "YOU WIN";
        } else
        {
            message = greetMessages[Random.Range(0, greetMessages.Length - 1)];
        }
        greetText.text = message;
        scaleTweenId = LeanTween.scale(greetText.gameObject, Vector3.one, 0.5f).setEaseSpring().id;
        LeanTween.rotateAroundLocal(greetText.gameObject, Vector3.forward, 360, 0.5f).setEaseSpring();
    }

    public void DisplayLevel(int level)
    {
        greetText.color = Color.white;
        greetText.text = ("Level " + (level + 1).ToString());
        scaleTweenId = LeanTween.scale(greetText.gameObject, Vector3.one, 0.5f).setEaseSpring().id;
        Vanish();
    }

    private void Vanish()
    {
        Invoke("HideGreetMessage", 2f);
    }

    public void HideGreetMessage()
    {
        LeanTween.cancel(scaleTweenId, false);
        greetText.transform.localScale = Vector3.zero;
        greetText.transform.localRotation = Quaternion.Euler(0,0,0);
    }
}
