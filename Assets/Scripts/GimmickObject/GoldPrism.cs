using UnityEngine;

public class GoldPrism : ResponseLaser
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
        if (conditions == null) return;

        //アクティブな入力を探す
        int inputIndex = -1;
        LaserColorType inputColor = LaserColorType.White;

        for (int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i].laser != null && conditions[i].laser.isActive)
            {
                inputIndex = i;
                inputColor = conditions[i].laser.laserColor;
                break;
            }
        }

        //入力がない場合は何もしない
        if (inputIndex == -1) return;

        //入力以外のすべての方向から出力
        for (int i = 0; i < conditions.Length; i++)
        {
            if (i != inputIndex && conditions[i].laser != null)
            {
                conditions[i].laser.SetColor(conditions[i].color == LaserColorType.White ? inputColor : conditions[i].color);
                conditions[i].laser.Fire();
            }
        }

        //マテリアル更新
        if (lightRenderers != null)
        {
            for (int i = 0; i < conditions.Length && i < lightRenderers.Length; i++)
            {
                if (lightRenderers[i] != null)
                {
                    ApplyMaterial(lightRenderers[i], conditions[i].color == LaserColorType.White ? inputColor : conditions[i].color);
                }
            }
        }
    }
}
