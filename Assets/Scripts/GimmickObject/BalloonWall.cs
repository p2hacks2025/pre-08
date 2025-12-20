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
            //全てのアクティブなレーザーを対向方向に再発射
            for (int i = 0; i < conditions.Length; i++)
            {
                if (conditions[i].laser != null && conditions[i].laser.isActive)
                {
                    LaserPointer laserPointer = conditions[i].laser.GetLaserPointer();
                    if (laserPointer != null)
                    {
                        //対向方向のインデックスを計算
                        int oppositeIndex = (i + 2) % 4;
                        
                        if (conditions[oppositeIndex].laser != null)
                        {
                            //対向方向に位置と向きを設定
                            laserPointer.transform.position = conditions[oppositeIndex].laser.transform.position;
                            laserPointer.transform.rotation = conditions[oppositeIndex].laser.transform.rotation;
                            
                            //再発射
                            laserPointer.Fire();
                            
                            //対向側のFireLaserをアクティブに
                            conditions[oppositeIndex].laser.isActive = true;
                        }
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
