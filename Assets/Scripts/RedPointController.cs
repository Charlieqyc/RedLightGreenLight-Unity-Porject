using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPointController : MonoBehaviour
{
    public static RedPointController Instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void HandleLevelCompleted(GameManager.GAME_PROGRESS progress)
    {
        if(progress>=GameManager.GAME_PROGRESS.LEVEL_1)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
