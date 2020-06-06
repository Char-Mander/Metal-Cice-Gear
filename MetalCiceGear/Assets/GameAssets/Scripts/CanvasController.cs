using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public Transform content;
    public GameObject bulletImg;
    #region Singleton
    public static CanvasController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this.gameObject) ;
        }
    }

    #endregion

    public void UpdateBullets(int numeBullets) {
        if (content.childCount > numeBullets) {
            Destroy(content.GetChild(0).gameObject);
        }
        if (content.childCount == 0) {
            for (int i = 0; i < numeBullets; i++)
            {
                Instantiate(bulletImg, content);
            }
        }
        
    }


}
