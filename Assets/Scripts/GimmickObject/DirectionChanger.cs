using UnityEngine;

public class DirectionChanger : ResponseLaser
{
    protected override void OutputLaser()
    {
        //条件が2つ以外のときは処理しない
        if (conditions == null || conditions.Length != 2) return;
        
        //どちらのレーザーがアクティブか判定して、反対側から出力
        for (int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i].laser != null && conditions[i].laser.isActive)
            {
                //反対側のレーザーを取得
                int oppositeIndex = (i + 1) % conditions.Length;
                var outputLaser = conditions[oppositeIndex].laser;
                
                if (outputLaser != null)
                {
                    //入力レーザーの色で出力
                    outputLaser.SetColor(conditions[i].laser.laserColor);
                    outputLaser.Fire();
                    return;
                }
            }
        }
    }
}
