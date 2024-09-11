using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using ColorUtility = UnityEngine.ColorUtility; //있어도 없어도 되는거

public class ChatManager : MonoBehaviourPun
{
    // Input Field
    public TMP_InputField inputChat;

    // chatItem prefab
    public GameObject chatItemFactory;

    // content 의 transform
    public RectTransform trContent;

    // Chatview 의 Transform
    public RectTransform trChatView;

    // 채팅이 추가되기 전의 Content 의 H(높이) 값을 가지고 있는 변수
    float prevContentH;

    // 닉네임 색상
    Color nickNameColor;

    void Start()
    {
        // 닉네임 색상 랜덤하게 설정
        nickNameColor = Random.ColorHSV();
        // inputChat 의 내용이 변경될 때 호출되는 함수 등록
        inputChat.onValueChanged.AddListener(OnvalueChanged);
        // inputChat 의 엔터를 쳤을 때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);
        // inputChat 포커싱을 잃을 때 호출되는 함수 등록
        inputChat.onEndEdit.AddListener(OnEndEdit);
    }

    // Update is called once per frame
    void Update()
    {
        // 만약에 왼쪽 컨트롤키 누르면
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // 마우스 포인터 활성화
            Cursor.lockState = CursorLockMode.None;
        }

        // 만약에 마우스 왼쪽버튼을 눌렀으면
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 포인터가 활성화 되어있다면
            if (Cursor.lockState == CursorLockMode.None)
            {
                // 만약에 UI가 클릭이 되지 않았다면
                //EventSystem.current.IsPointerOverGameObject()
                if (EventSystem.current.IsPointerOverGameObject() == false) // 모바일은 이러면 안됨
                {
                    // 마우스 포인터를 비활성화
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
        
   
    void OnSubmit(string s)
    {
        // 만약에 s 길이가 0  이면 함수를 나가자
        if (s.Length == 0) return;

        // 채팅 내용을 NickName : 채팅 내용
        // "<color=#ffffff"> 원하는 내용 </color>"
        string nick = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" + PhotonNetwork.NickName + "</color>";
        string chat = nick + " : " + s;
        //string chat = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" + PhotonNetwork.NickName + "</color> : " + s;

        // AddChat RPC 함수 호출
        photonView.RPC(nameof(AddChat), RpcTarget.All,chat);

        print("엔터 침 : " + s);

        // 강제로 inputchat을 활성화.
        inputChat.ActivateInputField();

        //AutoScrollBottom();
    }

    // 채팅 추가 함수
    [PunRPC]
    void AddChat(string chat)
    {
        // 새 채팅 추가되기 전 content의 H 값 저장
        prevContentH = trContent.sizeDelta.y;

        // chatItem 하나 만들자 (부모를 chatview 의 content로 하자)
        GameObject go = Instantiate(chatItemFactory, trContent);
        // chatItem 컴포넌트 가져오자.
        ChatItem chatItem = go.GetComponent<ChatItem>();
        // 가져온 컴포넌트의 SetText 함수 실행
        chatItem.SetText(chat);
        // 가져온 컴포넌트의 onAutoScroll 변수에 AutoScrollBottom 을 설정
        chatItem.onAutoScroll = AutoScrollBottom;
        // inputchat 에 있는 내용 초기화
        inputChat.text = "";
    }

    // 채팅 추가 되었을 때 맨밑으로 content 위치를 옮기는 함수
    public void AutoScrollBottom()
    {
        // chatView 의 H 보다 content 의 H 값이 크다면 (스크롤이 가능한 상태라면)
        if (trContent.sizeDelta.y > trChatView.sizeDelta.y)
        {
            //// 이거랑 아래 식이랑 똑같이 다 됨.
            //trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

            // 이전 바닥에 닿아있었다면 // 이거랑 위에 식이랑 똑같이 다 됨.
            if (prevContentH - trChatView.sizeDelta.y <= trContent.anchoredPosition.y)
            {
                // content 의 y 값을 재설정한다.
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trChatView.sizeDelta.y);
            }
        }
    }
    void OnvalueChanged(string s)
    {
        //print("변경 중 : " + s);
    }

    void OnEndEdit(string s)
    {
        print("작성 끝 : " + s);
    }
}
