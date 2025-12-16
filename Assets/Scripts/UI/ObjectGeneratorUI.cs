using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ObjectGeneratorUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject objectPrefab;   //オブジェクトプレハブ
    [SerializeField] private UIController UI;
    
    [SerializeField] private float spawnDepth = 10.0f;  //カメラからの距離
    
    private GameObject spawnedObject;   //生成オブジェクト
    private DragDrop dragDropComponent; //生成オブジェクトのDragDropコンポーネント
    private Image image;                //UIアイコン
    
    private void Start()
    {
        //コンポーネントの取得
        image = GetComponent<Image>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (objectPrefab == null)
        {
            Debug.LogWarning("objectPrefabが設定されていません");
            return;
        }
        
        //UIアイコンを非表示にする
        if (image != null)
        {
            image.enabled = false;
        }
        
        //パネルをスライドアウト
        if (UI != null)
        {
            UI.SlideOutPanel();
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
            //DragDropを初期化
            dragDropComponent.OnPointerDown(eventData);
            
            //スナップ結果のコールバックを登録する
            dragDropComponent.OnSnapResult = (success) =>
            {
                if (success)
                {
                    //スナップ成功：UIアイコンを操作不可にする
                    if (image != null) image.raycastTarget = false;
                }
                else
                {
                    //スナップ失敗：UIアイコンを再表示、パネルを再表示する
                    if (image != null) image.enabled = true;
                    if (UI != null) UI.SlideInPanel();
                    if (spawnedObject != null) Destroy(spawnedObject);
                }
                dragDropComponent.OnSnapResult = null;
            };
        }
        else
        {
            Debug.LogWarning("生成されたオブジェクトにDragDropコンポーネントがありません");
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        //生成オブジェクトのドラッグ処理を実行
        if (dragDropComponent != null)
        {
            dragDropComponent.OnDrag(eventData);
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        //生成オブジェクトのドロップ処理を実行
        if (dragDropComponent != null)
        {
            dragDropComponent.OnPointerUp(eventData);
            dragDropComponent = null;
        }
    }
}
