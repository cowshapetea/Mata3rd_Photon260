using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    // 채팅을 총괄하는 객체
    ChatClient chatclient;

    // 채팅 입력 UI
    public TMP_InputField inputChat;

    // 채팅 채널
    string currchannel = "메타";

    // 스크롤 뷰의 Content
    public RectTransform trContent;

    // ChatItem Prefab
    public GameObject chatItemFactory;

    // Start is called before the first frame update
    void Start()
    {
        // 채팅 내용을 작성하고 엔터를 쳤을때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSubmit);

        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        // 채팅 서버에서 오는 응답을 수신하기 위해서 계속 호출 해줘야 한다.
        if (chatclient != null)
        {
            chatclient.Service();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // 채널에서 나가자
            //string[] channels = { "전체" };
            string[] channels = { currchannel };
            
            chatclient.Unsubscribe(channels);
        }
    }

    void Connect()
    {
        // 포톤 설정을 가져오자
        AppSettings photonSettings = PhotonNetwork.PhotonServerSettings.AppSettings;

        // 위 설정을 가지고 ChatAppSettings 셋팅
        ChatAppSettings chatAppSettings = new ChatAppSettings();
        chatAppSettings.AppIdChat = photonSettings.AppIdChat;
        chatAppSettings.AppVersion = photonSettings.AppVersion;
        chatAppSettings.FixedRegion = photonSettings.FixedRegion;
        chatAppSettings.NetworkLogging = photonSettings.NetworkLogging;
        chatAppSettings.Protocol = photonSettings.Protocol;
        chatAppSettings.EnableProtocolFallback = photonSettings.EnableProtocolFallback;
        chatAppSettings.Server = photonSettings.Server;
        chatAppSettings.Port = (ushort)photonSettings.Port;
        chatAppSettings.ProxyServer = photonSettings.ProxyServer;

        // ChatClient 만들자
        chatclient = new ChatClient(this);
        // 닉네임
        chatclient.AuthValues = new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName);
        // 연결시도
        chatclient.ConnectUsingSettings(chatAppSettings);
    }

    void OnSubmit(string s)
    {
        // 만약 s 의 길이가 0이면 함수를 나가자.
        if (s.Length == 0) return;

        // 채팅을 보내자.
        chatclient.PublishMessage(currchannel, s);

        // 채팅 입력란 초기화
        inputChat.text = "";
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        //throw new System.NotImplementedException();
    }
    // 채팅 서버에 접속이 성공하면 호출되는 함수
    public void OnConnected()
    {
        print("채팅 서버 접속 성공");
        // "전체" 채널에 들어가자 (구독)
        //chatclient.Subscribe("전체");
        chatclient.Subscribe(currchannel);
        // 채널에서 
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
    }

    // 특정 채널에 다른 사람(나)이 메시지를 보내고 나한테 응답이 올 때 호출되는 함수
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            print(senders[i] + " : " + messages[i]);

            // ChatItem 생성 (Content 의 자식으로)
            GameObject go = Instantiate(chatItemFactory, trContent);
            // 생성된 게임오브젝트에서 ChatItem 컴포넌트 가져온다
            ChatItem chatItem = go.GetComponent<ChatItem>();
            // 가져온 컴포넌트에서 SetText 함수 실행
            chatItem.SetText(senders[i] + " : " + messages[i]);
        }

       
        //throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    // 채팅 채널에 접속이 성공했을 때 들어오는 함수
    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            print(channels[i] + " 채널에 접속을 성공했습니다.");
        }
        //throw new System.NotImplementedException();
    }

    // 채팅 채널에서 나갔을때 들어오는 함수
    public void OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            print(channels[i] + " 채널에서 나갔습니다.");
        }
        //throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }
}
