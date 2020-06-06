using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    [HideInInspector]
    public OptionsController optionsC;
    [HideInInspector]
    public SceneController sceneC;
    [HideInInspector]
    public SoundManager sound;

    [SerializeField]
    private int maxLevels = 2;
    private int currentLvl = 1;
    private int levelsCompleted = 1;
    [SerializeField]
    private int initLifes = 1;
    private int currentLifes = 1;

    //Puntuaciones
    public TimeSpan lvlTime { get; set; }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        //References
        optionsC = GetComponentInChildren<OptionsController>();
        sceneC = GetComponent<SceneController>();
        sound = GetComponent<SoundManager>();
        //Init
        InitData();

    }

    public void InitData()
    {
        currentLifes = initLifes;
        currentLvl = 1;
    }

    public int GetMaxLevels() { return maxLevels; }

    public int GetLevelsCompleted() { return levelsCompleted; }

    public void SetLevelsCompleted(int lvl)
    {
        levelsCompleted = lvl;
    }

    public int GetCurrentLvl() { return currentLvl; }

    public void SetCurrentLvl(int lvl)
    {
        currentLvl = lvl;
    }

    public int GetInitLifes() { return initLifes; }

    public int GetCurrentLifes() { return currentLifes; }

    public void SetCurrentLifes(int currentLifes)
    {
        this.currentLifes = currentLifes;
        if (this.currentLifes > initLifes) this.currentLifes = initLifes;
    }
}
