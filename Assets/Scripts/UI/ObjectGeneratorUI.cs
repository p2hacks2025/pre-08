using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectGeneratorUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject objectPrefab;   //オブジェクトプレハブ
    
    [SerializeField] private float spawnDepth = 10.0f;  //カメラからの距離
    
    private GameObject spawnedObject;   //生成オブジェクト
    private DragDrop dragDropComponent; //生成オブジェクトのDragDropコンポーネント
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (objectPrefab == null)
        {
            Debug.LogWarning("objectPrefabが設定されていません");
            return;
        }
        
        //スクリーン座標をワールド座標に変換
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, spawnDepth);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        //指定位置にオブジェクトを生成
        spawnedObject = Instantiate(objectPrefab, worldPos, Quaternion.identity);
        
        //DragDropコンポーネントを取得
        dragDropComponent = spawnedObject.GetComponent<DragDrop>();
        
        if (dragDropComponent != null)
        {
            //DragDropのドラッグ処理を開始
            dragDropComponent.OnPointerDown(eventData);
        }
        else
        {
            Debug.LogWarning("生成されたオブジェクトにDragDropコンポーネントがありません");
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        //生成されたオブジェクトのドラッグ処理を中継
        if (dragDropComponent != null)
        {
            dragDropComponent.OnDrag(eventData);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        //生成されたオブジェクトのドロップ処理（スナップ等）を実行
        if (dragDropComponent != null)
        {
            dragDropComponent.OnPointerUp(eventData);
        }
        
        //参照をリセット
        spawnedObject = null;
        dragDropComponent = null;
    }
}
