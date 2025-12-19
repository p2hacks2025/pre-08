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

        //条件を満たしたレーザーの色をチェック
        bool shouldBreak = false;
        
        for (int i = 0; i < conditions.Length; i++)
        {
            var con = conditions[i];
            if (con.isDetected && con.laser != null)
            {
                //ターゲットカラーと一致、またはWhiteの場合は壊れる
                if (con.laser.laserColor == targetColor || targetColor == LaserColorType.White)
                {
                    shouldBreak = true;
                    break;
                }
            }
        }

        if (shouldBreak)
        {
            BreakWall();
        }
    }

    private void BreakWall()
    {
        //全てのアクティブなレーザーを対向方向から出力
        for (int i = 0; i < conditions.Length; i++)
        {
            var con = conditions[i];
            if (con.laser != null && con.laser.isActive)
            {
                int oppositeIndex = (i + 2) % 4;
                
                if (conditions[oppositeIndex].laser != null)
                {
                    conditions[oppositeIndex].laser.SetColor(con.laser.laserColor);
                    conditions[oppositeIndex].laser.Fire();
                }
            }
        }

        //破壊エフェクトを生成
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        //オブジェクトを破壊
        Destroy(gameObject);
    }
}
