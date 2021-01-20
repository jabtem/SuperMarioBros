using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childFlag : MonoBehaviour
{
    bool groundHit = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        //땅에 깃발이 닿으면 충돌유무를 부모에게 알림
        if(collision.gameObject.tag == "ground"&&!groundHit)
        {
            Flag flag = gameObject.transform.parent.GetComponent<Flag>();
            flag.OnChildCollison();
            groundHit = true;
        }

    }
}
