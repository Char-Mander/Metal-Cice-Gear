using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class PlayCinematics : MonoBehaviour
{
    PlayableDirector director;
    NavMeshAgent agent;
    List<bool> cinematicsPlayed = new List<bool>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<2; i++)
        {
            cinematicsPlayed.Add(false);
        }
        director = GetComponent<PlayableDirector>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager._instance.GetCurrentLvl() == 1 && !cinematicsPlayed[0])
        {
            cinematicsPlayed[0] = true;
            PlayDirector(director);
        }
    }

    void PlayDirector(PlayableDirector director)
    {
        agent.enabled = false;
        director.stopped += OnPlayableDirectorStopped;
        director.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            director.Stop();
            agent.enabled = true;
            GameManager._instance.sound.PlayMainTheme();
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }
}
