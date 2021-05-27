using UnityEngine;

[CreateAssetMenu(fileName = "AllGameLevels", menuName = "SO/Levels")]
public class GameLevels : ScriptableObject
{
    public LevelLayout[] Levels;
}
