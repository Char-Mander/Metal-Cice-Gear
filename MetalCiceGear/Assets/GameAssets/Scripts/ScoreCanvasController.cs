using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCanvasController : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private GameObject losePanel;
    //Score    
    /*[SerializeField]
    private TextMeshProUGUI timeText;*/


    private void Start()
    {
        LoadWinLoseSetUp(GameManager._instance.GetCurrentLifes() > 0);
    }

    public void LoadWinLoseSetUp(bool hasWin)
    {
        //SetUpTitleAndPanels
        winPanel.SetActive(hasWin);
        losePanel.SetActive(!hasWin);
        //SetUpSound
        if (hasWin) GameManager._instance.sound.PlayWinOneShot();
        else GameManager._instance.sound.PlayGameOverOneShot();
    }

    public void btnRetry()
    {
        GameManager._instance.sceneC.LoadSceneLvl(1);
    }

    public void btnLoadMenu()
    {
        GameManager._instance.sceneC.LoadMenu();
    }

    public void btnOptions()
    {
        GameManager._instance.optionsC.SwitchPause();
    }
    
}
