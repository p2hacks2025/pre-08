using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private ChangeScene changeScene;
    private string pendingScene = "";
    private bool waitingForClick = false;

    //シングルトンインスタンス
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
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

    private void Start()
    {
        //コンポーネントの取得
        changeScene = GetComponent<ChangeScene>();
    }
    
    private void Update()
    {
        //左クリック待機中の場合
        if (waitingForClick && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (pendingScene != "")
            {
                if (changeScene != null)
                {
                    changeScene.LoadScene(pendingScene);
                }
                pendingScene = "";
                waitingForClick = false;
            }
        }

        //Rキーでシーン再読み込み（デバッグ用）
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (changeScene != null)
            {
                changeScene.ReloadCurrentScene();
            }
        }
    }

    //左クリック待ちでシーン遷移
    public void LoadSceneWithClick(string sceneName)
    {
        pendingScene = sceneName;
        waitingForClick = true;
    }

    public void GameOver()
    {
        LoadSceneWithClick("GameOverScene");
    }
    public void GameClear()
    {
        LoadSceneWithClick("GameClearScene");
    }
}
