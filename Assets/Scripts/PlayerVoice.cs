using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoice : MonoBehaviour
{
    // Photon Voice View
    public PhotonVoiceView pvv;
    // Image Speaker
    public GameObject imgSpeaker;
    // Image Speech Bubble
    public GameObject imgSpeechBubble;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 만약에 내가 말하고 있다면
        // imgSpeechBubble 활성화
        // 그렇지 않으면 비활성화
        imgSpeechBubble.SetActive(pvv.IsRecording);

        // 만약에 말하는 소리가 들린다면
        // imgSpeaker 활성화
        // 그렇지 않으면 비활성화
        imgSpeaker.SetActive(pvv.IsSpeaking);
    }
}
