using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Delegate : 함수를 담는 변수를 만드는 자료형
public delegate void CallBack();
public delegate void CallBack2(string s);

public class DelegateStudy : MonoBehaviour
{
    CallBack callBack;
    Action action;

    CallBack2 callBack2;
    Action<string> action2;

    void Start()
    {
        //callBack = Update;
        callBack = () =>   // 무명함수 람다식 함수 기능 함. 아래랑 같음
        {
            print(gameObject.name);
        };

        callBack = PrintName; // 위와 같음
        action = PrintName;

        PrintName(); // 아래와 같음
        callBack();  // 위와 같음

        callBack2 = (string s) => // 무명함수 람다식 함수기능 . 아래와 같음
        {
            print(s + " : " + gameObject.name);
        };

        callBack2 = PrintName2; // 위와 같음
        callBack2 += PrintName3;

        //PrintName2("직접 호출");
        callBack2("딜리게이트로 호출");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PrintName()
    {
        print(gameObject.name);
    }

    void PrintName2(string s)
    {
        print(s + " : " + gameObject.name);
    }

    void PrintName3(string s)
    {
        print(s);
    }

}
