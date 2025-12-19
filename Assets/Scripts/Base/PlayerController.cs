using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FireLaser fireLaser;       //FireLaserコンポーネント
    [SerializeField] private LaserColorType laserColor; //発射するレーザーの色
    private Camera mainCamera;                          //メインカメラ
    
    void Start()
    {
        //メインカメラの取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        //左クリックまたはタップを判定する
        bool isPressed = false;
        Vector2 inputPosition = Vector2.zero;
        
        //マウス入力
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isPressed = true;
            inputPosition = Mouse.current.position.ReadValue();
        }
        //タッチ入力
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            isPressed = true;
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        
        if (isPressed)
        {
            CheckClickOnPlayer(inputPosition);
        }
    }
    
    public void CheckClickOnPlayer(Vector2 inputPosition)
    {
        if (mainCamera == null) return;
        
        //入力位置からRayを飛ばす
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        RaycastHit hit;
        
        //Raycastで当たり判定
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Collide))
        {
            //クリックしたオブジェクトがプレイヤーかチェック
            if (hit.collider.gameObject == gameObject)
            {
                if (fireLaser != null)
                {
                    //発射
                    fireLaser.SetColor(laserColor);
                    fireLaser.Fire();
                }
            }
        }
    }
}
