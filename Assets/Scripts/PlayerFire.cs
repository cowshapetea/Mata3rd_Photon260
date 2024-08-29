using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // 큐브 Prefab
    public GameObject cubeFactory;
    void Start()
    {
        
    }

    void Update()
    {
        // 만약에 내 것이라면
        if (photonView.IsMine)
        {
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
}
