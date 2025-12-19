using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public ChangeScene changeScene;   //ChangeSceneコンポーネント

    //インスタンス（シングルトン）
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

    void Start()
    {
        //コンポーネントの取得
        changeScene = GetComponent<ChangeScene>();
    }
    
    void Update()
    {
        //Rキーを押すとシーンを再読み込みする(デバッグ用)
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (changeScene != null)
            {
                changeScene.ReloadCurrentScene();
            }
        }
    }

    public void GameTitle()
    {
        //タイトル画面へ戻る
        if (changeScene != null)
        {
            changeScene.LoadScene("TitleScene");
        }
    }
    public void GameOver()
    {
        //ゲームオーバー画面へ遷移
        if (changeScene != null)
        {
            changeScene.LoadScene("GameOverScene");
        }
    }
    public void GameClear()
    {
        //ゲームクリア画面へ遷移
        if (changeScene != null)
        {
            changeScene.LoadScene("GameClearScene");
        }
    }
}
