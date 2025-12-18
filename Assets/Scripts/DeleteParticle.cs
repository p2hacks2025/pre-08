using UnityEngine;

public class DeleteParticle : MonoBehaviour
{
    [SerializeField] private float deleteTime = 3.0f;  //削除までの時間(秒)
    
    void Start()
    {
        //指定秒数後にオブジェクトを削除
        Destroy(gameObject, deleteTime);
    }
}
