using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalBlock : MonoBehaviour
{
    PlayerCtrl player;
    Animator anim;
    public float bounceForce;//캐릭터와 부딫히면 살짝 튕기는힘
    bool isbounce = false;
    float gravity = 9.8f; //튕겼다가 다시돌아올 힘(중력)
    float bounceTime = 0.0f;
    Vector3 startPos;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();//플레이어상태를확인하기위한 참조
        anim = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (isbounce)
            Bounce();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && transform.position.y > collision.transform.position.y)
        {
            if(player.get_State() >0)
            {
                anim.SetTrigger("break");
            }
            else
            {
                isbounce = true;
            }

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
            bounceTime = 0.0f;
            transform.position = startPos;
        }
    }
}
