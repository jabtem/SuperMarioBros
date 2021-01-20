using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D flagrigid;
    public AudioClip[] flagsound;
    AudioSource auido;
    bool flagDown = false;
    bool flagHit = false;

    void Awake()
    {
        //자식 flag의 rigidbody 가져옴
        flagrigid = transform.Find("flag").GetComponent<Rigidbody2D>();
        auido = gameObject.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveFlag();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !flagHit)
        {
            flagDown = true;
            //한번만 호출되게 하기위함
            flagHit = true;
            auido.PlayOneShot(flagsound[0], 1f);
        }
    }


    //자식 flag의 충돌여부 받아오기위함
    public void OnChildCollison()
    {
        flagDown = false;
        auido.PlayOneShot(flagsound[1], 1f);
    }

    void MoveFlag()
    {
        if (flagDown)
        {
            flagrigid.velocity = Vector2.down * 3;
        }
        else
        {
            flagrigid.velocity = Vector2.zero;
        }
    }

    public bool Get_FlagDown()
    {
        return flagDown;
    }
}
