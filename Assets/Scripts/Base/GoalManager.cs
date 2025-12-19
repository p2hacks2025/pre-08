using UnityEngine;

public class GoalManager : ResponseLaser
{
    protected override void OutputLaser()
    {
        if (conditions == null) return;

        //全ての条件が満たされているか確認
        bool allConditionsMet = true;
        
        foreach (var con in conditions)
        {
            //条件が検知されていない場合は失敗
            if (!con.isDetected || con.laser == null)
            {
                allConditionsMet = false;
                break;
            }
            
            //White以外の場合、色が一致しているか確認
            if (con.color != LaserColorType.White && con.laser.laserColor != con.color)
            {
                allConditionsMet = false;
                break;
            }
        }
        
        //結果に応じてシーン遷移（左クリック待ち）
        if (allConditionsMet)
        {
            Debug.Log("ゲームクリア！");
            GameManager.Instance?.GameClear();
        }
        else
        {
            Debug.Log("ゲームオーバー！");
            GameManager.Instance?.GameOver();
        }
    }
}
