using UnityEngine;

public class BalloonWall : ResponseLaser
{
    [SerializeField] private Renderer lightRenderer;
    [SerializeField] private GameObject breakEffect;        //破壊エフェクト
    [SerializeField] private LaserColorType targetColor;    //壊れる色

    void Start()
    {
        if (lightRenderer != null)
        {
            ApplyMaterial(lightRenderer, targetColor);
        }
    }

    protected override void OutputLaser()
    {
        if (conditions == null || conditions.Length != 4) return;

        //指定色のレーザーが当たっているか確認
        bool shouldBreak = false;
        foreach (var condition in conditions)
        {
            if (condition.laser != null && condition.laser.isActive && condition.laser.laserColor == targetColor)
            {
                shouldBreak = true;
                break;
            }
        }

        if (shouldBreak)
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (conditions[i].laser != null && conditions[i].laser.isActive)
                {
                    //対向方向のインデックスを計算 (0↔2, 1↔3)
                    int oppositeIndex = (i + 2) % 4;
                    
                    if (conditions[oppositeIndex].laser != null)
                    {
                        //入力レーザーの色を対向方向に適用
                        conditions[oppositeIndex].laser.SetColor(conditions[i].laser.laserColor);
                        
                        //反対方向のレーザーから出力
                        conditions[oppositeIndex].laser.Fire();
                        
                        //対向側のFireLaserをアクティブに
                        conditions[oppositeIndex].laser.isActive = true;
                    }
                }
            }

            //破壊エフェクトを生成
            if (breakEffect != null)
            {
                Instantiate(breakEffect, transform.position, transform.rotation);
            }

            //壁を壊す処理
            Destroy(gameObject);
        }
    }
}
