using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // 큐브 Prefab
    public GameObject cubeFactory;

    // Impact Prefab
    public GameObject impactFactory;

    // 총알 Prefab
    public GameObject bulletFactory;

    // 총구 Transform
    public Transform firePos;

    void Start()
    {
        
    }

    void Update()
    {
        // 만약에 내 것이라면
        if (photonView.IsMine)
        {
            // 마우스 왼쪽 버튼 누르면
            if (Input.GetMouseButtonDown(0))
            {
                // 총알공장에서 총알을 생성, 위치 회전 세팅
               PhotonNetwork.Instantiate("Bullet", firePos.position, Camera.main.transform.rotation);
                //photonView.RPC(nameof(CreateBullet), RpcTarget.All,firePos.position,Camera.main.transform.rotation);
                
            }


            // 마우스 오른쪽 버튼 누르면
            if (Input.GetMouseButtonDown(1))
            {
                // RPC 새로 만들경우
               // photonView.RPC(nameof(Fire), RpcTarget.All,Camera.main.transform.position,Camera.main.transform.forward);
                
                // 카메라 위치, 카메라 앞방향으로 된 Ray를 만들자.
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                // 만들어진 Ray를 이용해서 Raycast 하자.
                RaycastHit hit;
                // 만약 부딪힌 지점이 있으면
                if (Physics.Raycast(ray, out hit))
                {
                    // 폭발효과를 생성하고 부딪힌 위치에 놓자.
                    //GameObject impact = Instantiate(impactFactory);
                    //impact.transform.position = hit.point;
                    //CreateImpact(hit.point);
                    photonView.RPC(nameof(CreateImpact), RpcTarget.All,hit.point); //("CreateImpact")
                }
            }
           

            // 1번 키 누르면 // 카메라의 앞 방향으로 5만큼 떨어진 위치에 큐브를 생성하고 싶다.
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 큐브 공장에서 큐브를 생성하자
                //GameObject Cube = Instantiate(cubeFactory);
                // 카메라의 앞 방향으로 5만큼 떨어진 위치를 구하자.
                Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
                // 큐브 공장에서 큐브를 생성, 위치, 회전
                //GameObject Cube = Instantiate(cubeFactory);
                //GameObject Cube = Instantiate(cubeFactory, pos, Quaternion.identity);
                PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
                // 생성된 큐브를 계산한 위치에 놓자
                //cube.transform.position = pos;
                photonView.RPC(nameof(CreateCube), RpcTarget.All,pos);
            }
        }
        // 만약에 내 것이 아니라면
        //if (photonView.IsMine == false) return;

        //// 1번 키 누르면 // 카메라의 앞 방향으로 5만큼 떨어진 위치에 큐브를 생성하고 싶다.
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    // 큐브 공장에서 큐브를 생성하자
        //    //GameObject cube = Instantiate(cubeFactory);
        //    // 카메라의 앞 방향으로 5만큼 떨어진 위치를 구하자.
        //    Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
        //    // 큐브 공장에서 큐브를 생성, 위치, 회전
        //    //GameObject Cube = Instantiate(cubeFactory);
        //    //GameObject Cube = Instantiate(cubeFactory, pos, Quaternion.identity);
        //    PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
        //    // 생성된 큐브를 계산한 위치에 놓자
        //    //cube.transform.position = pos;

        //}

    }
    [PunRPC]
    void CreateBullet(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletFactory,position, rotation);
    }
    [PunRPC]
    void CreateImpact(Vector3 position)
    {
        GameObject impact = Instantiate(impactFactory);
        impact.transform.position = position;
    }
    //[PunRPC]
    //void Fire(Vector3 pos, Vector3 forward)

    [PunRPC]
    void CreateCube(Vector3 position)
    {
        //Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
        Instantiate(cubeFactory, position, Quaternion.identity);
    }
}
