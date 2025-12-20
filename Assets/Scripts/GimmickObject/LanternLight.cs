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
        
        //レーザーポインターの色に応じて出力する
        for (int i = 0; i < conditions.Length && i < lightRenderers.Length; i++)
        {
            var con = conditions[i];
            var renderer = lightRenderers[i];
            if (con.laser != null && renderer != null)
            {
                //白色の場合のみレーザーの現在の色を使用、それ以外は条件の色を使用
                LaserColorType outputColor = con.color == LaserColorType.White ? con.laser.laserColor : con.color;
                con.laser.SetColor(outputColor);
                con.laser.Fire();
                
                //ライトのマテリアルを更新
                ApplyMaterial(renderer, outputColor);
            }
        }
    }
}
