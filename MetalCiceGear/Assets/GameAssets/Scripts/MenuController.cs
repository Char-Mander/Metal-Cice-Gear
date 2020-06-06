using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private void Start()
    {
        GameManager._instance.InitData();
    }

    public void BtnStartGame()
    {
        //GameManager._instance.sound.PlayButtonSound();
        GameManager._instance.sceneC.LoadSceneLvl(1);
    }

    public void BtnOptions()
    {
        //GameManager._instance.sound.PlayButtonSound();
        GameManager._instance.optionsC.SwitchPause();
    }

    public void BtnExitGame()
    {
        //GameManager._instance.sound.PlayButtonSound();
        Application.Quit();
    }
}
