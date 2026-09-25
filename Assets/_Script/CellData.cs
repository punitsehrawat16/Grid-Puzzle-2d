using UnityEngine;

public enum CellType
{
    Empty,
    Orange,
    Green,
    Yellow,
    Blue
}

public struct CellData
{
    public CellType Type;

    public CellData(CellType type)
    {
        Type = type;
    }
}