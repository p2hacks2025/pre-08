using UnityEngine;

[System.Serializable]
public class CollisionData
{
    public LaserColorType colorType;
    public GameObject particlePrefab;
}
public class MoonMirror : DirectionChanger
{
    [SerializeField] private CollisionData[] datas; //各色衝突パーティクルデータ
    
    protected override void DetectLaser()
    {
        foreach (var con in conditions)
        {
            if (con.laser == null || con.isDetected) continue;
            
            //レーザーがアクティブな場合
            if (con.laser.isActive)
            {
                //White色は「どの色でもOK」として扱う
                if (con.color == LaserColorType.White || con.laser.laserColor == con.color)
                {
                    con.isDetected = true;
                    //色が一致する衝突パーティクルを生成
                    foreach (var data in datas)
                    {
                        if (data.colorType == con.laser.laserColor && data.particlePrefab != null)
                        {
                            Instantiate(data.particlePrefab, transform.position, transform.rotation);
                            break;
                        }
                    }
                }
                else
                {
                    //色不一致
                    Debug.LogWarning($"{gameObject.name}: レーザー色が条件不一致 (入力: {con.laser.laserColor}, 必要: {con.color})");
                }
            }
        }
    }
}
