using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGenerator : MonoBehaviour
{
    public string[] Dialogs_Topic;
    public string[] Dialogs_Reply_Option1;
    public string[] Dialogs_Reply_Option2;
    public string[] Dialogs_Reply_Reaction1;
    public string[] Dialogs_Reply_Reaction2;
    public string[] Dialogs_Reply_RandomTimelessReply;
    public float MaxWaitTime;
    public Text RandomTimelessReply, DialogsTxt, Dialogs_Reply_Option1Txt,Dialogs_Reply_Option2Txt;
    public ChatUI ui;
    public static DialogGenerator Instance;
    public bool DialogStarted;
    public bool DialogCompleted;
    public enum DIALOG_STAGE
    {
        THROW_TOPIC,
        WAIT_FOR_REPLY,
        SHOW_REACTION,
        TOPIC_END,
    }

    private int currentDialogIndex;
    private DIALOG_STAGE currentStage;
    private float timer, ReactionTimer;
    private int userReplyType;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentDialogIndex = 0;
        DialogStarted = false;
        DialogCompleted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogStarted || GameManager.Instance.IsGameOver() || GameManager.Instance.IsGamePause() || GameManager.Instance.IsLevelCompleted(GameManager.GAME_PROGRESS.LEVEL_1)) return;

        if (currentDialogIndex >= Dialogs_Topic.Length)
        {
            Dialogs_Reply_Option1Txt.transform.parent.gameObject.SetActive(false);
            Dialogs_Reply_Option2Txt.transform.parent.gameObject.SetActive(false);
            DialogCompleted = true;
            GameManager.Instance.HandleLevelCompleted(GameManager.GAME_PROGRESS.LEVEL_1);
            return;
        }
        if(currentStage == DIALOG_STAGE.THROW_TOPIC)
        {
            FireCurrentDialog();
        }
        if(currentStage == DIALOG_STAGE.WAIT_FOR_REPLY)
        {
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                ui.AddChatMessage(Dialogs_Reply_RandomTimelessReply[Random.Range(0, Dialogs_Reply_RandomTimelessReply.Length)], ChatUI.enumChatMessageType.MessageLeft);
               // RandomTimelessReply.text = Dialogs_Reply_RandomTimelessReply[Random.Range(0, Dialogs_Reply_RandomTimelessReply.Length)];
                //print(Dialogs_Reply_RandomTimelessReply[Random.Range(0, Dialogs_Reply_RandomTimelessReply.Length)]);
                timer = MaxWaitTime;
            }
        }
        if(currentStage == DIALOG_STAGE.SHOW_REACTION)
        {
            if (userReplyType == 0)
            {
                ui.AddChatMessage(Dialogs_Reply_Reaction1[currentDialogIndex], ChatUI.enumChatMessageType.MessageLeft);              
            }
            else
            {
                ui.AddChatMessage(Dialogs_Reply_Reaction2[currentDialogIndex], ChatUI.enumChatMessageType.MessageLeft);               
            }
            currentStage = DIALOG_STAGE.TOPIC_END;
            
        }

        if (currentStage == DIALOG_STAGE.TOPIC_END)
        {
           
            currentStage = DIALOG_STAGE.THROW_TOPIC;
            currentDialogIndex++;
        }
    }
  

    private void FireCurrentDialog()
    {
        ui.AddChatMessage(Dialogs_Topic[currentDialogIndex], ChatUI.enumChatMessageType.MessageLeft);        
        //print(Dialogs_Topic[currentDialogIndex]);
        currentStage = DIALOG_STAGE.WAIT_FOR_REPLY;
        timer = MaxWaitTime;
    }

    public void SetUserFirstReply()
    {
        SetUserReply(0);

    }

    public void SetUserSecondReply()
    {
        SetUserReply(1);
    }
    public void SetUserReply(int type)
    {
        int Dialogs_Reply_OptionIndex = 0;        
        if (currentStage != DIALOG_STAGE.WAIT_FOR_REPLY) return;
           
        if(currentDialogIndex < Dialogs_Reply_Option1.Length-1)
        Dialogs_Reply_OptionIndex = currentDialogIndex+1;   
        else
            Dialogs_Reply_OptionIndex = currentDialogIndex;        
        Dialogs_Reply_Option1Txt.text = Dialogs_Reply_Option1[Dialogs_Reply_OptionIndex];
        Dialogs_Reply_Option2Txt.text = Dialogs_Reply_Option2[Dialogs_Reply_OptionIndex];
        //RandomTimelessReply.text = "";
        if(type==0)
            ui.AddChatMessage(Dialogs_Reply_Option1[currentDialogIndex], ChatUI.enumChatMessageType.MessageRight);
        else
            ui.AddChatMessage(Dialogs_Reply_Option2[currentDialogIndex], ChatUI.enumChatMessageType.MessageRight);
        //if (userReplyType == 0) Dialogs_Reply_Option1Txt.text = Dialogs_Reply_Option1[currentDialogIndex];//print(Dialogs_Reply_Option1[currentDialogIndex]);
        //else Dialogs_Reply_Option2Txt.text = Dialogs_Reply_Option2[currentDialogIndex];//print(Dialogs_Reply_Option2[currentDialogIndex]);

        userReplyType = type;
        currentStage = DIALOG_STAGE.SHOW_REACTION;
    }
}
