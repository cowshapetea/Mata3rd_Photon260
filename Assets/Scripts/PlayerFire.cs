using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
//using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // 큐브 Prefab
    public GameObject cubeFactory;

    // Impact Prefab
    public GameObject impactFactory;

    // RigidBody 로 움직이는 총알 Prefab
    public GameObject bulletFactory;

    // 총알 Prefab
    public GameObject rpcbulletFactory;

    // 총구 Transform
    public Transform firePos;

    // Animator
    Animator anim;

    // 스킬 중심점
    public Transform skillCenter;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        // HPSystem 가져오자.
        HPSystem hPSystem = GetComponentInChildren<HPSystem>();
        // onDie 변수에 OnDie 함수 설정
        hPSystem.onDie = OnDie;
    }

    void Update()
    {
        // 만약에 내 것이라면
        if (photonView.IsMine)        
        {
            // 마우스의 lockmode가 none 이면 (마우스 포인터 활성화 되어있다면) 함수 나가자
            if (Cursor.lockState == CursorLockMode.None)
                return;

            // Hp 0 이 되었으면 총 못 쏘게
            if (isDie) return;

            // 마우스 왼쪽 버튼 누르면
            if (Input.GetMouseButtonDown(0))
            {
                // 총 쏘는 애니메이션 실행 (Fire 트리거 발생)
                //anim.SetTrigger("Fire"); // -- 싱글용 
                photonView.RPC(nameof(SetTriggger),RpcTarget.All, "Fire"); // 멀티용
                // 총알공장에서 총알을 생성, 위치 회전 세팅
                PhotonNetwork.Instantiate("Bullet", firePos.position, firePos.transform.rotation);
                //photonView.RPC(nameof(CreateBullet), RpcTarget.All,firePos.position,Camera.main.transform.rotation);
            }

            // 마우스 가운데 버튼 눌렀을때
            if (Input.GetMouseButtonDown(2))
            {
                photonView.RPC(nameof(CreateBullet), RpcTarget.All,firePos.position,Camera.main.transform.rotation);
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
                    photonView.RPC(nameof(CreateImpact), RpcTarget.All, hit.point); //("CreateImpact")

                    // 부딪힌 놈의 데미지를 주자.
                    //PlayerFire pf = hit.transform.GetComponent<PlayerFire>();
                    HPSystem hpSystem = hit.transform.GetComponentInChildren<HPSystem>();
                    //if (pf != null)
                    //{
                    //    pf.photonView.RPC(nameof(OnDamaged), RpcTarget.All);
                    //}
                    if (hpSystem != null)
                    {
                        hpSystem.UpdateHP(-1);
                    }
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
                //photonView.RPC(nameof(CreateCube), RpcTarget.All,pos);
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

        // R 누르면
        if (Input.GetKeyDown(KeyCode.R))
        {
            int maxBulletCnt = 10;
            float angle = 360.0f / maxBulletCnt;

            for (int i = 0; i < maxBulletCnt; i++)
            {
                #region 싱글플레이 모드
                //// 총알 생성
                //GameObject bullet = Instantiate(bulletFactory);
                //// skillCenter 를 회전 (angle * i) 만큼 회전
                //skillCenter.localEulerAngles = new Vector3(0, angle * i,0);

                //// 생성된 총알을 skillCenter 의 앞방향 2만큼 떨어진 위치 놓자
                //bullet.transform.position = skillCenter.position + skillCenter.forward * 3;

                //// 생성된 총알의 up 방향을 skillcenter의 forward로 하자
                //bullet.transform.up = skillCenter.forward;
                #endregion

                #region 멀티플레이 모드
                // skillCenter 를 회전 (angle * i) 만큼 회전
                skillCenter.localEulerAngles = new Vector3(0, angle * i,0);
                Vector3 pos = skillCenter.position + skillCenter.forward * 3;
                Quaternion rot = Quaternion.LookRotation(Vector3.down, skillCenter.forward);
                PhotonNetwork.Instantiate(bulletFactory.name, pos, rot); // 아래와 같음
                //PhotonNetwork.Instantiate("bullet"); //위와 같음
                #endregion
            }
            // player 머리 위에서 10방향으로 총알 나가는 기능
            // step 1. player 머리 위에 빈오브젝트

            // step 2. 빈오브젝트 기준 10방향, 거리 2 떨어진 위치에 총알 생성

            // step 3. 총알의 up 방향을 앞 방향으로 설정


        }
    }

    [PunRPC]
    void SetTriggger(string parameter)
    {
        anim.SetTrigger(parameter);
    }

    [PunRPC]
    void CreateBullet(Vector3 position, Quaternion rotation)
    {
        Instantiate(rpcbulletFactory, position, rotation);
    }
    [PunRPC]
    void CreateImpact(Vector3 position)
    {
        GameObject impact = Instantiate(impactFactory);
        impact.transform.position = position;
    }
    [PunRPC]
    void OnDamaged()
    {
        HPSystem hpSystem = GetComponentInChildren<HPSystem>();
        hpSystem.RpcUpdateHP(-1);
    }


    //void Fire(Vector3 pos, Vector3 forward)

    [PunRPC]
    void CreateCube(Vector3 position)
    {
        //Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
        Instantiate(cubeFactory, position, Quaternion.identity);
    }

    // 죽었나
    bool isDie;
    public void OnDie()
    {
        isDie = true;
    }
}
