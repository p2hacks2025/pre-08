using UnityEngine;

public class FireLaser : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;    //プレハブ
    [SerializeField] private Transform firePoint;       //発射位置
    [HideInInspector] public bool isActive = false;
    [HideInInspector] public LaserColorType laserColor = LaserColorType.Red;
    
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
        if (laserPointer != null) laserPointer.laserColor = laserColor;
        
        isActive = true;
    }
}
