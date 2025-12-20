using UnityEngine;

public class FireLaser : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;    //プレハブ
    [SerializeField] private Transform firePoint;       //発射位置
    [HideInInspector] public bool isActive = false;
    [HideInInspector] public LaserColorType laserColor = LaserColorType.Red;
    private LaserPointer currentLaserPointer;           //現在のレーザーポインター
    
    public void SetColor(LaserColorType colorType)
    {
        laserColor = colorType;
    }
    
    public void Fire()
    {
        if (laserPrefab == null || isActive) return;
        
        //レーザーを生成する
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        
        //色を設定する
        LaserPointer laserPointer = laser.GetComponentInChildren<LaserPointer>();
        if (laserPointer != null)
        {
            laserPointer.laserColor = laserColor;
            currentLaserPointer = laserPointer;
            
            //自分のコライダーとレーザーのコライダーの衝突を無効化
            Collider myCollider = GetComponent<Collider>();
            Collider laserCollider = laserPointer.GetComponent<Collider>();
            if (myCollider != null && laserCollider != null)
            {
                Physics.IgnoreCollision(myCollider, laserCollider);
            }
        }
        
        isActive = true;
    }
    
    public LaserPointer GetLaserPointer()
    {
        return currentLaserPointer;
    }
}
