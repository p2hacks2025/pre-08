using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TapToScene : MonoBehaviour
{
    [SerializeField] private string sceneName;      //遷移先のシーン名
    [SerializeField] private Text text;             //表示するテキスト
    [SerializeField] private float fadeDuration = 1.0f; //フェードの周期（秒）
    private ChangeScene changeScene;                //ChangeSceneコンポーネント

    void Start()
    {
        changeScene = GetComponent<ChangeScene>();
    }

    void Update()
    {
        //テキストのフェード処理
        UpdateTextFade();

        //マウスまたはタッチ入力を検知
        if (IsInputDetected())
        {
            //シーンを切り替え
            if (changeScene != null)
            {
                changeScene.LoadScene(sceneName);
            }
        }
    }

    private void UpdateTextFade()
    {
        if (text == null) return;

        //PingPongで0から1の間を往復する値を取得
        float alpha = Mathf.PingPong(Time.time / fadeDuration, 1.0f);
        
        //テキストの色を更新（透明から白へ）
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    private bool IsInputDetected()
    {
        //マウス入力をチェック
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return true;
        }

        //タッチ入力をチェック
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }
}
