using UnityEngine;

//出力条件の設定
[System.Serializable]
public class OutputCondition
{
    public FireLaser outputLaser;       //出力レーザー
    public LaserColorType outputColor;  //出力する色
}

public class CountChanger : ResponseLaser
{
    [SerializeField] private OutputCondition[] outputConditions;    //出力条件
    
    protected override void OutputLaser()
    {
        if (outputConditions == null) return;
        
        //入力された色を取得(White用)
        LaserColorType inputColor = LaserColorType.White;
        foreach (var con in conditions)
        {
            if (con.laser != null && con.isDetected && con.laser.isActive)
            {
                inputColor = con.laser.laserColor;
                break;
            }
        }
        
        //各出力レーザーを指定した色で発射
        foreach (var condition in outputConditions)
        {
            if (condition.outputLaser != null)
            {
                //White指定の場合は入力色を使用、それ以外は設定色を使用
                LaserColorType outputColor = condition.outputColor == LaserColorType.White ? inputColor : condition.outputColor;
                
                condition.outputLaser.SetColor(outputColor);
                condition.outputLaser.Fire();
            }
        }
    }
}
