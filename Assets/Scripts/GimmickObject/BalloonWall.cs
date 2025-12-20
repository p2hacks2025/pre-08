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
            //初期色のマテリアルを設定
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
                    //反対方向のレーザーを取得
                    int oppositeIndex = (i + 2) % 4;
                    var outputLaser = conditions[oppositeIndex].laser;
                    
                    if (outputLaser != null)
                    {
                        //入力と反対方向にあるレーザーを入力色で出力
                        outputLaser.SetColor(conditions[i].laser.laserColor);
                        outputLaser.Fire();
                        outputLaser.isActive = true;
                    }
                }
            }

            //エフェクトの生成
            if (breakEffect != null)
            {
                Instantiate(breakEffect, transform.position, transform.rotation);
            }

            //削除処理
            Destroy(gameObject);
        }
    }
}
