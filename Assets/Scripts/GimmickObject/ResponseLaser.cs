using UnityEngine;

//レーザー検知条件の設定
[System.Serializable]
public class LaserCondition
{
    public LaserColorType color;    //色
    public FireLaser laser;         //レーザー
    [HideInInspector] public bool isDetected = false;       //検知フラグ
}

//マテリアルデータ（共通）
[System.Serializable]
public class MaterialData
{
    public LaserColorType colorType;
    public Material colorMaterial;
}

public class ResponseLaser : DragDrop
{
    [SerializeField] protected LaserCondition[] conditions;     //検知条件
    [SerializeField] protected int requiredCount = 0;             //必要な条件数(0で全条件)
    [SerializeField] protected MaterialData[] materialDatas;    //マテリアルデータ
    protected bool isActivated = false;                         //起動済みフラグ
    
    protected void Update()
    {
        if (isActivated || conditions == null) return;
        
        //requiredCountが0の場合は全条件必要として自動設定
        int targetCount = requiredCount > 0 ? requiredCount : conditions.Length;
        
        //各条件をチェック
        DetectLaser();
        
        //必要な条件数が満たされたかチェック
        if (CheckConditions(targetCount))
        {
            isActivated = true;
            OutputLaser();
        }
    }
    
    protected virtual void DetectLaser()
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
                }
            }
        }
    }
    protected virtual void OutputLaser()
    {
        Debug.Log($"{gameObject.name}: 条件が満たされました");
    }
    
    protected bool CheckConditions(int target)
    {
        //満たされた条件数をカウント
        int detectedCount = 0;
        foreach (var condition in conditions)
        {
            if (condition.isDetected) detectedCount++;
        }
        return detectedCount >= target;
    }
    public void ResetConditions()
    {
        isActivated = false;
        foreach (var condition in conditions)
        {
            condition.isDetected = false;
        }
    }
    
    protected void ApplyMaterial(Renderer renderer, LaserColorType colorType)
    {
        //マテリアル適用（共通処理）
        if (materialDatas == null || renderer == null) return;
        
        foreach (var data in materialDatas)
        {
            if (data.colorType == colorType && data.colorMaterial != null)
            {
                renderer.material = data.colorMaterial;
                break;
            }
        }
    }
}
