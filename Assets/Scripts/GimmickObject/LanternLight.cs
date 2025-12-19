using UnityEngine;

[System.Serializable]
public class LanternData
{
    public LaserColorType colorType;
    public Material colorMaterial;
}

public class LanternLight : ColorChanger
{
    [SerializeField] private LanternData[] datas;
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
        //親クラスのレーザー出力処理を実行
        base.OutputLaser();

        if (conditions == null || lightRenderers == null) return;
        //出力レーザー色に応じてライトのマテリアルを変更
        for (int i = 0; i < conditions.Length && i < lightRenderers.Length; i++)
        {
            var con = conditions[i];
            if (con.laser != null && lightRenderers[i] != null)
            {
                ApplyMaterial(lightRenderers[i], con.laser.laserColor);
            }
        }
    }
    private void ApplyMaterial(Renderer renderer, LaserColorType colorType)
    {
        if (datas == null) return;
        
        //現在のレーザー色に対応するマテリアルを取得
        foreach (var data in datas)
        {
            if (data.colorType == colorType)
            {
                if (data.colorMaterial != null)
                {
                    renderer.material = data.colorMaterial;
                }
                break;
            }
        }
    }
}
