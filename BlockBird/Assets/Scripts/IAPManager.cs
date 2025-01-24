using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;


public class IAPManager : MonoBehaviour, IStoreListener, IDetailedStoreListener
{
    public TextMeshProUGUI removeADPriceText;

    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    public static string removeAdsProductId = "remove_ads"; // 실제 IAP ID로 설정


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



        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    // 광고 제거 버튼 클릭 시 호출할 메서드
    public void BuyRemoveAdsOrRestore()
    {
        if (IsAdsRemoved()) // 이미 광고가 제거된 상태인지 확인
        {
            Debug.Log("이미 광고가 제거된 상태입니다.");
            return;
        }

        // iOS 또는 Android에서 복원 기능 실행
        RestorePurchases();
    }

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
        yield return new WaitForSeconds(second);
        PersistentObject.Instance.StopSpinningLoading();
    }

    // 특정 상품 구매
    void BuyProductID(string productId)
    {
        PersistentObject.Instance.StartSpinningLoading();
        StartCoroutine(StopSpinningLoading(2.5f));

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

    // 구매 또는 복원 성공 시 광고 제거 상태 저장
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //if(UIManager.Instance.levelName == "StartMenu")
        //{
        //    return PurchaseProcessingResult.Complete;
        //}

        if (args.purchasedProduct.definition.id == removeAdsProductId)
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

                    PersistentObject.Instance.ShowMessagePopup(0, () =>
                    {
                        //UIManager.Instance.adShopPopup.SetActive(false);
                    });

                    return PurchaseProcessingResult.Complete;
                }
                else
                {
                    Debug.Log("환불된 구매입니다.");
                }
            }
        }
        return PurchaseProcessingResult.Complete;
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
    void RestorePurchases()
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
                        PersistentObject.Instance.ShowMessagePopup(1);
                    }
                }
                else
                {
                    Debug.Log("복원할 구매 항목이 없습니다. / " + message);
                    PersistentObject.Instance.ShowMessagePopup(1);
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

            // UI 업데이트 (예: 텍스트 필드에 가격 표시)
            removeADPriceText.text = localizedPrice;
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
}
