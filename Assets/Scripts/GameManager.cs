using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject playerFactory;
    
    // 스폰 위치 담을 변수
    public Vector3[] spawnPos;
    public Transform spawnCenter;

    void Start()
    {
        SetSpawnPos();

        // RPC 보내는 빈도 설정
        PhotonNetwork.SendRate = 60;
        // OnPhotonSerializeView 보내고 받고 하는 빈도 설정
        PhotonNetwork.SerializationRate = 60;

        // 내가 위치해야 하는 idx를 알아오자 (현재 방에 들어와 있는 인원 수)
        int idx = PhotonNetwork.CurrentRoom.PlayerCount -1;

        // 플레이어를 생성 (현재 Room 에 접속 되어있는 친구들도 보이게)
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);
        //PhotonNetwork.Instantiate("AA/Player", Vector3.zero, Quaternion.identity); - AA 폴더 안에 플레이어 있을경우 (Resources 안에 있어야함)

    }

    // Update is called once per frame
    int i = 0;
    void Update()
    {
        //i = i + 36;
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //transform.rotation = Quaternion.Euler(i, 0, 0); // x, y, z 축에 대해 도씩 회전
        //cube.transform.position = Vector3(2, 0, 0);
    }

  
    void SetSpawnPos()
    {
        // maxPlayer 를 현재 방의 최대 인원으로 설정
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers; //currentroom에 최대인원. currentroom이 지금 없어서 오류

        // 최대 인원 만큼 spawnPos 의 공간 할당
        spawnPos = new Vector3[maxPlayer];

        // spawnPos 간의 간격(각도)
        float angle = 360.0f / maxPlayer;

        // maxPlayer 만큼 반복
        for (int i = 0; i < maxPlayer; i++)
        {
            // spawnCenter 회전 (i * angle) 만큼
            spawnCenter.eulerAngles = new Vector3(0, i*angle, 0);
            // spawnCenter 앞방향으로 2만큼 떨어진 위치 구하자.
            spawnPos[i] = spawnCenter.position + spawnCenter.forward * 2;
            //// 큐브 하나 생성
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //// 생성된 큐브를 위에서 구한 위치에 놓자.
            //cube.transform.position = spawnPos[i];
        }

        
        //i = i + 36;
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.position = new Vector3(2, 0, 0);
        //transform.rotation = Quaternion.Euler(i, 0, 0); // x, y, z 축에 대해 도씩 회전


        //cube.transform.position = new Vector3(0,0, 0);
        //for (int i = 0; i <= 360; i = i + 36)
        //{
        //    transform.rotation = Quaternion.Euler(60, 60, 60); // x, y, z 축에 대해 도씩 회전
        //    GameObject.CreatePrimitive(PrimitiveType.Cube); // 큐브 생성
        //    Quaternion quaternion = Quaternion.Euler(rotationVector);
        //    transform.rotation = quaternion;
        //}
        //transform.rotation


    }
}
