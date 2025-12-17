using UnityEngine;

public class SnapSlot : MonoBehaviour
{
    private GameObject snapObject; //このスロットを占有しているオブジェクト
    
    public bool CheckAvailable()
    {
        return snapObject == null;
    }
    public void Snap(GameObject obj)
    {
        snapObject = obj;
    }
    public void Release(GameObject obj)
    {
        if (snapObject == obj)
        {
            snapObject = null;
        }
    }
    public bool CheckOccupiedBy(GameObject obj)
    {
        return snapObject == obj;
    }
}
