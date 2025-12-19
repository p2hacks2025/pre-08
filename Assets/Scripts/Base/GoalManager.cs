using UnityEngine;

// ゴール条件の設定
[System.Serializable]
public class GoalCondition
{
    public LaserColorType colorType;     //必要な色
    public int requiredCount = 1;        //必要な回数
    [HideInInspector] public int currentCount = 0; //現在のカウント
}

public class GoalManager : MonoBehaviour
{
    [SerializeField] private GoalCondition[] goalConditions;
    
    public void OnLaserHit(LaserColorType hitColor)
    {
        //該当する色の条件を探してカウント
        foreach (var condition in goalConditions)
        {
            if (condition.colorType == hitColor && condition.currentCount < condition.requiredCount)
            {
                condition.currentCount++;
                Debug.Log($"{hitColor}が{condition.currentCount}/{condition.requiredCount}回当たりました");
                break;
            }
        }
        
        //全ての条件が満たされたかチェック
        CheckGoalConditions();
    }
    
    private void CheckGoalConditions()
    {
        bool allConditionsMet = true;
        
        foreach (var condition in goalConditions)
        {
            if (condition.currentCount < condition.requiredCount)
            {
                allConditionsMet = false;
                break;
            }
        }
        
        if (allConditionsMet)
        {
            Debug.Log("ゲームクリア！");
            GameManager.Instance.GameClear();
        }
        else
        {
            Debug.Log("ゲームオーバー！");
            GameManager.Instance.GameOver();
        }
    }
}
