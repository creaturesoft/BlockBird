using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections;
using TMPro;

public class AdRemove : MonoBehaviour
{
    public TextMeshProUGUI removeADPriceText;
    public bool IsPlaying { get; set; } = true;

    void Start()
    {
        removeADPriceText.text = PersistentObject.Instance.IAPManager.RemoveADPriceText;
        StartCoroutine(CheckNoAd());
    }

    public void BuyRemoveAds()
    {
        PersistentObject.Instance.IAPManager.BuyRemoveAds();
    }

    IEnumerator CheckNoAd()
    {
        //�������� ���� Ȯ��
        while (!PersistentObject.Instance.IsNoAd)
        {
            yield return new WaitForSecondsRealtime(0.2f); // �ǽð� ���
        }

        IsPlaying = false;
        Destroy(gameObject);
    }

    public void Close()
    {
        IsPlaying = false;
        Destroy(gameObject);
    }

}