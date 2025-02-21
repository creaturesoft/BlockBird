using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI gemText;

    public TextMeshProUGUI removeADPriceText;

    public TextMeshProUGUI gem1000PriceText;
    public TextMeshProUGUI gem2050PriceText;
    public TextMeshProUGUI gem3300PriceText;
    public TextMeshProUGUI gem4600PriceText;
    public TextMeshProUGUI gem12500PriceText;


    public GameObject removeADButton;
    public GameObject restorePurchasesButton;

    void Start()
    {
        SetGemText();

        removeADPriceText.text = PersistentObject.Instance.IAPManager.RemoveADPriceText;
        gem1000PriceText.text = PersistentObject.Instance.IAPManager.Gem1000PriceText;
        gem2050PriceText.text = PersistentObject.Instance.IAPManager.Gem2050PriceText;
        gem3300PriceText.text = PersistentObject.Instance.IAPManager.Gem3300PriceText;
        gem4600PriceText.text = PersistentObject.Instance.IAPManager.Gem4600PriceText;
        gem12500PriceText.text = PersistentObject.Instance.IAPManager.Gem12500PriceText;

        if(PersistentObject.Instance.IsNoAd)
        {
            removeADButton.SetActive(false);
        }

        //아이폰인 경우만 복원 버튼 활성화
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            restorePurchasesButton.SetActive(false);
        }

#if UNITY_EDITOR
        restorePurchasesButton.SetActive(true);
#endif


        GameManager.Instance.UserDataChanged += SetGemText;
        GameManager.Instance.LoadGuestGem();
    }

    private void OnDestroy()
    {
        GameManager.Instance.UserDataChanged -= SetGemText;
    }

    void SetGemText()
    {
        gemText.text = GameManager.Instance.UserData.gem.ToString();
    }

    public void RestorePurchases()
    {
        PersistentObject.Instance.IAPManager.RestorePurchases();
    }

    public void BuyRemoveAds()
    {
        PersistentObject.Instance.IAPManager.BuyRemoveAds();
    }

    public void BuyGem1000()
    {
        PersistentObject.Instance.IAPManager.BuyGem1000();
    }

    public void BuyGem2050()
    {
        PersistentObject.Instance.IAPManager.BuyGem2050();
    }

    public void BuyGem3300()
    {
        PersistentObject.Instance.IAPManager.BuyGem3300();
    }

    public void BuyGem4600()
    {
        PersistentObject.Instance.IAPManager.BuyGem4600();
    }

    public void BuyGem12500()
    {
        PersistentObject.Instance.IAPManager.BuyGem12500();
    }
}