using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField]
    int bulletAmmount = 3;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<Player>() != null && Player.instance.currentBullets < Player.instance.maxBullets)
        {
            Player.instance.currentBullets += bulletAmmount;
            FindObjectOfType<PlayerCanvas>().UpdateBullets(Player.instance.currentBullets);
            Destroy(this.gameObject);
        }
    }
}
