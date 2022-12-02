using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public GameObject Circle;
    public GameObject Notification;
    //Mouse direction
    private Vector2 mD;

    //The capsule parent!
    private Transform myBody;

    // Use this for initialization
    void Start()
    {
        mD.y = -this.transform.eulerAngles.x;
        mD.x = this.transform.eulerAngles.y;
        myBody = this.transform.parent;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ShouldDisableUserControl()) return;

        Vector2 mC = new Vector2
            (Input.GetAxisRaw("Mouse X"),
                Input.GetAxisRaw("Mouse Y"));

        mD += mC;
        //���¿������
        //��x����ת����ת��С
        this.transform.localRotation =
            Quaternion.AngleAxis(-mD.y, Vector3.right);
        //���ҿ��Ƹ���
        //��y����ת����ת��С
        myBody.localRotation =
            Quaternion.AngleAxis(mD.x, Vector3.up);

        //ray cast detection
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));//����

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))//��������(���ߣ�������ײ��Ϣ�����߳��ȣ����߻���Ĳ㼶)
        {
            if(hit.collider.tag == "STUDY")
            {
                Circle.GetComponent<Image>().color = Color.green;
                GameManager.Instance.UpdatePlayerStatus(GameManager.PLAYER_STATUS.STUDYING);
            }
            else
            {
                Circle.GetComponent<Image>().color = Color.red;
                if (hit.collider.tag == "PHONE")
                {
                    GameManager.Instance.UpdatePlayerStatus(GameManager.PLAYER_STATUS.WATCHING_PHONE);
                    Notification.SetActive(true);
                    Notification.GetComponent<Text>().text = "Left Click To Interact";
                }
                else if (hit.collider.tag == "MONITOR")
                {
                    GameManager.Instance.UpdatePlayerStatus(GameManager.PLAYER_STATUS.WATCHING_MONITOR);
                    if(GameManager.Instance.GameProgress==GameManager.GAME_PROGRESS.LEVEL_2)
                    {
                        Notification.SetActive(true);
                        Notification.GetComponent<Text>().text = "Left Click To Interact";
                    }
                }
                else
                {
                    GameManager.Instance.UpdatePlayerStatus(GameManager.PLAYER_STATUS.NOT_STUDYING);
                    Notification.SetActive(false);
                }

            }
        }

    }
}
