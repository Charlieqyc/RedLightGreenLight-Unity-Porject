using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebPageManager : MonoBehaviour
{

    public static WebPageManager Instance;
    public GameObject[] buttons;
    public Sprite[] sprites;
    public GameObject Monitor;
    public GameObject WebPage;
    public WEB_PAGE CurrentPage;
    public GameObject Notification;

    public enum WEB_PAGE
    {
        HOME_PAGE,
        HIM_1,
        HIM_2,
        KIDS_1,
        KIDS_2,
        KIDS_3,
        MUG_DETAIL,
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    { 
        SetCurrentPage(WEB_PAGE.HOME_PAGE);
    }
    public void HandleMonitorOn()
    {
        Monitor.SetActive(true);
    }
    public void HandleMonitorOff()
    {
        Monitor.SetActive(false);
        GameManager.Instance.UpdateGameStatus(GameManager.GAME_STATUS.IN_PROCESS);
    }

    public void UserChooseHomePage()
    {
        SetCurrentPage(WEB_PAGE.HOME_PAGE);
    }
    public void UserChooseKidsGift()
    {
        SetCurrentPage(WEB_PAGE.KIDS_1);
    }

    public void UserChooseHimGift()
    {
        SetCurrentPage(WEB_PAGE.HIM_1);
    }

    public void UserChooseHimPage2()
    {
        SetCurrentPage(WEB_PAGE.HIM_2);
    }

    public void UserChooseKidPage2()
    {
        SetCurrentPage(WEB_PAGE.KIDS_2);
    }
    public void UserChooseKidPage3()
    {
        SetCurrentPage(WEB_PAGE.KIDS_3);
    }
    public void UserChooseMugDetail()
    {
        SetCurrentPage(WEB_PAGE.MUG_DETAIL);
    }

    public void UserChooseUnfordableMug()
    {
        Notification.SetActive(true);
        Notification.GetComponent<Text>().text = "ARE YOU RICH?";
        StartCoroutine(CloseNotification(1.5f));
    }

    IEnumerator CloseNotification(float sec)
    {
        yield return new WaitForSeconds(sec);
        Notification.SetActive(false);
    }
    public void HandlePlayerBuyMug()
    {
        Notification.SetActive(true);
        Notification.GetComponent<Text>().text = "Congrats! Buy Success!";
        GameManager.Instance.HandleLevelCompleted(GameManager.GAME_PROGRESS.LEVEL_2);
        StartCoroutine(CloseNotification(3));
    }

    public void SetCurrentPage(WEB_PAGE page)
    {
        CurrentPage = page;
        WebPage.GetComponent<Image>().sprite = sprites[(int)page];
        for(int i = 0;i<buttons.Length;i++)
        {
            buttons[i].SetActive(false);
        }
        buttons[(int)page].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsMonitorPopup()) return;
    }
}
