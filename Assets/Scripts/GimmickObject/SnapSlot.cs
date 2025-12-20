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
        //スロットをオブジェクトで占有
        snapObject = obj;
    }
    public void Release(GameObject obj)
    {
        //スロットの占有を解除
        if (snapObject == obj)
        {
            snapObject = null;
        }
    }
    public bool CheckOccupiedBy(GameObject obj)
    {
        //指定オブジェクトがスロットを占有しているかチェック
        return snapObject == obj;
    }
    
    public void StartDragging(GameObject obj)
    {
        //ドラッグ開始
        if (snapObject == obj)
        {
            draggingObject = obj;
            snapObject = null;
        }
    }
    public void FinishDragging()
    {
        //ドラッグ終了
        draggingObject = null;
    }
    public void CancelDragging()
    {
        //ドラッグをキャンセルして元に戻す
        if (draggingObject != null)
        {
            snapObject = draggingObject;
            draggingObject = null;
        }
    }
}
