using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform player;
    public Vector2 maxPos;
    public Vector2 minPos;
    Vector2 startPos;//카메라의 시작포지션
    public Vector2 firstminPos;
    public float xMargin;      // 카메라가 Player의 X좌표로 이동하기 전에 체크하는 Player와 Camera의 거리 값
    
    //public float yMargin = 1f;      // 카메라가 Player의 Y좌표로 이동하기 전에 체크하는 Player와 Camera의 거리 값

    public float xSmooth = 8f;      // 타겟이 X축으로 이동과함께 얼마나 스무스하게 카메라가 따라가야 하는지 설정 값.
   // public float ySmooth = 8f;		// 타겟이 Y축으로 이동과함께 얼마나 스무스하게 카메라가 따라가야 하는지 설정 값. 


    void Awake()
    {
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        firstminPos = minPos;
    }
    bool CheckXmargin()
    {

        //카메라의 스크린 좌표
        //Vector3 camScreenPos = Camera.main.WorldToScreenPoint(transform.position);  
        ////플레이어의 스크린 좌표
        //Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;

        //return Mathf.Abs(camScreenPos.x - playerScreenPos.x) > xMargin;
    }

    //카메라 y축이동은 필요X
    //bool CheckYMargin()
    //{
    //    return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    //}
    // Start is called before the first frame update

    void FixedUpdate()
    {
        TrackPlayer();
    }
    void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // Mathf.Lerp(a,b,c) : 선형보간법(Linear Interpolation)함수로서 a는 start값, b는 finish값 c는 factor로서 a+(b-a)*c 값을 반환
        if (CheckXmargin())
        {
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.fixedDeltaTime);
        }
       
        minPos.x = (firstminPos.x + Mathf.Abs(transform.position.x - startPos.x));

        //if (CheckYMargin())
        //    targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.fixedDeltaTime);

        targetX = Mathf.Clamp(targetX, minPos.x, maxPos.x);
        //targetY = Mathf.Clamp(targetY, minPos.y, maxPos.y);

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
