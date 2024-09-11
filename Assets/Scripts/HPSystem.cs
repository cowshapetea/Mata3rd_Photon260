using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviourPun
{
    // 최대 HP
    public float maxHP;
    // 현재 HP
    float currHP;
    // HPBar Image
    public Image HpBar;
    // HP가 0이 되었을때 호출되는 함수를 담을 변수
    public Action onDie;

    // Start is called before the first frame update
    void Start()
    {
        InitHP();
    }

    public void InitHP()
    {
        // 현재 HP를 최대 HP로 설정
        currHP = maxHP;
    }
    void Update()
    {
        //체력 닳기 테스트
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    UpdateHP(-1);
        //}
    }

    public void UpdateHP(float value)
    {
        photonView.RPC(nameof(RpcUpdateHP), RpcTarget.All, value);
    }

    // HP 갱신 함수
    [PunRPC]
    public void RpcUpdateHP(float value)
    {
        // 현재 HP 를 value 만큼 더하자.
        currHP += value;

        // HPBar Image 갱신
        if (HpBar != null)
        { 
             HpBar.fillAmount = currHP / maxHP;
        }
        // 만약에 현재 HP 가 0보다 작거나 같으면
        if (currHP <= 0)
        {
            print(gameObject.name + "의 HP 가 0 입니다.");

            if (onDie != null)
            {
                onDie();
            }
            else
            {
                // 기본적으로 죽음 처리 되는것 실행
                Destroy(gameObject);
            }

            //if (gameObject.layer == LayerMask.NameToLayer("Player"))
            //{
            //    // 플레이어 죽음 처리
            //    PlayerFire pf = GetComponentInParent<PlayerFire>();
            //    pf.OnDie();
            //}
            //else if (gameObject.layer == LayerMask.NameToLayer("ObstacleCube"))
            //{
            //    // 큐브 죽음 처리
            //    ObstacleCube cube = GetComponent<ObstacleCube>();
            //    cube.OnDie();
            //}
            //else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            //{
            //    // 적 죽음 처리
            //}

        }
    }
    //[RPC]
    //public void UpdateHP(float value)
    //{
    //    // 현재 HP 를 value 만큼 더하자.
    //    currHP += value;

    //    // HPBar Image 갱신
    //    HpBar.fillAmount = currHP / maxHP;

    //    // 만약에 현재 HP 가 0보다 작거나 같으면
    //    if (currHP <= 0)
    //    {

    //    }
    //}
}
