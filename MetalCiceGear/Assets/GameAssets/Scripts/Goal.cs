using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class Goal : MonoBehaviour
{
    bool triggered = false;
    PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponentInParent<Player>() != null && !triggered)
        {
            triggered = true;
            Player.instance.godMode = true;
            PlayDirector(director);
            StartCoroutine(WaitForGameOver());
        }
    }

    void PlayDirector(PlayableDirector director)
    {
        Player.instance.GetComponent<NavMeshAgent>().enabled = false;
        director.stopped += OnPlayableDirectorStopped;
        director.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            director.Stop();
            Player.instance.GetComponent<NavMeshAgent>().enabled = true;
            GameManager._instance.sound.PlayMainTheme();
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

    IEnumerator WaitForGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        GameManager._instance.sceneC.LoadGameOver();
    }

}
