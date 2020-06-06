using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ConfusedStates {Shocked, OnTheWay, Ignoring}
public class Confused : MonoBehaviour
{
    public float detectProbability = 70;
    public float shockTime;
    public GameObject confusedObj;
    public float calculateProbabilityCadency = 10;
    [HideInInspector]
    public bool probabilityCalculated = false;

    Enemy enemy;
    NavMeshAgent agent;
    Player player;
    ConfusedStates confusedState;
    bool[] probabilityArray = new bool[10];
    bool isMoving = false;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        player = Player.instance;
        FillProbabilityArray();
        Init();
    }

    public void Init()
    {
        confusedState = ConfusedStates.Shocked;
        confusedObj.SetActive(false);
    }

    void Update()
    {
        if (!enemy.isDead)
        {
                if (enemy.state == EnemyStates.CONFUSED)
                {
                    if (confusedState == ConfusedStates.Shocked && !probabilityCalculated)
                    {
                        StartCoroutine(CanCheckPlayerAgain());
                        confusedObj.SetActive(true);
                        confusedObj.GetComponent<AudioSource>().Play();
                    }
                    else if (confusedState == ConfusedStates.OnTheWay)
                    {
                        StartCoroutine(DetectingPlayer());
                    }
                    else if (confusedState == ConfusedStates.Ignoring)
                    {
                        StartCoroutine(Shock());
                    }
                }
        }
        else
        {
            confusedObj.SetActive(false);
        }
    }

    IEnumerator DetectingPlayer()
    {
        yield return new WaitForSeconds(shockTime);
        if (enemy.state == EnemyStates.CONFUSED)
        {
            Player.instance.CancelZombieMode();
            GetComponent<Patrol>().MosquedEnemy();
            enemy.state = EnemyStates.ALERT;
            Init();
        }

    }

    IEnumerator Shock()
    {
        agent.SetDestination(enemy.transform.position);
        GetComponent<Enemy>().LookAtPlayer();
        yield return new WaitForSeconds(shockTime);
        if (enemy.state == EnemyStates.CONFUSED)
        {
            enemy.state = EnemyStates.PATROL;
            GetComponent<Patrol>().Init();
            isMoving = false;
            Init();
        }
    }

    IEnumerator CanCheckPlayerAgain()
    {
        probabilityCalculated = true;
        yield return new WaitForSeconds(2);
        if (!CalculateDetectionProbability()) confusedState = ConfusedStates.OnTheWay;
        else confusedState = ConfusedStates.Ignoring;
        yield return new WaitForSeconds(calculateProbabilityCadency);
        probabilityCalculated = false;
    }

    void FillProbabilityArray()
    {
        float probabilityValue = (detectProbability / 100) * probabilityArray.Length;
        for (int i=0; i<probabilityArray.Length; i++)
        {
            if (i < probabilityValue) probabilityArray[i] = true;
            else probabilityArray[i] = false;
        }
    }

    bool CalculateDetectionProbability()
    {
        int randomIndex = Random.Range(0,probabilityArray.Length);
        return probabilityArray[randomIndex];
    }
}
