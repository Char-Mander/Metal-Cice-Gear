using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class FireAlarm : MonoBehaviour
{
    [SerializeField]
    float activationTime = 4;
    bool active = false;
    PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Player>() != null && !active)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                active = true;
                StartCoroutine(Activation());
            }
        }
    }

    private void EnableOrDisableAreas(bool value)
    {
        SlowArea[] areas = FindObjectsOfType<SlowArea>();
        foreach (SlowArea area in areas)
        {
            area.Activation(value);
        }
    }


    void PlayDirector(PlayableDirector director)
    {
        Player.instance.GetComponent<NavMeshAgent>().enabled = false;
        SetPlayerPos();
        director.stopped += OnPlayableDirectorStopped;
        director.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            director.Stop();
            Player.instance.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    void OnDisable()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

    IEnumerator Activation()
    {
        PlayDirector(director);
        yield return new WaitForSeconds(2.4f);
        EnableOrDisableAreas(true);
        yield return new WaitForSeconds(activationTime);
        active = false;
        EnableOrDisableAreas(false);
    }

    void SetPlayerPos()
    {
        Vector3 pos;
        pos = Player.instance.transform.position;
        pos.x = this.transform.position.x + 0.2f;
        pos.z = this.transform.position.z + 0.5f;
        Player.instance.transform.position = pos;
        Player.instance.transform.rotation = this.transform.rotation;
    }
}
