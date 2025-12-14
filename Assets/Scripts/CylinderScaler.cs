using UnityEngine;

public class CylinderScaler : MonoBehaviour
{
    [SerializeField] private Transform startPoint;  //始点
    [SerializeField] private Transform endPoint;    //終点
    
    void Start()
    {
        //初期更新
        UpdateCylinder();
    }

    void Update()
    {
        //startPointとendPointが設定されている場合、常に更新
        if (startPoint != null && endPoint != null)
        {
            UpdateCylinder();
        }
    }

    public void UpdateCylinder()
    {
        if (startPoint == null || endPoint == null) return;

        //始点と終点の位置
        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;
        
        //2点間の距離を計算
        float distance = Vector3.Distance(startPos, endPos);
        
        //Cylinderのスケール調整
        Vector3 newScale = transform.localScale;
        newScale.y = distance * 0.5f;
        transform.localScale = newScale;
        
        //2点の中間点に配置
        Vector3 midPoint = (startPos + endPos) / 2.0f;
        transform.position = midPoint;
        
        //2点を結ぶ方向に回転
        Vector3 direction = endPos - startPos;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }
    }
}
