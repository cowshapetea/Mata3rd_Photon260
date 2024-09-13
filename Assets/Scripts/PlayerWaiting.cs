using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerWaiting : MonoBehaviourPun
{
    // 닉네임 UI
    public TMP_Text nickName;

    // Start is called before the first frame update
    void Start()
    {
        // 닉네임 UI 에 표시
        nickName.text = photonView.Owner.NickName;

        // 내것이라면
        if (photonView.IsMine)
        {
            // WaitingMgr 에게 나를 알려주자.
            WaitingMgr.instance.myPlayer = this;
            //GameObject go = GameObject.Find("WaitingManager");
            //WaitingMgr mgr = go.GetComponent<WaitingMgr>();
            //mgr.myPlayer = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetReady(bool isReady)
    {
        photonView.RPC(nameof(RpcSetReady), RpcTarget.AllBuffered, isReady);
    }

    [PunRPC]
    void RpcSetReady(bool isReady)
    {
        if (isReady)
        {
            // nickName 색을 검은색으로 하자
            nickName.color = Color.black;

        }
        else
        {
            nickName.color = Color.white;
        }
        //// 만약에 모두 Ready 를 했다면
        //// GameScene 으로 이동

        // 모두 Ready 했는지 체크
        WaitingMgr.instance.CheckAllReady(isReady);

    }

}
