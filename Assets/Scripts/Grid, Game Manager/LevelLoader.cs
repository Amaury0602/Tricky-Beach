using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    private GridSystem grid;
    [SerializeField] private GameLevels levelData;
    [SerializeField] private IntEvent onLoadNewLevel;
    private int levelCount;
    private bool firstTimePlaying = true;

    private void Start()
    {
        grid = GetComponent<GridSystem>();
        levelCount = levelData.Levels.Length;
        if (!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") - 1);
        }
        else
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") -1);
            //PlayerPrefs.SetInt("Level", -1);
        }
        LoadNewLevel();
        if (firstTimePlaying == true) firstTimePlaying = false;
    }

    public void LoadNewLevel()
    {
        if (!firstTimePlaying)
        {
            TinySauce.OnGameFinished(PlayerPrefs.GetInt("Level")); // just finish level "Level"
        }
        if (PlayerPrefs.GetInt("Level") >= levelCount - 1)
        {
            if (levelCount > 10)
            {
                PlayerPrefs.SetInt("Level", 20);
            } else
            {
                PlayerPrefs.SetInt("Level", -1);
            }
        }
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        onLoadNewLevel.Raise(PlayerPrefs.GetInt("Level"));
        grid.BuildLevel(levelData.Levels[PlayerPrefs.GetInt("Level")]);

        TinySauce.OnGameStarted(PlayerPrefs.GetInt("Level").ToString());
    }

    public void ResetLevel()
    {
        onLoadNewLevel.Raise(PlayerPrefs.GetInt("Level"));
        grid.BuildLevel(levelData.Levels[PlayerPrefs.GetInt("Level")]);
    }
}
