using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private ChangeScene changeScene;
    private string sceneName = "";                  //シーン名
    [SerializeField] private float delay = 2.0f;    //遅延時間(秒)
    private float timer = 0.0f;                     //タイマー

    private void Awake()
    {
        //シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //コンポーネントの取得
        changeScene = GetComponent<ChangeScene>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                //時間経過後にゲームオーバーシーンへ遷移
                changeScene.LoadScene(sceneName);
            }
        }
    }

    public void GoToTitle()
    {
        Debug.Log("タイトルへ戻る");
        //タイトルシーンへ遷移
        timer = delay;
        sceneName = "TitleScene";
    }
    public void GameOver()
    {
        Debug.Log("ゲームオーバー！");
        //ゲームオーバーシーンへ遷移
        timer = delay;
        sceneName = "GameOverScene";
    }
    public void GameClear()
    {
        Debug.Log("ゲームクリア！");
        //ゲームクリアシーンへ遷移
        timer = delay;
        sceneName = "GameClearScene";
    }
}
