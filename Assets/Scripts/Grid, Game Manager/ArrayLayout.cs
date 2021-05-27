[System.Serializable]
public class ArrayLayout
{
    [System.Serializable]
    public struct RowData
    {
        public CellType[] row;
    }

    public RowData[] rows = new RowData[7];
}

public enum CellType
{
    N, W, B, P, G, S, Bo, T //neutral, white, black, player, goal, static, teleport
}
