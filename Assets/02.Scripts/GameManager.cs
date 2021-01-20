using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerCtrl player;
    public GameObject music;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        if(!player.isalive)
        {
            //플레이어가 죽으면 배경음악 중단
            AudioSource  audio = music.GetComponent<AudioSource>();
            audio.Stop();
        }

    }
}
