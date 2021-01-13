using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : MonoBehaviour
{
    //이블럭이 보유한 아이템넘버 0:성장버섯, 1:라이프업버섯, 2:불꽃 , 3:별, 4:동전

    SpriteRenderer spr;
    Animator anim;
    public int itemNum;
    public float bounceForce;//캐릭터와 부딫히면 살짝 튕기는힘
    bool isbounce = false;
    bool mushRoomShow = false;
    float gravity = 9.8f; //튕겼다가 다시돌아올 힘(중력)
    float bounceTime = 0.0f;
    //아이템 프리펩 연결할 배열
    public GameObject[] items;

    //블럭상태에따라 스프라이트변경
    public Sprite noDropBlcok;

    Vector3 startPos;

    void Awake()
    {
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("canDrop", true);
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isbounce)
            Bounce();

        //버섯은 블록이 튕기고 돌아간후에 생성한다
        if(!isbounce && mushRoomShow)
        {
            switch (itemNum)
            {
                //성장버섯
                case 0:
                    Instantiate(items[0], new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), Quaternion.identity);
                    mushRoomShow = false;
                    break;
                //라이프업
                case 1:
                    Instantiate(items[1], new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z), Quaternion.identity);
                    mushRoomShow = false;
                    break;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //플레이어가 점프도중 아이템블록과 충돌시, 단 블럭이 캐릭터보다 위에있을경우
        if (collision.gameObject.tag == "Player"&& transform.position.y > collision.transform.position.y && anim.GetBool("canDrop"))
        {
            anim.SetBool("canDrop", false);
            isbounce = true;

        }
    }
    void Bounce()
    {
        float height = (bounceTime * bounceTime * (-gravity) / 2) + (bounceTime * bounceForce);
        transform.position = new Vector3(transform.position.x, startPos.y + height, transform.position.z);
        bounceTime += Time.deltaTime;

        //처음높이보다 더내려갔을경우
        if (height < 0.0f)
        {
            isbounce = false;
            mushRoomShow = true;
            bounceTime = 0.0f;
            transform.position = startPos;
        }
    }
}
