using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollMode : MonoBehaviour
{
    [HideInInspector]
    public bool isRagdoll = false;

    Animator anim;
    NavMeshAgent agent;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ApplyRagdoll(isRagdoll);
    }

    public void ApplyRagdoll(bool isActive) {
        anim.enabled = !isActive;
        if (agent != null) {
            agent.enabled = !isActive;
        }

        Rigidbody[] rigis = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigi in rigis) {
            rigi.isKinematic = !isActive;
        }
    }

}
