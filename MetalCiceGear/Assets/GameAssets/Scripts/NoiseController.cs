using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController :MonoBehaviour
{
    public LayerMask lm;

    public void GenerateNoise(float noiseDist, Vector3 pos) {
        Collider[] colliders = Physics.OverlapSphere(pos, noiseDist, lm);
        foreach (Collider col in colliders)
        {
            col.GetComponentInParent<Enemy>().state = EnemyStates.ALERT;
            col.GetComponentInParent<Alert>().SetAlertDestination(pos);
        }
    }
}
