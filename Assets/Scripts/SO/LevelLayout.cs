using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelLayout", menuName = "SO/LevelLayout")]
public class LevelLayout : ScriptableObject
{
    public ArrayLayout level;
    public CellProperty spawnCellProperty = CellProperty.Free;
}
