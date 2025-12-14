using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;     //光線のプレハブ
    [SerializeField] private Transform firePoint;       //発射位置
    private Camera mainCamera;                          //メインカメラ
    
    void Start()
    {
        //メインカメラの取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        //左クリックで判定（New Input System）
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckClickOnPlayer();
        }
    }
    
    void CheckClickOnPlayer()
    {
        if (mainCamera == null) return;
        
        //マウス位置からRayを飛ばす（New Input System）
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        
        //Raycastで当たり判定（isTriggerにも反応）
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Collide))
        {
            //クリックしたオブジェクトがプレイヤーかチェック
            if (hit.collider.gameObject == gameObject)
            {
                FireBeam();
            }
        }
    }
    
    void FireBeam()
    {
        if (beamPrefab == null || firePoint == null) return;
        
        //ビームを生成
        GameObject beam = Instantiate(beamPrefab, firePoint.position, firePoint.rotation);
    }
}
