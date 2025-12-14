using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float speed = 5f;
    private bool isActive = true;
    
    //色の設定
    [HideInInspector] public LaserColorType laserColorType = LaserColorType.Red;
    [SerializeField] private LaserColorData[] colorMaterials; //各色に対応するマテリアル
    private Renderer laserRenderer;

    void Start()
    {
        //コンポーネントの取得
        rb = GetComponent<Rigidbody>();
        laserRenderer = GetComponent<Renderer>();
        
        //初速度を与える
        rb.linearVelocity = transform.forward * speed;
        isActive = true;
        
        //色に応じたマテリアルを適用
        ApplyColorMaterial();
    }
    
    void ApplyColorMaterial()
    {
        if (laserRenderer == null || colorMaterials == null) return;
        
        //色タイプに対応するマテリアルを検索
        foreach (var colorData in colorMaterials)
        {
            if (colorData.colorType == laserColorType && colorData.material != null)
            {
                laserRenderer.material = colorData.material;
                break;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            //レーザーを停止
            rb.linearVelocity = Vector3.zero;
            isActive = false;
            //衝突検知
            if (other.gameObject.CompareTag("Goal"))
            {
                Debug.Log("GameClear!");
            }
            else if (other.gameObject.CompareTag("Pointer"))
            {
                Debug.Log("Pointer Hit!");
                //ポインターのアクティブ化と色情報の伝達
                FireLaser fireLaser = other.gameObject.transform.GetComponent<FireLaser>();
                if (fireLaser != null)
                {
                    fireLaser.isActive = true;
                    fireLaser.SetColor(laserColorType);
                }
            }
        }
        //必要に応じてエフェクトやサウンドを再生
    }
}
