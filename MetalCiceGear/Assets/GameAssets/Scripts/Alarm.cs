using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;

public class Alarm : MonoBehaviour
{
    [SerializeField]
    private float activationTime = 7;
    [SerializeField]
    private float noiseDistance = 20;
    [SerializeField]
    private GameObject alarmObj;
    bool activated = false;
    PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponentInParent<Player>() != null && !activated)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                activated = true;
                StartCoroutine(Activate());
            }
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

    IEnumerator Activate()
    {
        PlayDirector(director);
        yield return new WaitForSeconds(4);
        alarmObj.GetComponent<AudioSource>().Play();
        FindObjectOfType<NoiseController>().GenerateNoise(noiseDistance, alarmObj.transform.position);
        yield return new WaitForSeconds(activationTime);
        activated = false;
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
