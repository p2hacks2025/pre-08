using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class ColorMaterialData
{
    public LaserColorType colorType;    //レーザーの色タイプ
    public Material colorMaterial;      //対応するマテリアル
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private FireLaser fireLaser;               //FireLaserコンポーネント
    [SerializeField] private LaserColorType laserColor;         //発射するレーザーの色
    [SerializeField] private Renderer[] auraRenderers;          //オーラのレンダラー配列
    [SerializeField] private ColorMaterialData[] materialDatas; //色とマテリアルのデータ配列
    
    private Camera mainCamera;  //メインカメラのキャッシュ

    void Start()
    {
        //メインカメラを取得
        mainCamera = Camera.main;
        
        //レーザーの色を設定
        if (fireLaser != null)
        {
            fireLaser.SetColor(laserColor);
        }
        
        //オーラのマテリアルを適用
        ApplyAuraMaterial();
    }

    void Update()
    {
        //入力位置を取得
        Vector2? inputPosition = GetInputPosition();
        if (inputPosition.HasValue)
        {
            //プレイヤーがクリックされたかチェック
            CheckClickOnPlayer(inputPosition.Value);
        }
    }

    private void ApplyAuraMaterial()
    {
        if (auraRenderers == null || materialDatas == null) return;
        
        //各オーラレンダラーに対して処理
        foreach (var renderer in auraRenderers)
        {
            if (renderer == null) continue;
            
            //マテリアルデータから一致する色のマテリアルを探して適用
            foreach (var data in materialDatas)
            {
                if (data.colorType == laserColor && data.colorMaterial != null)
                {
                    renderer.material = data.colorMaterial;
                    break;
                }
            }
        }
    }
    private Vector2? GetInputPosition()
    {
        //マウス入力をチェック
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return Mouse.current.position.ReadValue();
        }
        
        //タッチ入力をチェック
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        
        return null;
    }
    private void CheckClickOnPlayer(Vector2 inputPosition)
    {
        if (mainCamera == null) return;

        //スクリーン座標からレイを生成
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        
        //レイキャストで当たり判定
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Collide))
        {
            //クリックしたオブジェクトが自分自身（プレイヤー）かチェック
            if (hit.collider.gameObject == gameObject)
            {
                FirePlayerLaser();
            }
        }
    }
    public void FirePlayerLaser()
    {
        if (fireLaser != null)
        {
            //レーザーの色を設定して発射
            fireLaser.SetColor(laserColor);
            fireLaser.Fire();
        }
    }
}
