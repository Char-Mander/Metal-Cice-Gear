using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadSceneLvl(int level)
    {
        if(level!=1) GameManager._instance.sound.PlayMainTheme();
        GameManager._instance.SetCurrentLvl(level);
        SceneManager.LoadScene("Level" + level);
    }

    public void LoadMenu()
    {
        GameManager._instance.sound.StopMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitForLoadGameOver());
    }

    IEnumerator WaitForLoadGameOver()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameOver");
    }
}
