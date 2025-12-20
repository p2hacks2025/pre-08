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
    [SerializeField] private FireLaser fireLaser;
    [SerializeField] private LaserColorType laserColor;         //発射するレーザーの色
    [SerializeField] private Renderer[] auraRenderers;          //オーラのRenderer配列
    [SerializeField] private ColorMaterialData[] materialDatas; //色とマテリアルを対応させるデータ配列
    
    private Camera mainCamera;

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
        
        //全てのRendererに対して処理
        foreach (var renderer in auraRenderers)
        {
            if (renderer == null) continue;
            
            //マテリアルデータから一致するマテリアルを探して適用
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

        //レイキャストでクリック位置を確認
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.AllLayers, QueryTriggerInteraction.Collide))
        {
            //クリックしたオブジェクトが彦星かチェック
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
