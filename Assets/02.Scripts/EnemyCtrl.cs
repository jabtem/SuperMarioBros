using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;
    Transform frontCheck;
    SpriteRenderer spr;
    Collider2D col;
    Animator anim;
    PlayerCtrl player;
    //오디오클립 배열 
    public AudioClip [] Sound;
    int dir = -1;
    public float moveSpeed = 3f;
    bool isCol;//충돌시
    bool On = false;
    int bounceCnt = 5;
    bool canMove = false;//이동가능여부 뷰포트 안에 들어와야 이동이 가능하게 만들며 죽으면 다시 이동불가로 변경
    int state = 0;//거북이의 상태구분용 0:기본 1:숨음 2:죽음

    void Awake()
    {
        col = gameObject.GetComponent<Collider2D>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();//플레이어의 상태를받아오기위해
    }
    // Update is called once per frame
    void Update()
    {
        //플레이어가 살아있는동안에만 동작
        if(player.isalive)
        {
            if (canMove)
                rigid.velocity = new Vector2(dir * moveSpeed, rigid.velocity.y);

            //뷰포트 내에 들어오고부터 동작
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

            //한번만 작동
            if (viewPos.x <= 1 && !On)
            {
                canMove = true;
                On = true;
            }

            //화면밖으로 사라지면 삭제
            if (viewPos.x < -0.2)
                Destroy(gameObject);
        }
        else
        {
            //플레이어죽으면 애니메이션 정지
            anim.speed = 0;
        }

    }

    void FlipMush()
    {
        //단순 방향전환
        dir *= -1;
    }

    void FlipTurtle()
    {
        //좌우반전
        dir *= -1;
        Vector3 scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //몬스터가 벽에충돌시 방향전환
        if (collision.gameObject.tag != "ground" && collision.gameObject.tag != "Player")
        {
            if(gameObject.tag == "mush")
            {
                FlipMush();
            }
            else if(gameObject.tag == "turtle")
            {
                FlipTurtle();
            }
        }
        //몬스터가 밟혔을때 
        else if (collision.gameObject.tag == "Player" && collision.transform.position.y > transform.position.y && state ==0) //마리오가 몬스터위일때 : 마리오가 몬스터를 밟으면
        {

            //버섯은 한번에죽음
            if(gameObject.tag == "mush")
            {
                col.enabled = false;
                canMove = false;
                //완전히정지
                rigid.velocity = Vector2.zero;
                collision.rigidbody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                anim.SetTrigger("Die");
                rigid.gravityScale = 0;
                ScoreManager.instance.GetScore(transform.position+Vector3.up,100);
                gameObject.GetComponent<AudioSource>().PlayOneShot(Sound[0], 1f);
                StartCoroutine(Delete());
            }
            else if(gameObject.tag =="turtle")
            {
                anim.SetTrigger("Hide");
                state = 1;
                collision.rigidbody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                ScoreManager.instance.GetScore(transform.position + Vector3.up,100);
                gameObject.GetComponent<AudioSource>().PlayOneShot(Sound[0], 1f);
                canMove = false;

            }

        }
        //플레이어와 부딫히고 현재 상태가 1일때
        else if(collision.gameObject.tag == "Player" && state == 1)
        {
            if (collision.transform.position.y > transform.position.y)
                collision.rigidbody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

            gameObject.GetComponent<AudioSource>().PlayOneShot(Sound[1], 1f);
            state = 2;
            anim.SetTrigger("Die");
            dir = transform.position.x - collision.transform.position.x >0 ? 1: -1;
            canMove = true;
            moveSpeed *= 4;
        }


    }

    IEnumerator Delete()
    {
        //1초뒤에 오브젝트 삭제
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
