using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator anim;
    bool getScore = false;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //애니메이션이 끝난후
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("coinUpDown") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            if(!getScore)
            {
                getScore = true;
                ScoreManager.instance.GetScore(transform.position,200);
                Destroy(gameObject);
            }

        }
    }
}
