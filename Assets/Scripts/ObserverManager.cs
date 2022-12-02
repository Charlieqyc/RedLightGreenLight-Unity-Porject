using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverManager : MonoBehaviour
{
    public enum OBSERVER_TYPE
    {
        NONE,
        MOM,
        BROTHER,
        DAD,
    }
    public static ObserverManager Instance;
    public OBSERVER_TYPE NextObserver;
    public float RefreshTimeLowerCap;
    public float RefreshTimeUpperCap;
    public float ObserveLastTimeLowerCap;
    public float ObserveLastTimeUpperCap;
    public AudioClip[] Clips;
    public AudioClip PraiseClip;
    public AudioClip CloseDoorClip;
    public bool willObserverEnterRoom;
    private float timer;
    private float lastTimeTimer;
    private AudioSource audioSource;

    private void Awake()
    {
        if(Instance == null)Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        willObserverEnterRoom = GenerateNextObserver();
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(NextObserver!=OBSERVER_TYPE.NONE && !GameManager.Instance.IsGamePause())
        {
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                if(!audioSource.isPlaying)
                {
                    audioSource.clip = Clips[(int)NextObserver - 1];
                    audioSource.loop = true;
                    audioSource.Play();
                }
                lastTimeTimer -= Time.deltaTime;
                if(lastTimeTimer<=0)
                {
                    audioSource.Stop();
                    if(willObserverEnterRoom)
                    {
                        audioSource.clip = Clips[3];
                        audioSource.Play();
                        audioSource.loop = false;
                        GameManager.Instance.HandleMomEnterRoom();
                        if(!GameManager.Instance.IsGameOver())
                        {
                            StartCoroutine(HandleMomEnteredRoom());
                        }
                        else
                        {
                            NextObserver = OBSERVER_TYPE.NONE;
                        }
                    }
                    else willObserverEnterRoom = GenerateNextObserver();
                }
            }
        }
    }

    public void StopCurrentAudio()
    {
        audioSource.Stop();
        NextObserver = OBSERVER_TYPE.NONE;
    }

    public void HandleGameEnding()
    {
        GenerateNextObserver(true);
        willObserverEnterRoom = true;
    }


    IEnumerator HandleMomEnteredRoom()
    {
        NextObserver = OBSERVER_TYPE.NONE;
        yield return new WaitForSeconds(2f);
        //play praise audio
        audioSource.clip = PraiseClip;
        audioSource.loop = false;
        audioSource.Play();
        yield return new WaitForSeconds(3);
        audioSource.clip = CloseDoorClip;
        audioSource.loop = false;
        audioSource.Play();
        willObserverEnterRoom = GenerateNextObserver();
        GameManager.Instance.HandleMonQuitRoom();
    }

    public void HandleGamePause()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void HandleGameUnpause()
    {
        audioSource.UnPause();
    }

    public bool GenerateNextObserver(bool forceMom = false)
    {
        if (forceMom) timer = 0;
        else timer = Random.Range(RefreshTimeLowerCap,RefreshTimeUpperCap);
        if (forceMom) lastTimeTimer = 5;
        else lastTimeTimer = Random.Range(ObserveLastTimeLowerCap,ObserveLastTimeUpperCap);
        if (forceMom) NextObserver = OBSERVER_TYPE.MOM;
        else NextObserver = (OBSERVER_TYPE)Random.Range(1,4);
        if(NextObserver == OBSERVER_TYPE.MOM) return Random.Range(0,2) == 0;
        else return false;
    }
}
