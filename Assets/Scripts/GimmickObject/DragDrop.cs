using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector3 offset;
    private Camera mainCamera;
    private float fixedY;               //初期y座標
    private Vector3 originalPosition;   //初期座標
    private SnapSlot currentSlot;       //スロット情報
    [SerializeField] private float snapDistance = 1.0f;  //スナップ範囲
    
    //スナップ結果を通知するコールバック
    public Action<bool> OnSnapResult;
    
    protected virtual void Awake()
    {
        //カメラの取得
        mainCamera = Camera.main;
        //y座標を固定
        fixedY = transform.position.y;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //ドラッグ開始時の位置を保存
        originalPosition = transform.position;
        
        //現在のスロットを解放
        if (currentSlot != null)
        {
            currentSlot.Release(gameObject);
            currentSlot = null;
        }
        
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
        GameObject[] slotObjects = GameObject.FindGameObjectsWithTag("SnapPos");
        
        GameObject nearestSlotObject = null;
        SnapSlot nearestSlot = null;
        float nearestDistance = float.MaxValue;
        
        //最も近い空いているスロットを探す
        foreach (GameObject slotObj in slotObjects)
        {
            SnapSlot slot = slotObj.GetComponent<SnapSlot>();
            
            //SnapSlotコンポーネントがない、または既に占有されている場合はスキップ
            if (slot == null || !slot.CheckAvailable()) continue;
            
            float distance = Vector3.Distance(transform.position, slotObj.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestSlotObject = slotObj;
                nearestSlot = slot;
            }
        }
        
        //スナップ距離以内に有効なスロットがある場合は吸着
        if (nearestSlot != null && nearestDistance <= snapDistance)
        {
            transform.position = nearestSlotObject.transform.position;
            nearestSlot.Snap(gameObject);
            currentSlot = nearestSlot;
            
            //スナップ成功を通知
            OnSnapResult?.Invoke(true);
        }
        else
        {
            //スロットが範囲外の場合は元の位置に戻す
            transform.position = originalPosition;
            
            //スナップ失敗を通知
            OnSnapResult?.Invoke(false);
        }
    }

    public void UpdateFixedY(float newY)
    {
        fixedY = newY;
    }
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
