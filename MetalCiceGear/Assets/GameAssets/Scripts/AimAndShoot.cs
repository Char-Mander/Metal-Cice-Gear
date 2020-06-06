using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AimAndShoot : MonoBehaviour
{
    public float cadencia;
    public float shootForce;
    public LayerMask lmAttack;
    public LayerMask aimIgnoreLayer;
    public Transform playerHead;
    public Transform posiDisp;
    public GameObject shootParticle;
    public float shootNoise;

    public Texture2D crossHair;
    public Texture2D redCross;

    NavMeshAgent agent;
    Animator anim;
    bool canShoot = false;
    bool hasReload = true;
    RaycastHit hitEnemyPart;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        AimToTarget();
        if (agent.velocity.magnitude >= 0.01)
        {
           anim.SetBool("IsAiming", false);
        }
        anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), 0, 0.1f));


        if (Input.GetMouseButtonDown(0) && canShoot && hasReload)
        {
            if (GetComponent<Player>().currentBullets > 0)
            {
                hasReload = false;
                StartCoroutine(Reload());
                Destroy(Instantiate(shootParticle, posiDisp), 1);
                GetComponent<Player>().currentBullets--;
                anim.SetTrigger("Shoot");

                Vector3 auxDireToPart = (hitEnemyPart.point - posiDisp.position).normalized;
                Vector3 forcedisp = shootForce * auxDireToPart;
                hitEnemyPart.collider.GetComponentInParent<Enemy>().Die(hitEnemyPart, forcedisp);

                //Ruido disparo
                FindObjectOfType<NoiseController>().GenerateNoise(shootNoise, this.transform.position);
                PlayerCanvas.instance.UpdateBullets(Player.instance.currentBullets);
            }
            else
            {
                print("No tiene balas");
            }
        }

    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(cadencia);
        hasReload = true;
    }

    void AimToTarget()
    {
        Vector3 vec = GetComponent<PointAndClickMove>().RayOnTarget(lmAttack);
        if (vec != Vector3.zero)
        {
            Vector3 direToEnemy = vec - playerHead.position;
            if (Physics.Raycast(playerHead.position, direToEnemy.normalized, out hitEnemyPart, Mathf.Infinity, aimIgnoreLayer))
            {

                Enemy enemy = hitEnemyPart.collider.gameObject.GetComponentInParent<Enemy>();
                if (enemy != null && !enemy.isDead)
                {
                    //Apunta a anemigo
                    canShoot = true;
                    anim.SetBool("IsAiming", true);
                    anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), 1, 0.3f));
                    TurnToAimPos(vec);
                    agent.SetDestination(transform.position);

                    if (hasReload)
                    {
                        Cursor.SetCursor(crossHair, new Vector2(crossHair.width / 2, crossHair.height / 2), CursorMode.Auto);
                    }
                    else
                    {
                        Cursor.SetCursor(redCross, new Vector2(redCross.width / 2, redCross.height / 2), CursorMode.Auto);
                    }
                }
                else
                {
                    //no puede apuntar
                    canShoot = false;
                    Cursor.SetCursor(redCross, new Vector2(redCross.width / 2, redCross.height / 2), CursorMode.Auto);

                }
            }
        }
        else
        {
            canShoot = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

    }

    void TurnToAimPos(Vector3 aimPosi)
    {
        Vector3 lookPos = aimPosi - transform.position;
        lookPos.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 2f);
    }
}
