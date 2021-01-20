using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;
    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity = Vector2.up;
        StartCoroutine("hideScore");
    }
    IEnumerator hideScore()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
