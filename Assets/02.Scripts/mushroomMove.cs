using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroomMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform frontCheck;
    bool isCol;
    public float moveSpeed = 3f;
    int direction = 1;
    SpriteRenderer spr;
    public Sprite[] sprites;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        frontCheck = transform.Find("frontCheck").transform;
        spr = transform.Find("mushroom").GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        if (gameObject.tag == "growUp")
            spr.sprite = sprites[0];
        else if (gameObject.tag == "lifeUp")
            spr.sprite = sprites[1];

        anim.SetBool("animStart", true);
        spr.sortingLayerName = "Map";
    }
    void Update()
    {
        isCol = Physics2D.Linecast(this.transform.position, frontCheck.position, 1 << LayerMask.NameToLayer("Ground"));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(anim.GetBool("animStart") == false)
            rigid.velocity = new Vector2(direction * moveSpeed, rigid.velocity.y);

        if (isCol)
            Flip();

        //현재 애니메이션 스테이트의 이름이 MushroomUp이고 애니메이션동작이 끝난경우
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("MushroomUp") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            anim.SetBool("animStart", false);
            spr.sortingLayerName = "Player";
        }
    }

    //방향전환
    void Flip()
    {
        direction *= -1;
        Vector3 postion = frontCheck.localPosition;
        postion.x *= -1;
        frontCheck.localPosition = postion;
    }
}
