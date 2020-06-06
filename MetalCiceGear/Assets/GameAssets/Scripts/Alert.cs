using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AlertStates {Normal,Shocked,OnTheWay,Checking }

public class Alert : MonoBehaviour
{
    public float shockTime;
    public float checkingTime;
    public GameObject warningObj;

    Enemy enemy;
    NavMeshAgent agent;
    Player player;
    public AlertStates alertState;
    Vector3 destinationPosi;
    bool isMoving = false;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        player = Player.instance;
        Init();
    }

    public void Init() {
        alertState = AlertStates.Normal;
        destinationPosi = Vector3.zero;
        warningObj.SetActive(false);
    }

    void Update()
    {
        if (!enemy.isDead)
        {
            if (!player.GetZombieMode())
            {
                if (enemy.state == EnemyStates.ALERT)
                {
                    if (alertState == AlertStates.Normal)
                    {
                        warningObj.SetActive(true);
                        warningObj.GetComponent<AudioSource>().Play();

                        alertState = AlertStates.Shocked;
                        StartCoroutine(Shock());
                    }
                    else if (alertState == AlertStates.OnTheWay)
                    {
                        if (isMoving == false && destinationPosi != Vector3.zero)
                        {
                            isMoving = true;
                            agent.SetDestination(destinationPosi);
                        }
                        else if (agent.remainingDistance < agent.stoppingDistance)
                        {
                            alertState = AlertStates.Checking;
                            StartCoroutine(Checking());
                        }

                    }

                }
            }
            else
            {
                enemy.state = EnemyStates.CONFUSED;
                warningObj.SetActive(false);
            }

        }
        else {
            warningObj.SetActive(false);
        }
    }

    public void SetAlertDestination(Vector3 posi) {
        destinationPosi = posi;
    }

    public void SetAlertDestination() {
        destinationPosi = player.transform.position;
    }

    IEnumerator Shock() {

        agent.SetDestination(enemy.transform.position);

        GetComponent<Enemy>().LookAtPlayer();//cambiar

        yield return new WaitForSeconds(shockTime);

        alertState = AlertStates.OnTheWay;
        isMoving = false;

    }

    IEnumerator Checking() {
        yield return new WaitForSeconds(checkingTime);
        if (enemy.state == EnemyStates.ALERT) {
            enemy.state = EnemyStates.PATROL;
            GetComponent<Patrol>().MosquedEnemy();
            Init();
        }

    }
}
