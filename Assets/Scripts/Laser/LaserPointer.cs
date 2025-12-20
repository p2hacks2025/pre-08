using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 5f;                  //速度
    [SerializeField] private Renderer laserRenderer;            //Renderer
    [SerializeField] private GameObject hitParticlePrefab;      //衝突時のパーティクル
    [HideInInspector] public LaserColorType laserColor = LaserColorType.Red;    //レーザーの色
    private bool hasCollided = false;                           //衝突済みフラグ

    private void Start()
    {
        //コンポーネントの取得
        rb = GetComponent<Rigidbody>();
        
        //色に応じたマテリアルを適用する
        ApplyColor(laserColor);
        //初速度を与える
        Fire();
    }
    
    private void ApplyColor(LaserColorType color)
    {
        laserRenderer.material = LaserColor.Instance.GetMaterial(color);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //既に衝突済みなら処理しない
        if (hasCollided) return;
        
        if (other.gameObject != null)
        {
            Debug.Log($"{gameObject.name}: {other.gameObject.name} に衝突");

            //レーザーを停止する
            Stop();

            //衝突位置にパーティクルを生成
            if (hitParticlePrefab != null)
            {
                Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            }
                        
            /*
            if (other.gameObject.CompareTag("Goal"))
            {
                //GoalManagerに色情報を伝える
                GoalManager goalManager = other.gameObject.GetComponent<GoalManager>();
                if (goalManager != null)
                {
                    goalManager.OnLaserHit(laserColor);
                }
            }
            */

            //衝突検知
            if (other.gameObject.CompareTag("Pointer"))
            {
                //FireLaserに色情報を伝える
                FireLaser fireLaser = other.gameObject.transform.GetComponent<FireLaser>();
                if (fireLaser != null)
                {
                    fireLaser.SetColor(laserColor);
                    fireLaser.isActive = true;
                }
            }/*
            else if (other.gameObject.CompareTag("Defenser"))
            {
                //ゲームオーバー処理
                GameManager.Instance.GameOver();
            }*/
        }
    }

    public void Fire()
    {
        //発射する
        hasCollided = false;
        if (rb != null) rb.linearVelocity = transform.forward * speed;
    }
    public void Stop()
    {
        //停止する
        hasCollided = true;
        if (rb != null) rb.linearVelocity = Vector3.zero;
    }
    public LaserPointer GetLaserPointer()
    {
        return this;
    }
}
