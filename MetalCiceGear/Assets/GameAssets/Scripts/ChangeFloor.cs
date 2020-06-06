using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeFloor : MonoBehaviour
{
    public enum StairsWay { DOWNSTAIRS, UPSTAIRS }
    public StairsWay way; 

    bool activated = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<Player>() != null && !activated)
        {
            activated = true;
            int lvl = GameManager._instance.GetCurrentLvl();
            switch (way)
            {
                case StairsWay.DOWNSTAIRS:
                    if (lvl < GameManager._instance.GetMaxLevels()) GameManager._instance.sceneC.LoadSceneLvl(lvl + 1);
                    break;
                case StairsWay.UPSTAIRS:
                    if (lvl > 1) GameManager._instance.sceneC.LoadSceneLvl(lvl - 1);
                    break;
            }
        }
    }
}
