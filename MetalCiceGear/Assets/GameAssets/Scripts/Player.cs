using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [HideInInspector]
    public float noise { get; private set; }

    public Transform chest;
    public Transform head;
    public int maxBullets = 10;
    public float secondsToDie = 1;
    public float zombieCadency = 7;
    public float zombieTime = 4;
    [HideInInspector]
    public bool godMode = false;

    public List<keyColor> keysC = new List<keyColor>();

    private int _currentBullets;
    public int currentBullets
    {
        get { return _currentBullets; }
        set
        {
            _currentBullets = value;
            if (value > maxBullets) { _currentBullets = maxBullets; }
            if (value < 0) { _currentBullets = 0; }
        }
    }

    NavMeshAgent agent;
    Animator anim;
    AlertArea alertA;
    bool isDisabled = false;
    bool zombieMode = false;
    bool canFakeZombie = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        alertA = GetComponentInChildren<AlertArea>();
        currentBullets = maxBullets;
        anim = GetComponent<Animator>();
        PlayerCanvas.instance.UpdateBullets(currentBullets);
        FindObjectOfType<Timer>().speed = zombieTime;
        FindObjectOfType<PlayerCanvas>().UpdateTimer();
    }
    private void Update()
    {
        noise = agent.velocity.magnitude * 2;
        alertA.distance = noise;

    }

    public void ApplySlowMode(int mod)
    {
        GetComponent<PointAndClickMove>().slowMod = mod;
    }

    public void DisablePlayer()
    {
        if (!isDisabled)
        {
            isDisabled = true;
            agent.enabled = false;
            anim.enabled = false;

            GetComponent<PointAndClickMove>().enabled = false;
            GetComponent<AimAndShoot>().enabled = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

    }

    public void EnablePlayer()
    {

        if (isDisabled)
        {
            isDisabled = false;
            agent.enabled = true;
            anim.enabled = true;

            GetComponent<PointAndClickMove>().enabled = true;
            GetComponent<AimAndShoot>().enabled = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void Die(Vector3 impactForce)
    {
        GetComponent<RagdollMode>().ApplyRagdoll(true);
        alertA.enabled = false;
        head.GetComponent<Rigidbody>().AddForce(impactForce, ForceMode.Impulse);
        StartCoroutine(SlowMoDie());
    }

    IEnumerator SlowMoDie()
    {
        GameManager._instance.sound.PlayDieOneShot();
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(secondsToDie);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1);
        GameManager._instance.SetCurrentLifes(GameManager._instance.GetCurrentLifes()-1);
        GameManager._instance.sceneC.LoadGameOver();
    }

    public bool CheckKey(keyColor keyColor) {
        foreach (keyColor keyc in keysC) {
            if (keyc == keyColor) {
                return true;
            }
        }
        return false;
    }
    
    public void AddKey(keyColor k) {
        if (!keysC.Contains(k)) {
            keysC.Add(k);
        }
    }

    public bool IsDisabled() { return isDisabled; }

    public bool GetZombieMode() { return zombieMode; }

    public void ActivateZombieMode()
    {
        canFakeZombie = false;
        zombieMode = true;
        FindObjectOfType<PlayerCanvas>().UpdateTimer();
        FindObjectOfType<Timer>().SetIsTriggered(true);
        StartCoroutine(ResetZombieMode(zombieTime));
    }

    public void CancelZombieMode()
    {
        StartCoroutine(ResetZombieMode(0));
    }

    IEnumerator ResetZombieMode(float endTime)
    {
        yield return new WaitForSeconds(endTime);
        zombieMode = false;
        anim.SetLayerWeight(3, 0);
        FindObjectOfType<Timer>().SetIsTriggered(false);
        FindObjectOfType<Timer>().ResetTimer(0);
        yield return new WaitForSeconds(0.1f);
        FindObjectOfType<PlayerCanvas>().UpdateTimer();
        yield return new WaitForSeconds(zombieCadency);
        canFakeZombie = true;
    }

    public bool CanFakeZombie() { return canFakeZombie; }
}
