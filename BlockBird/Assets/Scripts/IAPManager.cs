using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using static UnityEngine.Rendering.GPUSort;


public class IAPManager : MonoBehaviour, IStoreListener, IDetailedStoreListener
{
    public string RemoveADPriceText { get; set; }
    public string Gem1000PriceText { get; set; }
    public string Gem2050PriceText { get; set; }
    public string Gem3300PriceText { get; set; }
    public string Gem4600PriceText { get; set; }
    public string Gem12500PriceText { get; set; }


    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

#if UNITY_ANDROID
    public static string removeAdsProductId = "remove_ads";
    public static string gem1000ProductId = "gem1000";
    public static string gem2050ProductId = "gem2050";
    public static string gem3300ProductId = "gem3300";
    public static string gem4600ProductId = "gem4600";
    public static string gem12500ProductId = "gem12500";
#else
    public static string removeAdsProductId = "blockbirds_remove_ads";
    public static string gem1000ProductId = "blockbirds_gem1000";
    public static string gem2050ProductId = "blockbirds_gem2050";
    public static string gem3300ProductId = "blockbirds_gem3300";
    public static string gem4600ProductId = "blockbirds_gem4600";
    public static string gem12500ProductId = "blockbirds_gem12500";
#endif

    void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(removeAdsProductId, ProductType.NonConsumable);
        builder.AddProduct(gem1000ProductId, ProductType.Consumable);
        builder.AddProduct(gem2050ProductId, ProductType.Consumable);
        builder.AddProduct(gem3300ProductId, ProductType.Consumable);
        builder.AddProduct(gem4600ProductId, ProductType.Consumable);
        builder.AddProduct(gem12500ProductId, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    //구매 복원
    //public void BuyRemoveAdsOrRestore()
    //{
    //    if (IsAdsRemoved()) // 이미 광고가 제거된 상태인지 확인
    //    {
    //        Debug.Log("이미 광고가 제거된 상태입니다.");
    //        return;
    //    }

    //    // iOS 또는 Android에서 복원 기능 실행
    //    RestorePurchases();
    //}

    // 광고 제거 구매 시작
    public void BuyRemoveAds()
    {
        if (IsAdsRemoved()) // 이미 광고가 제거된 상태인지 확인
        {
            Debug.Log("이미 광고가 제거된 상태입니다.");
            return;
        }

        BuyProductID(removeAdsProductId);
    }

    IEnumerator StopSpinningLoading(float second)
    {
        yield return new WaitForSecondsRealtime(second);
        PersistentObject.Instance.StopSpinningLoading();
    }

    // 특정 상품 구매
    void BuyProductID(string productId)
    {
        PersistentObject.Instance.StartSpinningLoading();
        StartCoroutine(StopSpinningLoading(1f));

        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                PersistentObject.Instance.ShowMessagePopup(2);
                Debug.Log("구매할 수 없는 상품입니다.");
            }
        }
    }


    private bool IsCancelOrRefundReceipt(string receipt)
    {
        // 영수증을 서버로 전송하여 검증 (권장)
        // 여기서는 간단히 로컬에서 처리하는 예제
        if (receipt.Contains("\"purchaseState\":1") || receipt.Contains("\"purchaseState\":2")) // Google Play 환불 상태 예제
        {
            return true; // 환불된 구매
        }
        if (receipt.Contains("\"cancellation_date\"")) // App Store 환불 상태 예제
        {
            return true; // 환불된 구매
        }
        return false; // 유효한 구매
    }

    // iOS 및 Android에서 광고 제거 복원 기능
    public void RestorePurchases()
    {
        PersistentObject.Instance.StartSpinningLoading();
        StartCoroutine(StopSpinningLoading(1f));

        if (!IsInitialized())
        {
            Debug.Log("IAP 초기화가 필요합니다.");
            return;
        }

        // iOS 또는 Android에서 복원 시도
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result, message) =>
            {
                if (result)
                {
                    Debug.Log("구매 복원이 성공적으로 완료되었습니다.");
                    // 복원된 제품 처리
                    Product product = storeController.products.WithID(removeAdsProductId);
                    if (product != null && product.hasReceipt)
                    {
                        //PlayerPrefs.SetInt("RemoveAds", 1); // 광고 제거 상태 저장
                        //SaveLoadManager.SaveNoADData();

                        //Debug.Log("광고 제거 상태가 복원되었습니다.");

                        //PersistentObject.Instance.ShowMessagePopup(0, () =>
                        //{
                        //    UIManager.Instance.adShopPopup.SetActive(false);
                        //});
                    }
                    else
                    {
                        Debug.Log("복원할 구매 항목이 없습니다.");
                        //PersistentObject.Instance.ShowMessagePopup(1);
                    }
                }
                else
                {
                    Debug.Log("복원할 구매 항목이 없습니다. / " + message);
                    //PersistentObject.Instance.ShowMessagePopup(1);
                }
            });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            // Android에서는 ProcessPurchase가 자동으로 이전 구매 상태를 확인하고 처리합니다.
            Product product = storeController.products.WithID(removeAdsProductId);
            if (product != null && product.hasReceipt)
            {
                SaveLoadManager.SaveNoADData(); // 광고 제거 상태 저장
                Debug.Log("Android에서 이전 구매 항목이 자동으로 복원되었습니다.");

                PersistentObject.Instance.ShowMessagePopup(0, () =>
                {
                    //UIManager.Instance.adShopPopup.SetActive(false);
                });
            }
            else
            {
                Debug.Log("복원할 구매 항목이 없습니다.");
                PersistentObject.Instance.ShowMessagePopup(1);
            }
            
        }
        else
        {
            Debug.Log("이 플랫폼에서는 복원이 지원되지 않습니다.");
        }
    }

    // 광고 제거 상태 확인
    bool IsAdsRemoved()
    {
        return SaveLoadManager.LoadNoADData() == "true";
    }

    // 초기화 성공 시 호출
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;

        // 상품의 지역화된 가격 가져오기
        Product product = storeController.products.WithID(removeAdsProductId);
        if (product != null)
        {
            string localizedPrice = product.metadata.localizedPriceString; // 로컬화된 가격 (예: "₩5,500")
            string currencyCode = product.metadata.isoCurrencyCode; // 통화 코드 (예: "KRW")

            //광고제거 가격
            RemoveADPriceText = localizedPrice;
        }

        product = storeController.products.WithID(gem1000ProductId);
        if (product != null)
        {
            string localizedPrice = product.metadata.localizedPriceString; // 로컬화된 가격 (예: "₩5,500")
            string currencyCode = product.metadata.isoCurrencyCode; // 통화 코드 (예: "KRW")

            //1000gem 가격
            Gem1000PriceText = localizedPrice;
        }

        product = storeController.products.WithID(gem2050ProductId);
        if (product != null)
        {
            string localizedPrice = product.metadata.localizedPriceString; // 로컬화된 가격 (예: "₩5,500")
            string currencyCode = product.metadata.isoCurrencyCode; // 통화 코드 (예: "KRW")

            Gem2050PriceText = localizedPrice;
        }

        product = storeController.products.WithID(gem3300ProductId);
        if (product != null)
        {
            string localizedPrice = product.metadata.localizedPriceString; // 로컬화된 가격 (예: "₩5,500")
            string currencyCode = product.metadata.isoCurrencyCode; // 통화 코드 (예: "KRW")

            Gem3300PriceText = localizedPrice;
        }

        product = storeController.products.WithID(gem4600ProductId);
        if (product != null)
        {
            string localizedPrice = product.metadata.localizedPriceString; // 로컬화된 가격 (예: "₩5,500")
            string currencyCode = product.metadata.isoCurrencyCode; // 통화 코드 (예: "KRW")

            Gem4600PriceText = localizedPrice;
        }

        product = storeController.products.WithID(gem12500ProductId);
        if (product != null)
        {
            string localizedPrice = product.metadata.localizedPriceString; // 로컬화된 가격 (예: "₩5,500")
            string currencyCode = product.metadata.isoCurrencyCode; // 통화 코드 (예: "KRW")

            Gem12500PriceText = localizedPrice;
        }

        storeController = controller;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("IAP 초기화 실패: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("IAP 초기화 실패: " + error + "/" + message);
    }

    // 구매 실패 시 호출
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("구매 실패: " + failureReason);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log("구매 실패: " + failureDescription.message);
    }


    // 구매 또는 복원 성공 시 광고 제거 상태 저장
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        PersistentObject.Instance.StartSpinningLoading();
        StartCoroutine(StopSpinningLoading(5f));

        if (args.purchasedProduct.definition.id == removeAdsProductId)
        {
            RemoveAdsProcessPurchase(args);
            PersistentObject.Instance.StopSpinningLoading();
        }
        else if(args.purchasedProduct.definition.id == gem1000ProductId)
        {
            GemProcessPurchase(1000);
        }
        else if (args.purchasedProduct.definition.id == gem2050ProductId)
        {
            GemProcessPurchase(2050);
        }
        else if (args.purchasedProduct.definition.id == gem3300ProductId)
        {
            GemProcessPurchase(3300);
        }
        else if (args.purchasedProduct.definition.id == gem4600ProductId)
        {
            GemProcessPurchase(4600);
        }        
        else if (args.purchasedProduct.definition.id == gem12500ProductId)
        {
            GemProcessPurchase(12500);
        }

        return PurchaseProcessingResult.Complete;
    }

    //광고 제거
    void RemoveAdsProcessPurchase(PurchaseEventArgs args)
    {
        SaveLoadManager.DeleteNoADData();

        // 영수증 검증
        string receipt = args.purchasedProduct.receipt;
        if (!string.IsNullOrEmpty(receipt))
        {
            Debug.LogWarning($"Receipt: {receipt}");
            bool isRefunded = IsCancelOrRefundReceipt(receipt);
            if (!isRefunded)
            {
                Debug.Log("광고 제거 상품 구매 성공");
                //PlayerPrefs.SetInt("RemoveAds", 1); // 광고 제거 상태 저장
                SaveLoadManager.SaveNoADData();

                if (PageController.Instance.Shop != null)
                {
                    PageController.Instance.Shop.removeADButton.SetActive(false);
                    PersistentObject.Instance.ShowMessagePopup(1, () => { });
                }
            }
            else
            {
                Debug.Log("환불된 구매입니다.");
            }
        }
    }

    public void BuyGem1000()
    {
        BuyProductID(gem1000ProductId);
    }

    public void BuyGem2050()
    {
        BuyProductID(gem2050ProductId);
    }

    public void BuyGem3300()
    {
        BuyProductID(gem3300ProductId);
    }

    public void BuyGem4600()
    {
        BuyProductID(gem4600ProductId);
    }

    public void BuyGem12500()
    {
        BuyProductID(gem12500ProductId);
    }


    public void GemProcessPurchase(int amount)
    {
        string data = "{\"userId\": \"" + PersistentObject.Instance.UserData.userId + "\", " +
                "\"type\": \"PURCHASE\", " +
                "\"amount\": " + amount + "}";

        StartCoroutine(WebRequest.Post(WebRequest.GemTransactionURL, data, (result) =>
        {
            if (result == null)
            {
                //실패
                PersistentObject.Instance.ShowMessagePopup(0, () => { });
            }
            else
            {
                //성공
                PersistentObject.Instance.UserData.gem += (int)result.GetValue("amount");

                SaveLoadManager.SaveUserData(PersistentObject.Instance.UserData);
                PersistentObject.Instance.ShowMessagePopup(1, () => { });
            }
        }));
    }

}
