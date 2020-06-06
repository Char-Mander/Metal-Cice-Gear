using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowArea : MonoBehaviour
{
    [SerializeField]
    private int slowModificator = 3;
    [SerializeField]
    GameObject waterObj;

    bool isActive = false;

    private void Start()
    {
        waterObj.SetActive(false);   
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Enemy>() != null && isActive)
        {
            if (!other.GetComponentInParent<Enemy>().speedModified)
            {
                other.GetComponentInParent<Enemy>().speedModified = true;
                other.GetComponentInParent<NavMeshAgent>().speed /= slowModificator;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Enemy>() != null)
        {
            if (other.GetComponentInParent<Enemy>().speedModified)
            {
                other.GetComponentInParent<Enemy>().speedModified = false;
                other.GetComponentInParent<NavMeshAgent>().speed *= slowModificator;
            }
        }
    }

    public void Activation(bool value)
    {
        isActive = value;
        waterObj.SetActive(value);
    }
}