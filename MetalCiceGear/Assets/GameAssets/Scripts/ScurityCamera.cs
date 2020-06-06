using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScurityCamera : MonoBehaviour
{
    public float viewDistance;
    public float viewAngle;
    public float callDistance;

    VisionCone visionC;
    Player player;
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        visionC = GetComponentInChildren<VisionCone>();
        player = Player.instance;
        visionC.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        Looking();
    }

    void ActualiceVisonCone()
    {
        visionC.angle = viewAngle;
        visionC.GetComponent<Projector>().orthographicSize = viewDistance;
    }

    void Looking()
    {
        ActualiceVisonCone();
        Vector3 cameraGroundPos = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 direToPlayer = player.transform.position - cameraGroundPos;
        float angle = Vector3.Angle(transform.forward, direToPlayer);
        if (angle < viewAngle && direToPlayer.magnitude < viewDistance)
        {
            Vector3 viewDire = (player.chest.transform.position - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, viewDire, out hit, viewDistance))
            {
                if (hit.collider.GetComponentInParent<Player>() != null && !isActive && !Player.instance.GetZombieMode())
                {
                    Debug.DrawLine(transform.position, hit.transform.position, Color.blue);
                    visionC.color = Color.red;
                    StartCoroutine(BackToYellow());
                    FindObjectOfType<NoiseController>().GenerateNoise(callDistance, player.transform.position);
                }
            }
        }
    }

    IEnumerator BackToYellow() {
        isActive = true;
        yield return new WaitForSeconds(5);
        visionC.color = Color.yellow;
        isActive = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, callDistance);
    }

}
