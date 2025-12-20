using UnityEngine;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour
{
    [SerializeField] private Text stageNumberText;      //ステージナンバー表示用テキスト    
    [SerializeField] private int minStage = 1;          //ステージの最小値
    [SerializeField] private int maxStage = 10;         //ステージの最大値
    private ChangeScene changeScene;                     //シーン遷移用スクリプト参照
    private int currentStage = 1;                       //現在選択されているステージ
    
    void Start()
    {   
        //コンポーネントの取得
        changeScene = GetComponent<ChangeScene>();
        //初期ステージナンバーを表示
        UpdateStageDisplay();
    }

    public void PrevStage()
    {
        if (currentStage > minStage)
        {
            currentStage--;
            UpdateStageDisplay();
        }
    }
    public void NextStage()
    {
        if (currentStage < maxStage)
        {
            currentStage++;
            UpdateStageDisplay();
        }
    }
    public void DecideStage()
    {
        //選択されたステージシーンを読み込む
        changeScene.LoadScene("Stage" + currentStage);
    }
    private void UpdateStageDisplay()
    {
        if (stageNumberText != null)
        {
            stageNumberText.text = "Stage " + currentStage.ToString();
        }
        
        //ボタンの有効/無効を更新
        UpdateButtonStates();
    }
    private void UpdateButtonStates()
    {
        //Prevボタンの有効/無効設定
        Button prevButton = GameObject.Find("PrevBtn").GetComponent<Button>();
        if (prevButton != null)
        {
            prevButton.interactable = currentStage > minStage;
        }

        //Nextボタンの有効/無効設定
        Button nextButton = GameObject.Find("NextBtn").GetComponent<Button>();
        if (nextButton != null)
        {
            nextButton.interactable = currentStage < maxStage;
        }
    }
}
