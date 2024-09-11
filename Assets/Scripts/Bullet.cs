using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    public float moveSpeed = 10;

    Rigidbody rb;

    // 충돌시 효과 Prefab
    public GameObject exploFactory;


    // Start is called before the first frame update
    void Start()
    {
        // 내것일때만
        //if (photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.up * moveSpeed;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 내것일때만
        if (photonView.IsMine)
        {
            // 부딪힌 지점을 향해서 Raycast 하자.
            Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                
            }
            // 폭발효과를 만들자
            photonView.RPC(nameof(CreateExplo),RpcTarget.All, transform.position, hit.normal);
            // 나를 파괴하자
            PhotonNetwork.Destroy(gameObject);
        }
    }
    [PunRPC]
    void CreateExplo(Vector3 position, Vector3 normal)
    {
        // 폭발효과 생성
        GameObject explo = Instantiate(exploFactory);
        // 생성된 효과를 나의 위치에 옮김
        explo.transform.position = position;
        // 생성된 효과의 앞방향을 부딪힌 지점의 normal 바꾸자
        explo.transform.forward = normal;
    }

}
