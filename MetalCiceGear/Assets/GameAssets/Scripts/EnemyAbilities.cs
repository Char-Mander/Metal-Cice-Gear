using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilities : MonoBehaviour
{
    public float viewDistance;
    public float viewAngle;
    public LayerMask lm;

    Player player;
    Enemy enemy;
    VisionCone visionC;

    private void Start()
    {
        player = Player.instance;
        enemy = GetComponent<Enemy>();
        visionC = GetComponentInChildren<VisionCone>();
    }

    private void Update()
    {
        if (!enemy.isDead)
        {
            Hearing();
            Looking();
        }
            
    }

    void ActualiceVisonCone() {
        visionC.angle = viewAngle;
        visionC.GetComponent<Projector>().orthographicSize = viewDistance;
        switch (enemy.state) {
            case EnemyStates.PATROL:
                visionC.color = Color.green;
                break;
            case EnemyStates.ALERT:
                visionC.color = Color.yellow;
                break;
            case EnemyStates.KILLER:
                visionC.color = Color.red;
                break;
        }
    }

    public void Looking() {
        ActualiceVisonCone();

        Vector3 direToPlayer = player.transform.position - enemy.transform.position;
        float angle = Vector3.Angle(enemy.transform.forward, direToPlayer);
        if (angle < viewAngle && direToPlayer.magnitude < viewDistance) {
            Vector3 viewDire = (player.chest.transform.position - enemy.head.transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(enemy.head.transform.position, viewDire, out hit, viewDistance, lm)) {
                if (hit.collider.GetComponentInParent<Player>() != null) {
                    Debug.DrawLine(enemy.head.transform.position, hit.transform.position, Color.blue);
                    if (enemy.state == EnemyStates.PATROL && hit.collider.GetComponentInParent<Player>().GetZombieMode() && !enemy.GetComponent<Confused>().probabilityCalculated)
                    {
                        enemy.state = EnemyStates.CONFUSED;
                    }
                    else if (enemy.state == EnemyStates.PATROL && !hit.collider.GetComponentInParent<Player>().GetZombieMode())
                    {
                        enemy.state = EnemyStates.ALERT;
                        GetComponent<Alert>().SetAlertDestination();
                    }
                    else if (enemy.state == EnemyStates.ALERT && enemy.GetComponent<Alert>().alertState != AlertStates.Shocked)
                    {
                        enemy.state = EnemyStates.KILLER;
                    }
                }
            }
        }
    }

    public void Hearing() {
        if (enemy.state != EnemyStates.KILLER) {
            float distPlyEmy = Vector3.Distance(this.transform.position, player.transform.position);
            if (distPlyEmy < player.noise)
            {
                enemy.state = EnemyStates.ALERT;
                if (GetComponent<Alert>().alertState == AlertStates.Shocked)
                {
                    GetComponent<Alert>().SetAlertDestination();
                }
            }
        }
    }
}
