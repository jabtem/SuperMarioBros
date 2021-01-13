using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    public float maxSpeed =5f;
    public float moveForce = 100f;
    public float jumpForce = 500f;

    public int state = 0;//캐릭터가 현재 작은상태,큰상태인지 구분하기위한변수 0:기본, 1: 버섯먹음

    //긴점프
    bool isLongJump = false;

    //빠른달리기
    bool fastRun = false;

    //방향구분
    bool dirRight = true;//기본방향이 오른쪽이므로


    Rigidbody2D rigid;
    SpriteRenderer spr;
    public Collider2D[] cols;

    bool jump = false;

    //지면체크
    Transform groundCheck;
    bool isGround = false;

    //정면충돌체크
    Transform frontCheck;
    public bool isFront = false;

    //애니메이터
    Animator anim;

    //오디오
    AudioSource audio;
    public AudioClip jumpsound;

    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        groundCheck = GameObject.FindGameObjectWithTag("groundCheck").transform;
        frontCheck = transform.Find("frontCheck").transform;
        audio = gameObject.GetComponent<AudioSource>();
    }



    // Update is called once per frame
    void Update()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp(viewPos.x,0.02f,0.98f);
        Vector3 wordlPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = wordlPos;

        //좌우방향키 입력시 왼쪽으로이동시엔 스프라이트 방향전환
        //if(Input.GetButton("Horizontal"))
        //    spr.flipX = (Input.GetAxisRaw("Horizontal") == -1);

        //레이어가 Ground인 대상과 상하 충돌반응이 있을 경우
        isGround = Physics2D.Linecast(this.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //레이어가 Ground인 대상과 좌우 충돌반응이 있을 경우
        isFront = Physics2D.Linecast(this.transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        //캐릭터가 공중에서 떨어지는중에 땅에 닿았을때
        if (rigid.velocity.y<0 &&isGround)
            anim.SetBool("isJump", false);

        if (Input.GetButtonDown("Jump") && isGround)
        {
            anim.SetBool("isJump", true);
            jump = true;
        }



        if (Input.GetButtonDown("Jump"))
            isLongJump = true;
        else if (Input.GetButtonUp("Jump"))
            isLongJump = false;

        if (Input.GetKeyDown(KeyCode.X))
            fastRun = true;
        else if (Input.GetKeyUp(KeyCode.X))
            fastRun = false;

        Debug.Log("Velocityx : " + rigid.velocity.x);

    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(h));
        anim.SetFloat("Velocity", Mathf.Abs(rigid.velocity.x));

        if (h * rigid.velocity.x < maxSpeed &&!isFront)
            rigid.AddForce(Vector2.right * h * moveForce);

        if (h > 0 && !dirRight)
        {
            Flip();
            if (Mathf.Abs(rigid.velocity.x) > 2)
            {
                anim.SetTrigger("Flip");
            }
        }

        else if (h < 0 && dirRight)
        {
            Flip();
            if (Mathf.Abs(rigid.velocity.x) > 2)
            {
                anim.SetTrigger("Flip");
            }
        }
            

        //최대속도보다 빨라지지않도록 속도를 제한함
        if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
            rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * maxSpeed, rigid.velocity.y);

        if(jump)
        {
            rigid.AddForce(Vector2.up*jumpForce);
            jump = false;
        }

        if (isLongJump && rigid.velocity.y > 0)
            rigid.gravityScale = 1.0f;
        else
            rigid.gravityScale = 2.5f;

        if (fastRun)
            maxSpeed = 10f;
        else
            maxSpeed = 5f;

    }

    void Flip()
    {
        dirRight = !dirRight;
        Vector3 sacle = this.transform.localScale;
        sacle.x *= -1;
        this.transform.localScale = sacle;
    }

    //플레이어의 상태반환
    public int get_State()
    {
        return state;
    }

    void PlayJumpSound()
    {
        audio.PlayOneShot(jumpsound, 1f);
    }
}
