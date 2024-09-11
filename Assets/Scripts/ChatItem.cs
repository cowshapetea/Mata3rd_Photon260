using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatItem : MonoBehaviour
{
    // Text
    TMP_Text ChatText;

    // 매개없는 함수 담을 변수
    public Action onAutoScroll;
    
    private void Awake()
    {
        // Text 컴포넌트 가져오자
        ChatText = GetComponent<TMP_Text>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(string s)
    {
        // 텍스트 갱신
        ChatText.text = s;
        
        // 사이즈 조절 코루틴 실행
        StartCoroutine(UpdateSize());
    }
    IEnumerator UpdateSize()
    {
        yield return null;

        // 텍스트 내용에 맞춰 크기 조절
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, ChatText.preferredHeight);

        yield return null;

        // 만약에 onAutoScroll 에 함수가 들어있다면
        if (onAutoScroll != null)
        {
            onAutoScroll();
        }

        //// ChatView 게임오브젝트 찾자
        //GameObject go = GameObject.Find("ChatView");
        //// 찾은 오브젝트에서 chatManager 컴포넌트 가져오자.
        //ChatManager cm = go.GetComponent<ChatManager>();
        //// 가져온 컴포넌트 AutoScrollBottom 함수 호출
        //cm.AutoScrollBottom();
    }
}
