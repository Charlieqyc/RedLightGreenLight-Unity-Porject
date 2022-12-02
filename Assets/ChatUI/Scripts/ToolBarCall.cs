using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarCall : MonoBehaviour {
    public GameObject TextInputBox;
    public GameObject EmojiListBox;
    public GameObject PhotoListBox;

    private void Start()
    {
        ShowTextInputBox();
    }

    public void ShowTextInputBox()
    {
        TextInputBox.SetActive(true);
        EmojiListBox.SetActive(false);
        PhotoListBox.SetActive(false);
    }

    public void ShowEmojiListBox()
    {
        TextInputBox.SetActive(false);
        EmojiListBox.SetActive(true);
        PhotoListBox.SetActive(false);
    }

    public void ShowPhotoListBox()
    {
        TextInputBox.SetActive(false);
        EmojiListBox.SetActive(false);
        PhotoListBox.SetActive(true);
    }
}
