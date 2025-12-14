using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    [SerializeField] private FireLaser laserA;
    [SerializeField] private FireLaser laserB;
    private bool isActive = false;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (isActive || laserA == null || laserB == null) return;
        
        if (laserA.isActive)
        {
            laserB.Fire();
            isActive = true;
        }
        else if (laserB.isActive)
        {
            laserA.Fire();
            isActive = true;
        }
    }
}
