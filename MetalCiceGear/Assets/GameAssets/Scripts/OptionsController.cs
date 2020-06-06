using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject optionPanel;

    //Botones que hay que desactivar
    List<GameObject> buttonList;

    private bool isPaused = false;
    private bool soundOn = false;

    // Start is called before the first frame update
    void Start()
    {
        optionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPause();
        }
    }


    public void BackMenuBtn()
    {
        /*GameManager._instance.sound.StopMusic();
        GameManager._instance.sound.PlayButtonSound();*/
        print("Entra al backmenubtn");
        GameManager._instance.sceneC.LoadMenu();
        SwitchPause();
    }

    public void ResumeBtn()
    {
        //GameManager._instance.sound.PlayButtonSound();
        SwitchPause();
    }

    public void SwitchPause()
    {
        isPaused = !isPaused;
        Button[] buttons = FindObjectsOfType<Button>();
        //Desactivar/Activar los botones que no sean del panel de opciones
        foreach(Button button in buttons)
        {
            if(button.gameObject.GetComponentInParent<OptionsController>() == null)
            {
                button.enabled = !isPaused;
            }
        }
        //GameManager._instance.sound.PlayPauseMusic();
        optionPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.000001f : 1;
    }
}
