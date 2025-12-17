using UnityEngine;

public class DirectionChanger : DragDrop
{
    [SerializeField] private FireLaser laserA;  //レーザーA
    [SerializeField] private FireLaser laserB;  //レーザーB
    private bool isActive = false;              //起動フラグ
    
    private void Update()
    {
        if (isActive || laserA == null || laserB == null) return;
        
        //Aがアクティブの場合、Bへ色を引き継いで発射
        if (laserA.isActive)
        {
            laserB.SetColor(laserA.laserColor);
            laserB.Fire();
            isActive = true;
        }
        //Bがアクティブの場合、Aへ色を引き継いで発射
        else if (laserB.isActive)
        {
            laserA.SetColor(laserB.laserColor);
            laserA.Fire();
            isActive = true;
        }
    }
}
