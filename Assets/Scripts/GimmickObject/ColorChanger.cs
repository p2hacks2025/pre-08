using UnityEngine;

//色条件の設定
[System.Serializable]
public class ColorCondition
{
    public FireLaser laser;             //レーザー
    public LaserColorType colorType;    //色
}

public class ColorChanger : DragDrop
{
    [SerializeField] private ColorCondition conditionA;  //条件A
    [SerializeField] private ColorCondition conditionB;  //条件B
    private bool isActive = false;                       //起動フラグ
    
    private void Update()
    {
        if (isActive || conditionA.laser == null || conditionB.laser == null) return;
        
        //Aがアクティブな場合、Bから出力
        if (conditionA.laser.isActive)
        {
            //色が一致すればBから出力
            if (conditionA.laser.laserColor == conditionA.colorType)
            {
                conditionB.laser.SetColor(conditionB.colorType);
                conditionB.laser.Fire();
                isActive = true;
            }
            else
            {
                //条件不一致
                Debug.LogWarning($"ColorChanger: 条件Aの色が不一致 (入力: {conditionA.laser.laserColor}, 必要: {conditionA.colorType})");
                isActive = true;
            }
        }
        //Bがアクティブな場合、Aから出力
        else if (conditionB.laser.isActive)
        {
            //色が一致すればAから出力
            if (conditionB.laser.laserColor == conditionB.colorType)
            {
                conditionA.laser.SetColor(conditionA.colorType);
                conditionA.laser.Fire();
                isActive = true;
            }
            else
            {
                //条件不一致
                Debug.LogWarning($"ColorChanger: 条件Bの色が不一致 (入力: {conditionB.laser.laserColor}, 必要: {conditionB.colorType})");
                isActive = true;
            }
        }
    }
}
