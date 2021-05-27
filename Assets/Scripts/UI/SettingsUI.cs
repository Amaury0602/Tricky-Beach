using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    private bool isDisplayed = false;
    private bool isMuted;
    private bool canVibrate;
    [SerializeField] private GameObject cog;

    [SerializeField] private GameObject[] settingButtons; // add additional buttons here
    [SerializeField] private Image[] buttonImages; //0 = sound and 1 = vibrations

    [SerializeField] private Sprite[] speakers; // 0 = mute and 1 = sound
    [SerializeField] private Sprite[] vibrations; // 0 = off and 1 = on

    private Vector2 initialPosition;

    private void Awake()
    {
        initialPosition = settingButtons[0].transform.localPosition;
        if (PlayerPrefs.HasKey("Sound"))
        {
            isMuted = PlayerPrefs.GetInt("Sound") == 1;
            MuteAudio();
        }

        if (PlayerPrefs.HasKey("Vibrate"))
        {
            canVibrate = PlayerPrefs.GetInt("Vibrate") == 0;
            SetVibration();
        } else
        {
            canVibrate = false;
            SetVibration();
        }
    }

    public void DisplaySettings()
    {
        int offset = 0;
        isDisplayed = !isDisplayed;
        LeanTween.rotateAroundLocal(cog, Vector3.forward, 45, 0.2f).setEaseOutQuad();
        for (int i = 0; i < settingButtons.Length; i++)
        {
            offset += 120;
            if (isDisplayed)
            {
                LeanTween.moveLocalY(settingButtons[i], initialPosition.y - offset, 0.2f).setEaseOutQuad();
            }
            else
            {
                LeanTween.moveLocalY(settingButtons[i], initialPosition.y, 0.2f).setEaseOutQuad(); 
            }
        }
    }

    public void MuteAudio()
    {
        isMuted = !isMuted;
        buttonImages[0].sprite = isMuted ? speakers[0] : speakers[1];
        AudioListener.volume = isMuted ? 0 : 1;
        PlayerPrefs.SetInt("Sound", isMuted ? 0 : 1);
    }

    public void Vibrate()
    {
        if (!canVibrate) return;
        Vibration.Vibrate(50);
    }

    public void BigVibrate()
    {
        if (!canVibrate) return;
        Vibration.Vibrate(100);
    }

    public void SetVibration()
    {
        canVibrate = !canVibrate;
        buttonImages[1].sprite = canVibrate ? vibrations[1] : vibrations[0];
        PlayerPrefs.SetInt("Vibrate", canVibrate ? 1 : 0);
    }
}
