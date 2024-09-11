using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    Animator anim;

    // 바라볼 대상
    public Transform trLook;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // 해당 위치를 바라보게 설정
        anim.SetLookAtPosition(trLook.position);
        // 바라보게 하는 IK 기능의 가중치를 주자.
        anim.SetLookAtWeight(1,1);

    }
}
