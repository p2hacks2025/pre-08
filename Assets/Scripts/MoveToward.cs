using UnityEngine;

public class MoveToward : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
