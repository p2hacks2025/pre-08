using UnityEngine;
using UnityEngine.InputSystem;

public class TapToScene : MonoBehaviour
{
    [SerializeField] private string sceneName;  //遷移先のシーン名
    private ChangeScene changeScene;            //ChangeSceneコンポーネント

    void Start()
    {
        changeScene = GetComponent<ChangeScene>();
    }

    void Update()
    {
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
