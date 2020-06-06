using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas instance;

    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject bulletImg;
    [SerializeField]
    private GameObject timerObj;

    #region Singleton
    private void Awake()
    {
       if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion


    public void UpdateTimer()
    {
        timerObj.SetActive(Player.instance.GetZombieMode());
    }

    public void UpdateBullets(int numBullets)
    {

        if (content.childCount > numBullets)
        {
            Destroy(content.GetChild(0).gameObject);
        }
        else if (content.childCount < numBullets)
        {
            int dif = numBullets - content.childCount;
            for (int i = 0; i < dif; i++)
            {
                Instantiate(bulletImg, content);
            }
        }
    }

}
