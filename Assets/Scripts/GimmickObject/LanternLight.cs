using UnityEngine;

public class LanternLight : ResponseLaser
{
    [SerializeField] private Renderer[] lightRenderers;

    void Start()
    {
        if (conditions == null || lightRenderers == null) return;
        //初期色のマテリアルを設定
        for (int i = 0; i < conditions.Length && i < lightRenderers.Length; i++)
        {
            var con = conditions[i];
            if (con.laser != null && lightRenderers[i] != null)
            {
                ApplyMaterial(lightRenderers[i], con.color);
            }
        }
    }

    protected override void OutputLaser()
    {
        if (conditions == null || lightRenderers == null) return;
        
        //出力レーザー色に応じてライトのマテリアルを変更
        for (int i = 0; i < conditions.Length && i < lightRenderers.Length; i++)
        {
            if (conditions[i].laser != null　&& lightRenderers[i] != null)
            {
                ApplyMaterial(lightRenderers[i], conditions[i].laser.laserColor == LaserColorType.White ? conditions[i].color : conditions[i].laser.laserColor);
                conditions[i].laser.Fire();
            }
        }
    }
}
