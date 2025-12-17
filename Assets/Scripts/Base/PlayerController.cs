using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FireLaser fireLaser;       //FireLaserコンポーネント
    [SerializeField] private LaserColorType laserColor = LaserColorType.Red;
    private Camera mainCamera;                          //メインカメラ
    
    void Start()
    {
        //メインカメラの取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        //左クリックまたはタップを判定する
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckClickOnPlayer();
        }
    }
    
    void CheckClickOnPlayer()
    {
        if (mainCamera == null) return;
        
        //マウス位置からRayを飛ばす
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
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
