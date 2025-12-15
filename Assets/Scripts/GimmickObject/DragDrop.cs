using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    private Camera mainCamera;
    private float fixedY; //Y軸を固定するための変数
    
    protected virtual void Start()
    {
        mainCamera = Camera.main;
        fixedY = transform.position.y; //初期y座標
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //スクリーン座標を変換
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, mainCamera.WorldToScreenPoint(transform.position).z);
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPos);
        //y座標を固定
        worldPoint.y = fixedY;
        offset = transform.position - worldPoint;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        //スクリーン座標を変換
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, mainCamera.WorldToScreenPoint(transform.position).z);
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(screenPos);
        //y座標を固定
        worldPoint.y = fixedY;
        transform.position = worldPoint + offset;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        // ドロップ時の処理（必要に応じて実装）
    }
}
