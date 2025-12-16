using UnityEngine;

public class ColorChanger : DragDrop
{
    [SerializeField] private FireLaser laserA;
    [SerializeField] private FireLaser laserB;
    [SerializeField] private LaserColorType colorA = LaserColorType.Red;
    [SerializeField] private LaserColorType colorB = LaserColorType.Blue;
    private bool isActive = false;
    
    private void Update()
    {
        if (isActive || laserA == null || laserB == null) return;
        
        if (laserA.isActive)
        {
            //Aの色が一致した場合はBの色に変換
            if (laserA.laserColor == colorA)
            {
                laserB.SetColor(colorB);
                laserB.Fire();
                isActive = true;
            }
        }
        else if (laserB.isActive)
        {
            //Bの色が一致した場合はAの色に変換
            if (laserB.laserColor == colorB)
            {
                laserA.SetColor(colorA);
                laserA.Fire();
                isActive = true;
            }
        }
    }
}
