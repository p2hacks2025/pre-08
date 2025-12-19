using UnityEngine;

public class SnapSlot : MonoBehaviour
{
    private GameObject snapObject;                      //このスロットを占有しているオブジェクト
    private GameObject draggingObject;                  //このスロットからドラッグ中のオブジェクト
    [SerializeField] private float checkRadius = 0.5f;  //開始時の検知範囲
    [SerializeField] private GameObject cursor;         //カーソルパーティクル
    
    void Start()
    {
        //開始時に近くにDragDropオブジェクトがあるかチェック
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius);
        foreach (var col in colliders)
        {
            DragDrop dragDrop = col.GetComponent<DragDrop>();
            if (dragDrop != null && col.gameObject != gameObject)
            {
                //近くにオブジェクトが見つかったらスロットを占有
                Snap(col.gameObject);
                break; //1つ見つかったら終了
            }
        }
    }

    void Update()
    {
        //カーソルパーティクルの表示制御（スロットが空いている時のみ表示）
        if (cursor != null)
        {
            cursor.SetActive(CheckAvailable());
        }
    }
    
    public bool CheckAvailable()
    {
        //スロットに何もなく、ドラッグ中でもない場合のみ利用可能
        return snapObject == null && draggingObject == null;
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
    
    //ドラッグ開始時：スロットを一時解放しつつ、ドラッグ中として記録
    public void StartDragging(GameObject obj)
    {
        if (snapObject == obj)
        {
            draggingObject = obj;
            snapObject = null;
        }
    }
    
    //ドラッグ成功時：完全に解放
    public void FinishDragging()
    {
        draggingObject = null;
    }
    
    //ドラッグ失敗時：元のオブジェクトで再占有
    public void CancelDragging()
    {
        if (draggingObject != null)
        {
            snapObject = draggingObject;
            draggingObject = null;
        }
    }
}
