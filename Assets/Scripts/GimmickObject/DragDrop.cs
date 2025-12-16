using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    private Camera mainCamera;
    private float fixedY; //Y軸を固定するための変数
    private Vector3 originalPosition; //ドラッグ開始時の位置を保存
    
    [SerializeField]
    private float snapDistance = 1.5f; //スナップする距離の閾値
    
    protected virtual void Start()
    {
        mainCamera = Camera.main;
        fixedY = transform.position.y; //初期y座標
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //ドラッグ開始時の位置を保存
        originalPosition = transform.position;
        
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
        //"SnapPos"タグのオブジェクトを全て検索
        GameObject[] slots = GameObject.FindGameObjectsWithTag("SnapPos");
        
        GameObject nearestSlot = null;
        float nearestDistance = float.MaxValue;
        
        //最も近いスロットを探す
        foreach (GameObject slot in slots)
        {
            float distance = Vector3.Distance(transform.position, slot.transform.position);
            
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestSlot = slot;
            }
        }
        
        //スナップ距離以内に有効なスロットがある場合は吸着
        if (nearestSlot != null && nearestDistance <= snapDistance)
        {
            transform.position = nearestSlot.transform.position;
        }
        else
        {
            //スロットが範囲外の場合は元の位置に戻す
            transform.position = originalPosition;
        }
    }
}
