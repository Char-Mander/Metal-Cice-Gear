using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates {PATROL,ALERT,CONFUSED,KILLER }

public class Enemy : MonoBehaviour
{
    public EnemyStates state { get; set; }

    public bool isDead = false;
    public Transform head;
    public GameObject VisonconeObj;
    public GameObject handWeapon;
    public GameObject weistWeapon;
    public GameObject posiDisp;
    public float shootForce = 100;
    
    public bool speedModified { get; set; }

    bool hasShoot = false;

    Animator anim;
    NavMeshAgent agent;
    RagdollMode ragdoll;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyStates.PATROL;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ragdoll = GetComponent<RagdollMode>();
        speedModified = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
        }
        else {
            VisonconeObj.SetActive(false);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 lookPos = Player.instance.transform.position - this.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        this.transform.rotation = rotation;
    }

    public void Die(RaycastHit enemyPart,Vector3 shootForce)
    {
        ragdoll.ApplyRagdoll(true);
        enemyPart.collider.GetComponent<Rigidbody>().AddForce(shootForce, ForceMode.Impulse);
        isDead = true;
    }

    public void TakeWeapon() {
        SwichWeapon(true);
    }
    public void DropWeapon(){
        SwichWeapon(false);
    }
    public void SwichWeapon(bool value) {
        weistWeapon.SetActive(!value);
        handWeapon.SetActive(value);
    }

    public void ShootPistol() {
        if (!hasShoot)
        {
            hasShoot = true;
            StartCoroutine(drawLaserLine());
            Vector3 auxForeceShoot = (Player.instance.head.position - posiDisp.transform.position).normalized * shootForce;
            Player.instance.Die(auxForeceShoot);
        }
    }

    IEnumerator drawLaserLine() {
        posiDisp.GetComponent<AudioSource>().Play();
        posiDisp.GetComponent<LineRenderer>().SetPosition(0, posiDisp.transform.position);
        posiDisp.GetComponent<LineRenderer>().SetPosition(1, Player.instance.head.GetChild(0).position);
        yield return new WaitForSeconds(0.05f);
        posiDisp.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
        posiDisp.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
    }
}
