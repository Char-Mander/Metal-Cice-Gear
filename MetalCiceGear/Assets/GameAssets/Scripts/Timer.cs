using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [HideInInspector]
    public float speed = 0;
    [SerializeField] Image timerImage;

    bool isTriggered = false;
    float fillAmount = 0;

    private void Start()
    {
        timerImage.fillAmount = fillAmount;
    }

    void Update()
    {
        if (isTriggered)
        {
            fillAmount += Time.deltaTime * 1/speed;
        }

        fillAmount = Mathf.Clamp(fillAmount, 0, 1);
        timerImage.fillAmount = fillAmount;

        if (fillAmount == 1)
        {
            isTriggered = false;
            fillAmount = 0;
            ResetTimer(1.5f);
        }
    }

    public void SetIsTriggered(bool value) { isTriggered = value; }

    public void ResetTimer(float time)
    {
        StartCoroutine(WaitForReset(time));
    }

    IEnumerator WaitForReset(float time)
    {
        yield return new WaitForSeconds(time);
        timerImage.fillAmount = fillAmount;
    }
}
