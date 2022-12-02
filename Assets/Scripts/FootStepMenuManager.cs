using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepMenuManager : MonoBehaviour
{
    public GameObject Menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMenuActive()
    {
        Menu.SetActive(true);
    }

    public void SetPauseMenuActive()
    {
        if (GameManager.Instance.IsGameOver()) return;
        Menu.SetActive(true);
        GameManager.Instance.HandleGamePause();
    }
    public void SetMenuDeActice()
    {
        GameManager.Instance.UpdateGameStatus(GameManager.GAME_STATUS.IN_PROCESS);
        Menu.SetActive(false);
    }
    public void SetPauseMenuDeActive()
    {
        SetMenuDeActice();
        GameManager.Instance.HandleGameUnpause();
    }
    
}
