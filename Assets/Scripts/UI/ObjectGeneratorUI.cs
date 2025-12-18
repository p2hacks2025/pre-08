using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectGeneratorUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform objectPlace; //移動対象のオブジェクトの初期位置
    private GameObject targetObject;                //移動対象のオブジェクト
    [SerializeField] private Transform field;       //配置フィールド
    [SerializeField] private UIController UI;       //UIコントローラー
    private DragDrop dragDropComponent;             //DragDropコンポーネント
    private RawImage rawImage;                      //UIアイコン
    
    private void Start()
    {
        //コンポーネントの取得
        rawImage = GetComponent<RawImage>();
        targetObject = objectPlace.GetChild(0).gameObject;
        dragDropComponent = targetObject.GetComponent<DragDrop>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        //UIアイコンを非表示
        rawImage.enabled = false;
        //パネルをスライドアウト
        UI.SlideOutPanel();
        
        //マウス位置をワールド座標に変換してオブジェクトを配置
        Vector3 screenPos = new Vector3(eventData.position.x, eventData.position.y, Camera.main.WorldToScreenPoint(targetObject.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.y = 1f;
        targetObject.transform.position = worldPos;
        
        //固定Y座標を更新してからDragDropを初期化
        dragDropComponent.UpdateFixedY(1f);
        dragDropComponent.OnPointerDown(eventData);
        
        //offsetをゼロに設定してマウス位置に正確に配置
        dragDropComponent.SetOffset(Vector3.zero);
        dragDropComponent.OnDrag(eventData);
        
        //スナップ結果のコールバックを登録
        dragDropComponent.OnSnapResult = (success) =>
        {
            if (success)
            {
                //成功：UIアイコンを操作不可
                rawImage.raycastTarget = false;
                //親オブジェクトを変更
                targetObject.transform.SetParent(field);
            }
            else
            {
                //失敗：元の位置に戻してUIアイコンを再表示、パネルを戻す
                targetObject.transform.position = objectPlace.position;
                rawImage.enabled = true;
                UI.SlideInPanel();
            }
            dragDropComponent.OnSnapResult = null;
        };
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        dragDropComponent.OnDrag(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        dragDropComponent.OnPointerUp(eventData);
    }
}
