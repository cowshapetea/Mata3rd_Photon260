using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject playerFactory;
    void Start()
    {
        // 플레이어를 생성 (현재 Room 에 접속 되어있는 친구들도 보이게)
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        //PhotonNetwork.Instantiate("AA/Player", Vector3.zero, Quaternion.identity); - AA 폴더 안에 플레이어 있을경우 (Resources 안에 있어야함)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
