using UnityEngine;

public class DirectionChanger : DragDrop
{
    [SerializeField] private FireLaser laserA;
    [SerializeField] private FireLaser laserB;
    private bool isActive = false;
    
    private void Update()
    {
        if (isActive || laserA == null || laserB == null) return;
        
        if (laserA.isActive)
        {
            //色を変更
            laserB.SetColor(laserA.laserColor);
            //発射
            laserB.Fire();
            isActive = true;
        }
        else if (laserB.isActive)
        {
            //色を変更
            laserA.SetColor(laserB.laserColor);
            //発射
            laserA.Fire();
            isActive = true;
        }
    }
}
