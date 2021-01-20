using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    public float maxSpeed =5f;
    public float moveForce = 100f;
    public float jumpForce = 10f;

    public int state = 0;//캐릭터가 현재 작은상태,큰상태인지 구분하기위한변수 0:기본, 1: 버섯먹음

    public bool isalive = true;//살아있는지여부

    //긴점프
    bool isLongJump = false;

    //빠른달리기
    bool fastRun = false;

    //방향구분
    bool dirRight = true;//기본방향이 오른쪽이므로


    bool canMove = true;//버섯먹으면 잠시 정지시켰다가 다시움직이게할것이므로 

    Rigidbody2D rigid;
    SpriteRenderer spr;
    public Collider2D[] cols;

    bool jump = false;
    float h;

    //지면체크
    Transform groundCheck;
    bool isGround = false;
    
    //깃발잡았을때 아래로내려가게함
    bool downmove = false;

    //정면충돌체크
    Transform frontCheck;
    public bool isFront = false;

    //애니메이터
    Animator anim;

    bool flagEnd = true;

    public bool autoMove = false;

    //오디오
    AudioSource audio;

    public AudioClip[] EffectSound;

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

        if(state == 0)
        {
            cols[0].enabled = true;
            cols[1].enabled = false;
        }
        else if(state ==1)
        {
            cols[0].enabled = false;
            cols[1].enabled = true;
        }

        //좌우방향키 입력시 왼쪽으로이동시엔 스프라이트 방향전환
        //if(Input.GetButton("Horizontal"))
        //    spr.flipX = (Input.GetAxisRaw("Horizontal") == -1);

        //레이어가 Ground인 대상과 상하 충돌반응이 있을 경우, 캐릭터가 위에서 떨어지고있을때만 체크 
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




    }

    void FixedUpdate()
    {
        if(canMove&&!autoMove)
        {
            h = Input.GetAxisRaw("Horizontal");
        }


        anim.SetFloat("Speed", Mathf.Abs(h));
        anim.SetFloat("Velocity", Mathf.Abs(rigid.velocity.x));

        if (h * rigid.velocity.x < maxSpeed &&!isFront&&canMove)
            rigid.AddForce(Vector2.right * h * moveForce);

        if (h > 0 && !dirRight)
        {
            if (Mathf.Abs(rigid.velocity.x) > 2)
            {
                anim.SetTrigger("Flip");
            }
            Flip();

        }

        else if (h < 0 && dirRight)
        {
            if (Mathf.Abs(rigid.velocity.x) > 2)
            {
                anim.SetTrigger("Flip");
            }
            Flip();

        }
            

        //최대속도보다 빨라지지않도록 속도를 제한함
        if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
            rigid.velocity = new Vector2(Mathf.Sign(rigid.velocity.x) * maxSpeed, rigid.velocity.y);

        if(jump)
        {
            rigid.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
            jump = false;
        }

        if(isalive)
        {
            if (isLongJump && rigid.velocity.y > 0)
                rigid.gravityScale = 2.5f;
            else
                rigid.gravityScale = 4;
        }


        if (fastRun)
            maxSpeed = 10f;
        else
            maxSpeed = 5f;

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("growUp")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
        {
            canMove = true;
            anim.SetInteger("State", state);
        }

        if (downmove)
        {
            rigid.velocity = Vector2.down*3;
        }

        //깃발다내려오고 자동으로 이동
        if(autoMove)
        {
            h = 1f;
        }
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

    void PlaySound(int i)
    {
         audio.PlayOneShot(EffectSound[i], 1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "growUp")
        {
            canMove = false;
            anim.SetTrigger("GrowUp");
            state = 1;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && gameObject.transform.position.y <= collision.transform.position.y)
        {
            switch(state)
            {
                case 0:
                    anim.SetTrigger("Die");
                    PlaySound(4);
                    cols[0].isTrigger = true;
                    isalive = false;
                    rigid.gravityScale = 0;
                    rigid.velocity = Vector2.zero;
                    StartCoroutine(Die());
                    break;
                case 1:
                    anim.SetTrigger("Damaged");
                    break;
            }

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //깃발에 닿았을때
        if(collision.gameObject.tag == "Flag")
        {
            //마리오를 플래그의 자식으로만듬
            GameObject Flag = GameObject.FindGameObjectWithTag("Flag");
            gameObject.transform.parent = Flag.transform;
            h = 0;
            //중력을안받기위해 잠시 죽은거로 처리
            isalive = false;
            canMove = false;
            downmove = true;
            rigid.velocity = Vector2.zero;

            anim.SetTrigger("Flagdown");
            rigid.gravityScale = 0;
            gameObject.transform.localPosition = new Vector3(-0.5f,gameObject.transform.localPosition.y, 0);
        }
        else if(collision.gameObject.tag == "Exit")
        {
            gameObject.SetActive(false);
            canMove = false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //깃발에 닿고있는중
        if (collision.gameObject.tag == "Flag")
        {
            Flag flag = GameObject.FindGameObjectWithTag("Flag").GetComponent<Flag>();
            if (!flag.Get_FlagDown()&&flagEnd)
            {
                flagEnd = false;
                gameObject.transform.localPosition = new Vector3(0.5f, gameObject.transform.localPosition.y, 0);
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                anim.speed = 0;
                anim.SetTrigger("FlagEnd");
                StartCoroutine(MarioRelease());
            }

        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);
        rigid.gravityScale = 2.5f;
        rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
        
    }

    //마리오를 깃발의 자식에서 해제
    IEnumerator MarioRelease()
    {
        yield return new WaitForSeconds(0.5f);
        anim.speed = 1;
        gameObject.transform.parent = null;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        canMove = true;
        downmove = false;
        isalive = true;

        //자동으로 출구까지 걸어가게함
        autoMove = true;
    }
}
