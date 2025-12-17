using UnityEngine;

//入力条件の設定
[System.Serializable]
public class InputCondition
{
    public FireLaser inputLaser;         //入力レーザー
    public LaserColorType requiredColor; //必要な色
}
//出力条件の設定
[System.Serializable]
public class OutputCondition
{
    public FireLaser outputLaser;       //出力レーザー
    public LaserColorType outputColor;  //出力する色
}

public class CountChanger : DragDrop
{
    [SerializeField] private InputCondition[] inputConditions;      //入力条件
    [SerializeField] private OutputCondition[] outputConditions;    //出力条件
    private int requiredInputCount = 0;                             //必要な入力数
    private bool isActive = false;                                  //起動フラグ
    
    private void Update()
    {
        if (isActive || inputConditions == null || outputConditions == null) return;

        //必要な入力数が条件数と異なる場合は更新
        if  (requiredInputCount != inputConditions.Length) requiredInputCount = inputConditions.Length;
        
        //条件を満たす入力レーザーをカウント
        int validInputCount = 0;
        
        foreach (var condition in inputConditions)
        {
            if (condition.inputLaser == null) continue;
            
            //レーザーがアクティブな場合
            if (condition.inputLaser.isActive)
            {
                //色条件を満たすかチェック
                if (condition.inputLaser.laserColor == condition.requiredColor)
                {
                    validInputCount++;
                }
                else
                {
                    //条件不一致
                    Debug.LogWarning($"CountChanger: 入力色が条件不一致 (入力: {condition.inputLaser.laserColor}, 必要: {condition.requiredColor})");
                    isActive = true;
                }
            }
        }
        
        //必要な入力数に達したら出力
        if (validInputCount >= requiredInputCount)
        {
            //各出力レーザーを指定した色で発射
            foreach (var condition in outputConditions)
            {
                if (condition.outputLaser != null)
                {
                    condition.outputLaser.SetColor(condition.outputColor);
                    condition.outputLaser.Fire();
                }
            }
            
            isActive = true;
        }
    }
}
