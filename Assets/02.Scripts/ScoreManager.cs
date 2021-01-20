using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ScoreManager instance;
    public GameObject score_200;
    public GameObject score_1000;
    public GameObject score_100;
    public GameObject score_400;
    List<GameObject> scores_200 = new List<GameObject>();
    List<GameObject> scores_1000 = new List<GameObject>();
    List<GameObject> scores_100 = new List<GameObject>();
    List<GameObject> scores_400 = new List<GameObject>();

    void Awake()
    {
        if (ScoreManager.instance == null)
        {
            ScoreManager.instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateScores100(2);
        CreateScores200(2);
        CreateScores400(2);
        CreateScores1000(2);
    }


    public void GetScore(Vector3 pos, int score)
    {
        string funcName = "GetScore" + score;
        SendMessage(funcName, pos);
    }

    // Update is called once per frame
    void CreateScores200(int scorecnt)
    {
        for (int i = 0; i < scorecnt; i++)
        {
            GameObject scoreObject = Instantiate(score_200) as GameObject;
            scoreObject.transform.parent = transform;
            scoreObject.SetActive(false);
            scores_200.Add(scoreObject);
        }
    }
    void CreateScores1000(int scorecnt)
    {
        for (int i = 0; i < scorecnt; i++)
        {
            GameObject scoreObject = Instantiate(score_1000) as GameObject;
            scoreObject.transform.parent = transform;
            scoreObject.SetActive(false);
            scores_1000.Add(scoreObject);
        }
    }
    void CreateScores100(int scorecnt)
    {
        for (int i = 0; i < scorecnt; i++)
        {
            GameObject scoreObject = Instantiate(score_100) as GameObject;
            scoreObject.transform.parent = transform;
            scoreObject.SetActive(false);
            scores_100.Add(scoreObject);
        }
    }

    void CreateScores400(int scorecnt)
    {
        for (int i = 0; i < scorecnt; i++)
        {
            GameObject scoreObject = Instantiate(score_400) as GameObject;
            scoreObject.transform.parent = transform;
            scoreObject.SetActive(false);
            scores_400.Add(scoreObject);
        }
    }

    public GameObject GetScore200(Vector3 pos)
    {
        GameObject reqScore = null;

        for (int i = 0; i < scores_200.Count; i++)
        {
            if (scores_200[i].activeSelf == false)
            {
                reqScore = scores_200[i];
                break;
            }
        }
        //초기설정 탄환수보다 부족할경우 오브젝트 추가생성
        if (reqScore == null)
        {
            GameObject newBullet = Instantiate(score_200) as GameObject;
            newBullet.transform.parent = transform;
            scores_200.Add(newBullet);
            reqScore = newBullet;
        }

        reqScore.SetActive(true);
        reqScore.transform.position = pos;


        return reqScore;
    }

    public GameObject GetScore1000(Vector3 pos)
    {
        GameObject reqScore = null;

        for (int i = 0; i < scores_1000.Count; i++)
        {
            if (scores_1000[i].activeSelf == false)
            {
                reqScore = scores_1000[i];
                break;
            }
        }
        //초기설정 탄환수보다 부족할경우 오브젝트 추가생성
        if (reqScore == null)
        {
            GameObject newBullet = Instantiate(score_1000) as GameObject;
            newBullet.transform.parent = transform;
            scores_1000.Add(newBullet);
            reqScore = newBullet;
        }

        reqScore.SetActive(true);
        reqScore.transform.position = pos;


        return reqScore;
    }

    public GameObject GetScore400(Vector3 pos)
    {
        GameObject reqScore = null;

        for (int i = 0; i < scores_400.Count; i++)
        {
            if (scores_400[i].activeSelf == false)
            {
                reqScore = scores_400[i];
                break;
            }
        }
        //초기설정 탄환수보다 부족할경우 오브젝트 추가생성
        if (reqScore == null)
        {
            GameObject newBullet = Instantiate(score_400) as GameObject;
            newBullet.transform.parent = transform;
            scores_400.Add(newBullet);
            reqScore = newBullet;
        }

        reqScore.SetActive(true);
        reqScore.transform.position = pos;


        return reqScore;
    }

    public GameObject GetScore100(Vector3 pos)
    {
        GameObject reqScore = null;

        for (int i = 0; i < scores_100.Count; i++)
        {
            if (scores_100[i].activeSelf == false)
            {
                reqScore = scores_100[i];
                break;
            }
        }
        //초기설정 탄환수보다 부족할경우 오브젝트 추가생성
        if (reqScore == null)
        {
            GameObject newBullet = Instantiate(score_100) as GameObject;
            newBullet.transform.parent = transform;
            scores_100.Add(newBullet);
            reqScore = newBullet;
        }

        reqScore.SetActive(true);
        reqScore.transform.position = pos;


        return reqScore;
    }
}
