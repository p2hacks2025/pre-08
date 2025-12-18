using UnityEngine;

// レーザーの色の種類
public enum LaserColorType
{
    Red,
    Orange,
    Yellow,
    Green,
    Skyblue,
    Blue,
    Purple,
    White
}

// 色とマテリアルの管理クラス
[System.Serializable]
public class LaserColorData
{
    public LaserColorType colorType;
    public Material material;
    public Material materialColor;
    public GameObject collisionParticle;
}

public class LaserColor : MonoBehaviour
{
    [SerializeField] private LaserColorData[] colorDataArray;

    //インスタンス（シングルトン）
    public static LaserColor Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Material GetMaterial(LaserColorType colorType)
    {
        //ColorTypeからマテリアルを取得する
        foreach (var data in colorDataArray)
        {
            if (data.colorType == colorType)
            {
                return data.material;
            }
        }
        return null;
    }
    public GameObject GetCollisionParticle(LaserColorType colorType)
    {
        //ColorTypeから衝突パーティクルを取得する
        foreach (var data in colorDataArray)
        {
            if (data.colorType == colorType)
            {
                return data.collisionParticle;
            }
        }
        return null;
    }
}
