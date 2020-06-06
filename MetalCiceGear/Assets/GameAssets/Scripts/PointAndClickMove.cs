using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PointAndClickMove : MonoBehaviour
{
    public LayerMask lmMove;

    public float crouchSpeed;
    public float walkSpeed;
    public float RunSpeed;

    float speed;
    NavMeshAgent agent;
    Animator anim;

    int _slowMod = 1;
    public int slowMod {
        get {return _slowMod;}
        set { _slowMod = value;
            if (_slowMod == 0) {
                _slowMod = 1;
            }
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = walkSpeed;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        speed = walkSpeed;
        if (Input.GetMouseButtonDown(1))
        {
            MoveToTarget();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Player.instance.CanFakeZombie())
            {
                speed = walkSpeed;
                anim.SetLayerWeight(1, 0);
                anim.SetLayerWeight(3, 1);
                Player.instance.ActivateZombieMode();
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl) && !Player.instance.GetZombieMode())
        {
            speed = crouchSpeed;
            anim.SetLayerWeight(1, 1);
            anim.SetLayerWeight(3, 0);
        }
        else if (!Player.instance.GetZombieMode())
        {
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(3, 0);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = RunSpeed;
            }
        }

        agent.speed = speed/ slowMod;

        anim.SetFloat("Speed", agent.velocity.magnitude);

        
    }

    void MoveToTarget() {
        Vector3 vec = RayOnTarget(lmMove);
        if (vec != Vector3.zero && agent.enabled == true)
        {
            agent.SetDestination(vec);
        }
    }

    public Vector3 RayOnTarget(LayerMask lm) {
        Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(rayo, out hit, Mathf.Infinity, lm))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    public void RandomIdle() {
        int nume = Random.Range(1, 3);
        anim.SetInteger("IdleRandom",nume );

    }

}
