using UnityEngine;

[System.Serializable]
public class MoonData
{
    public LaserColorType colorType;
    public GameObject particlePrefab;
}
public class MoonMirror : ResponseLaser
{
    [SerializeField] private MoonData[] datas;  //各色衝突パーティクルデータ
    
    protected override void OutputLaser()
    {
        if (conditions == null || conditions.Length != 2) return;
        
        //どちらのレーザーがアクティブか判定して、反対側から出力
        for (int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i].laser != null && conditions[i].laser.isActive)
            {
                //衝突パーティクルを生成
                LaserColorType inputColor = conditions[i].laser.laserColor;
                foreach (var data in datas)
                {
                    if (data.colorType == inputColor && data.particlePrefab != null)
                    {
                        Instantiate(data.particlePrefab, transform.position, transform.rotation);
                        break;
                    }
                }
                
                //反対側のレーザーを取得して出力
                int oppositeIndex = (i + 1) % conditions.Length;
                var outputLaser = conditions[oppositeIndex].laser;
                
                if (outputLaser != null)
                {
                    outputLaser.SetColor(inputColor);
                    outputLaser.Fire();
                    return;
                }
            }
        }
    }
}
