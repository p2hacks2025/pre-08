using UnityEngine;

public class FireLaser : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab; //プレハブ
    [SerializeField] private Transform firePoint;   //発射位置
    [HideInInspector] public bool isActive = false;
    
    //色の設定
    [SerializeField] private LaserColorType laserColorType = LaserColorType.Red;
    private LaserColorType currentColor;
    
    void Start()
    {
        currentColor = laserColorType;
    }
    
    public void SetColor(LaserColorType colorType)
    {
        currentColor = colorType;
    }
    
    public void Fire()
    {
        if (laserPrefab == null || isActive) return;
        
        //レーザーを生成
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        
        //色を設定
        LaserPointer laserPointer = laser.GetComponent<LaserPointer>();
        if (laserPointer != null)
        {
            laserPointer.laserColorType = currentColor;
        }
        
        isActive = true;
    }
}
