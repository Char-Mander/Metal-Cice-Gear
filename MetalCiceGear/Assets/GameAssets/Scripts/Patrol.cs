using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public float waitingTime;
    public float enemySpeed =3.5f;
    public List<Transform> wayPoints = new List<Transform>();

    NavMeshAgent agent;
    Enemy enemy;

    int wpIdex =0;
    bool isMoving = false;
    bool isMoqued = false;
    Coroutine wpStop;
    float moskedWaitgTime;
    float moskedEnemySpeed;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        Init();
    }

    public void Init() {
        isMoving = false;
        wpStop = StartCoroutine(WaitOnWP());
        agent.speed = enemySpeed;
        moskedWaitgTime = waitingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.isDead)
        {
            if (enemy.state == EnemyStates.PATROL)
            {
                Patrolling();
            }
            else
            {
                if (wpStop != null)
                {
                    StopCoroutine(wpStop);
                }
            }
        }
        else {
            StopAllCoroutines();
        }
    }

    public void MosquedEnemy() {
        Init();
        isMoqued = true;
        moskedWaitgTime = waitingTime / 2;
        moskedEnemySpeed = enemySpeed + enemySpeed / 2;
        agent.speed = moskedEnemySpeed;
        
    }

    void Patrolling() {
        if (wpIdex < wayPoints.Count )
        {
            if (isMoving && agent.remainingDistance <= agent.stoppingDistance) {
                wpStop = StartCoroutine(WaitOnWP());
                isMoving = false;
            }
        }
        else {
            wpIdex = 0;
        }
    }

    IEnumerator WaitOnWP() {
        yield return new WaitForSeconds(moskedWaitgTime);
        agent.SetDestination(wayPoints[wpIdex].position);
        if (isMoqued) {
            int rndNumber = Random.Range(0, wayPoints.Count);
            while (wpIdex == rndNumber) {
                rndNumber = Random.Range(0, wayPoints.Count);
            }
            wpIdex = rndNumber;
        }
        else {
            wpIdex++;
        }
        isMoving = true;
    }

}
