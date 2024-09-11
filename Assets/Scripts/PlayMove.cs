using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Photon.Pun;
using Unity.Mathematics;
using TMPro;
using UnityEngine.Rendering.UI;

public class PlayMove : MonoBehaviourPun, IPunObservable //MonoBehaviour를 Pun 없이 쓸 경우에 privite Photonview photonview; void start() photonview = GetComponent<PhotonView>(); 해야함

{
    // 이동 속력
    public float moveSpeed = 5f;

    // 중력
    float gravity = -9.81f;
    // y 속력
    float yVelocity;

    // 점프 초기 속력
    public float jumpPower = 3;

    // 캐릭터 컨트롤러
    CharacterController cc;

    // 카메라
    public GameObject cam;

    // 서버에서 넘어오는 위치값
    Vector3 receivePos;
    // 서버에서 넘어오는 회전값
    Quaternion receiveRot = Quaternion.identity;

    // 보정 속력
    public float lerpSpeed = 50;

    // ani
    Animator anim;

    // AD 키를 입력 받을 변수
    float h;
    // WS 키를 입력 받을 변수
    float v;

    // LookPos
    public Transform lookPos;

    // 닉네임 UI
    public TMP_Text nickName;

    void Start()
    {
        // 캐릭터 컨트롤러 가져오자
        cc = GetComponent<CharacterController>();

        // 내 것일 때만 카메라를 활성화
        cam.SetActive(photonView.IsMine); // 아래와 같음
        //if (photonView.IsMine) // 위와 같음
        //{
        //    cam.SetActive(true);
        //}

        if (photonView.IsMine)
        {
            // 마우스 잠그기
            Cursor.lockState = CursorLockMode.Locked;
        }

        //애니메이터 가져오기
        anim = GetComponentInChildren<Animator>();

        //nickName.text = PhotonNetwork.NickName; // 이러면 자기 이름으로 다른 사람도 덮어씌움

        // 닉네임 UI에 해당 캐릭터의 주인의 닉네임 설정
        nickName.text = photonView.Owner.NickName;
        
    }

    void Update()
    {
        // 내 것일 때만 컨틀롤 하자!
        if (photonView.IsMine)
        {
            // 마우스의 lockmode가 none 이면 (마우스 포인터 활성화 되어있다면) 함수 나가자
            if (Cursor.lockState == CursorLockMode.None)
            {
                return;
            }

            // 1. 키보드 WASD 키 입력을 받자.
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // 2. 방향을 정하자.
            //Vector3 dir = transform.TransformDirection(new Vector3(h, 0, v).normalized);

            //Vector3 dir = new Vector3(h, 0, v);

            //Vector3 dirH = Vector3.right * h;
            //Vector3 dirV = Vector3.forward * v;
            //Vector3 dir = dirH + dirV;
            //dir.Normalize();

            // 자신의 방향을 기준으로 dir 변경 1 (기존 식은 플레이어 회전 관계없이 월드 기준으로 앞뒤로만 움직임)
            //dir = transform.TransformDirection(dir);

            // 자신의 방향을 기준으로 dir 변경 2
            Vector3 dirH = transform.right * h;
            Vector3 dirV = transform.forward * v;
            Vector3 dir = dirH + dirV;
            dir.Normalize();

            // 만약에 땅에 있으면 yVelocity  를 0 으로 초기화
            if (cc.isGrounded)
            {
                yVelocity = 0;
            }

            // 만약에 Space 바를 누르면
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // yVelocity 를 jumpPower 로 설정
                yVelocity = jumpPower;
            }

            // yVelocity 값을 중력에 의해서 변경시키자.
            yVelocity += gravity * Time.deltaTime;

            #region 물리적인 점프 아닌것
            // dir.y 에 yVelocity 값을 셋팅
            dir.y = yVelocity;

            // 3. 그 방향으로 움직이자.
            //transform.position += (dir * moveSpeed * Time.deltaTime);
            //transform.position += dir * 5 * Time.deltaTime;
            cc.Move(dir * moveSpeed * Time.deltaTime);
            #endregion

            #region 물리적인 점프
            //dir = dir * moveSpeed;
            //dir.y = yVelocity;
            //cc.Move(dir * Time.deltaTime);
            #endregion

            //// anim 을 이용해서 h, v 값을 전달 -- 아닐때도 적용 하도록 밖으로 이동
            //anim.SetFloat("DirH", h);
            //anim.SetFloat("DirV", v);
        }
        // 나의 Player 아니라면
        else
        {
            // 위치 보정
            transform.position = Vector3.Lerp(transform.position,receivePos,Time.deltaTime * lerpSpeed);
            // 회전 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, Time.deltaTime * lerpSpeed);
        }
        // anim 을 이용해서 h, v 값을 전달
        anim.SetFloat("DirH", h);
        anim.SetFloat("DirV", v);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만약에 내가 데이터를 보낼 수 있는 상태라면(내 것이라면)
        if (stream.IsWriting)
        {
            // 나의 위치 값을 보낸다
            stream.SendNext(transform.position);
            // 나의 회전값을 보낸다
            stream.SendNext(transform.rotation);
            // 나의 h 값
            stream.SendNext(h);
            // 나의 v 값
            stream.SendNext(v);
            // LookPos 의 위치값을 보낸다.
            stream.SendNext(lookPos.position);

        }
        // 데이터를 받을 수 있는 상태라면 (내 것이 아니라면)
        else if(stream.IsReading)
        {
            // 위치 값을 받자. 순서 중요함. 위치 먼저 보냈으면 위치 먼저
            //transform.position = (Vector3)stream.ReceiveNext();
            receivePos = (Vector3)stream.ReceiveNext();

            // 회전 값을 받자.
            //transform.rotation = (quaternion)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            // 서버에서 전달 되는 h 값 받자
            h = (float)stream.ReceiveNext();
            // 서버에서 전달 되는 v 값 받자
            v = (float)stream.ReceiveNext();
            // LookPos 의 위치값을 받자.
            lookPos.position = (Vector3)stream.ReceiveNext();
        }
    }
}
