using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordingManager : MonoBehaviour
{
    public static RecordingManager Instance;
    public GameObject RecordingPages;
    public Sprite BeforeRecording;
    public Sprite AfterRecording;

    public Text text;
    private float timer;
    private bool startedRecord;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
        startedRecord = false;
        timer = 0;
    }

    public void HandleRecordingPageOpen()
    {
        if(GameManager.Instance.GameProgress!=GameManager.GAME_PROGRESS.LEVEL_3)
        {
            return;
        }
        RecordingPages.SetActive(true);
        RecordingPages.GetComponentInChildren<Image>().sprite = BeforeRecording;
    }

    public void HandleRecordingStart()
    {
        if (startedRecord) return;
        RecordingPages.GetComponentInChildren<Image>().sprite = AfterRecording;
        RecordingPages.GetComponent<AudioSource>().Play();
        startedRecord = true;
        GameManager.Instance.HandleGameEnding();
    }

   public void HandleRecordingStop()
    {
        RecordingPages.GetComponent<AudioSource>().Stop();
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(startedRecord)
        {
            timer += Time.deltaTime;

            string seconds = timer.ToString("00");

            text.text = (string.Format("00:{0}", seconds));
        }
    }
}
