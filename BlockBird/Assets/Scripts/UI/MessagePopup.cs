using System;
using UnityEngine;

public class MessagePopup : MonoBehaviour
{
    public Transform messages;

    Action okAction;
    Action closeAction;

    public void ShowMessage(int index, Action okCallback, Action closeCallback)
    {
        messages.GetChild(index).gameObject.SetActive(true);
        okAction = okCallback;
        closeAction = closeCallback;
    }

    public void Ok()
    {
        if (okAction != null)
        {
            okAction();
        }

        Destroy(gameObject);
    }

    public void Close()
    {
        if (closeAction != null)
        {
            closeAction();
        }

        Destroy(gameObject);
    }
}
