using UnityEngine;

// レーザーの色の種類
public enum LaserColorType
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet
}

// 色とマテリアルの管理クラス
[System.Serializable]
public class LaserColorData
{
    public LaserColorType colorType;
    public Material material;
}
