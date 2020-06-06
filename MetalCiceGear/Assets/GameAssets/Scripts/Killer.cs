using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Killer : MonoBehaviour
{
    Enemy enemy;
    Animator anim;
    NavMeshAgent agent;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!enemy.isDead && !Player.instance.GetZombieMode() && !Player.instance.godMode) {
            if (enemy.state == EnemyStates.KILLER)
            {
                if (!Player.instance.IsDisabled())
                {
                    anim.SetTrigger("Shoot");
                    agent.SetDestination(this.transform.position);
                    enemy.LookAtPlayer();
                    Player.instance.DisablePlayer();
                    StartCoroutine(WaitForShoot());
                }
            }
        }
    }

    IEnumerator WaitForShoot()
    {
        yield return new WaitForSeconds(1.75f);
        GetComponent<Enemy>().ShootPistol();
    }
}
