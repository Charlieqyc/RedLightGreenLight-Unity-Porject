using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text StatusText;
    public Text LevelTipsText;
    public GameObject Phone;
    public GameObject Door;
    public GameObject MomAvatar;
    public enum PLAYER_STATUS
    {
        STUDYING,
        WATCHING_PHONE,
        WATCHING_MONITOR,
        NOT_STUDYING,
    }
    public enum GAME_STATUS
    {
        IN_PROCESS,
        PAUSE,
        PHONE_POP_UP,
        MONITOR_POP_UP,
        GAME_COMPLETED,
        GAME_FAILED,
    }

    public enum GAME_PROGRESS
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        END,
    }

    public GAME_PROGRESS GameProgress;

    public GAME_STATUS GameStatus;

    public PLAYER_STATUS myStatus;

    public AudioClip[] BadEndingClip;

    private string GetStatusText(PLAYER_STATUS status)
    {
        switch (status)
        {
            case PLAYER_STATUS.STUDYING:
                return "Studying";
            case PLAYER_STATUS.WATCHING_PHONE:
                return "Watching Phone";
            case PLAYER_STATUS.WATCHING_MONITOR:
                return "Watching Monitor";
            case PLAYER_STATUS.NOT_STUDYING:
                return "What are you doing?";
        }
        return null;
    }

    private string GetLevelTipText(GAME_PROGRESS progress)
    {
        switch(progress)
        {
            case GAME_PROGRESS.LEVEL_1:
                return "Chat With Friend To See What Happened";
            case GAME_PROGRESS.LEVEL_2:
                return "Shop online for the best Gift!";
            case GAME_PROGRESS.LEVEL_3:
                return "Record 'Happy Birthday'";
        }
        return null;
    }


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStatus = GAME_STATUS.IN_PROCESS;
        GameProgress = GAME_PROGRESS.LEVEL_1;
    }

    public bool IsLevelCompleted(GAME_PROGRESS progress)
    {
        return GameProgress > progress;
    }

    public void HandleLevelCompleted(GAME_PROGRESS progress)
    {
        if(progress== GAME_PROGRESS.LEVEL_3)
        {
            GameStatus = GAME_STATUS.GAME_COMPLETED;
        }
        if (GameProgress == progress)
        {
            GameProgress++;
            RedPointController.Instance.HandleLevelCompleted(progress);
        }
    }

    public void HandleMomEnterRoom()
    {
        Door.transform.eulerAngles = new Vector3(0,-86,0);
        MomAvatar.SetActive(true);
        if (myStatus!=PLAYER_STATUS.STUDYING)
        {
            print("Game Over");
            GameStatus = GAME_STATUS.GAME_FAILED;
            RecordingManager.Instance.HandleRecordingStop();
            this.GetComponent<AudioSource>().clip = myStatus == PLAYER_STATUS.WATCHING_PHONE? BadEndingClip[0]:BadEndingClip[1];
            this.GetComponent<AudioSource>().Play();
            StartCoroutine(PlayEndingMusic());
        }
    }

    public void HandleMonQuitRoom()
    {
        Door.transform.localEulerAngles = new Vector3(0, 0, 0);
        MomAvatar.SetActive(false);
    }

    IEnumerator PlayEndingMusic()
    {
        yield return new WaitForSeconds(4);
        this.GetComponent<AudioSource>().clip = BadEndingClip[2];
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(7);
        SceneManager.LoadScene(1);
    }

    public bool IsGameOver()
    {
        return GameStatus >= GAME_STATUS.GAME_COMPLETED;
    }

    public bool IsGamePause()
    {
        return GameStatus == GAME_STATUS.PAUSE;
    }

    public bool IsPhonePopup()
    {
        return GameStatus == GAME_STATUS.PHONE_POP_UP;
    }

    public bool IsMonitorPopup()
    {
        return GameStatus == GAME_STATUS.MONITOR_POP_UP;
    }
    public bool ShouldDisableUserControl()
    {
        return (IsGameOver() || IsGamePause() || IsPhonePopup() || IsMonitorPopup());
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void UpdatePlayerStatus(PLAYER_STATUS status)
    {
        if (GameStatus != GAME_STATUS.IN_PROCESS) return;
        myStatus = status;
        StatusText.text = GetStatusText(status);
    }

    public void UpdateGameStatus(GAME_STATUS status)
    {
        GameStatus = status;
    }

    IEnumerator HandleGameEndingFlow()
    {
        ObserverManager.Instance.StopCurrentAudio();
        yield return new WaitForSeconds(15f);
        ObserverManager.Instance.HandleGameEnding();
    }

    public void HandleGameEnding()
    {
        HandleLevelCompleted(GameManager.GAME_PROGRESS.LEVEL_3);
        StartCoroutine(HandleGameEndingFlow());
    }

    public void HandleGamePause()
    {
        GameStatus = GAME_STATUS.PAUSE;
        ObserverManager.Instance.HandleGamePause();
    }

    public void HandleGameUnpause()
    {
        ObserverManager.Instance.HandleGameUnpause();
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        LevelTipsText.text = GetLevelTipText(GameProgress);
        if (Input.GetMouseButtonDown(0)&&myStatus == PLAYER_STATUS.WATCHING_PHONE && GameStatus == GAME_STATUS.IN_PROCESS)
        {
            // Phone.GetComponent<FootStepMenuManager>().SetMenuActive();
            Phone.SetActive(true);
            GameStatus = GAME_STATUS.PHONE_POP_UP;
            DialogGenerator.Instance.DialogStarted = true;
        }

        if (Input.GetMouseButtonDown(0) && myStatus == PLAYER_STATUS.WATCHING_MONITOR && GameStatus == GAME_STATUS.IN_PROCESS && GameProgress>GAME_PROGRESS.LEVEL_1)
        {
            WebPageManager.Instance.HandleMonitorOn();
            GameStatus = GAME_STATUS.MONITOR_POP_UP;
        }
    }
}
