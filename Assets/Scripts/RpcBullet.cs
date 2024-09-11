using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpcBullet : MonoBehaviour
{

    // 이동 속력
    public float moveSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
