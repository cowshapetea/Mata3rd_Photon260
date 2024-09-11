using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCube : MonoBehaviour
{

    void Start()
    {
        // HPSystem 가져오자.
        //HPSystem hPSystem = GetComponentInChildren<HPSystem>(); // 자식 해도 자기부터 검색해서 찾을 수 있음
        HPSystem hPSystem = GetComponent<HPSystem>();
        // onDie 변수에 OnDie 함수 설정
        hPSystem.onDie = OnDie;
    }

    void Update()
    {
        
    }

   
    public void OnDie()
    {
        // 크기를 현재 scale 20% 줄이자
        transform.localScale *= 0.8f;
        // 현재 HP 다시 최대 HP
        HPSystem hPSystem = GetComponent<HPSystem>();
        hPSystem.InitHP();

    }
}
