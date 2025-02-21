using System;
using System.Collections;
using UnityEngine;

public class WaitMessagePopup : MonoBehaviour
{

    bool isPopupConfirmed;
    Action action1;
    Action action2;


    public IEnumerator ShowPopupAndWait(Action action1Callback, Action action2Callback)
    {
        isPopupConfirmed = false;
        action1 = action1Callback;
        action2 = action2Callback;
        yield return new WaitUntil(() => isPopupConfirmed);
    }

    public void Action1()
    {
        if (action1 != null)
        {
            action1();
        }

        isPopupConfirmed = true;
        Destroy(gameObject);
    }

    public void Action2()
    {
        if (action2 != null)
        {
            action2();
        }

        isPopupConfirmed = true;
        Destroy(gameObject);
    }
}
