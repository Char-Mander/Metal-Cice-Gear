using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isOpen = false;
    public keyColor kColor;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        //GetComponent<Renderer>().material.color = Key.GetMyKeyColor(kColor);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.GetComponentInParent<Player>() != null) {
            if (col.GetComponentInParent<Player>().CheckKey(kColor)) {
                if (Input.GetKeyDown(KeyCode.E) && !isOpen){
                    anim.SetTrigger("Open");
                    isOpen = true;
                }
            }
            
        }
    }
}
