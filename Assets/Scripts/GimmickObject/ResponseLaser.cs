using UnityEngine;

//レーザー検知条件の設定
[System.Serializable]
public class LaserCondition
{
    public FireLaser laser;         //レーザー
    public LaserColorType color;    //色
    [HideInInspector] public bool isDetected = false; //検知フラグ
}
public class ResponseLaser : DragDrop
{
    [SerializeField] protected LaserCondition[] conditions;  //検知条件
    [SerializeField] private int requiredCount = 0;          //必要な条件数(0で全条件)
    protected bool isActivated = false;                      //起動済みフラグ
    
    protected virtual void Update()
    {
        if (isActivated || conditions == null) return;
        
        //requiredCountが0の場合は全条件必要として自動設定
        int targetCount = requiredCount > 0 ? requiredCount : conditions.Length;
        
        //各条件をチェック
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
                else
                {
                    //色不一致
                    Debug.LogWarning($"{gameObject.name}: レーザー色が条件不一致 (入力: {con.laser.laserColor}, 必要: {con.color})");
                }
            }
        }
        
        //必要な条件数が満たされたかチェック
        if (CheckConditions(targetCount))
        {
            isActivated = true;
            OutputLaser();
        }
    }
    
    protected virtual void OutputLaser()
    {
        Debug.Log($"{gameObject.name}: 全ての条件が満たされました");
    }
    
    protected bool CheckConditions(int target)
    {
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
}
