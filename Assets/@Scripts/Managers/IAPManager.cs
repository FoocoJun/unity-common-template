using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>
{
    StoreController _storeController;
    Action _onPurchaseCallback;

    public bool IsNoAds { get; private set; }

    public bool IsNoAdsProduct(string productId)
    {
        return productId.Contains("noads");
    }

    #region 아이템 구매
    public void Purchase(string productId, Action callback)
    {
        var product = _storeController?.GetProducts().FirstOrDefault(product => product.definition.id == productId);

        if (product != null)
        {
            _onPurchaseCallback = callback;
            _storeController?.PurchaseProduct(product);
        }
        else
        {
            Debug.Log($"The product service has no product with the ID {productId}");
        }
    }

    public void RestorePurchases()
    {
        _storeController.RestoreTransactions(OnTransactionsRestored);
    }
    #endregion

    public void Init()
    {
        InitializeIAP();
    }

    async void InitializeIAP()
    {
        _storeController = UnityIAPServices.StoreController();

        _storeController.OnPurchasePending += OnPurchasePending;
        _storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        _storeController.OnCheckEntitlement += OnCheckEntitlement;
        _storeController.OnProductsFetched += OnProductsFetched;
        // TODO
        _storeController.OnStoreDisconnected += OnStoreDisconnected;
        _storeController.OnProductsFetchFailed += OnProductsFetchedFailed;
        _storeController.OnPurchaseFailed += OnPurchaseFailed;
        _storeController.OnPurchaseDeferred += OnPurchaseDeferred;

        await _storeController.Connect();

        FetchProducts();
    }


    void FetchProducts()
    {
        var iapConfig = DataManager.Instance.IAPConfig;
        if (iapConfig == null)
        {
            Debug.LogError("IAPConfig is not loaded in DataManager!");
            return;
        }

        var initialProductsToFetch = iapConfig.GetProductDefinitions();
        _storeController.FetchProducts(initialProductsToFetch);
    }

    void OnProductsFetched(List<Product> products)
    {
        foreach (Product product in products)
        {
            _storeController.CheckEntitlement(product);
        }
    }

    void OnCheckEntitlement(Entitlement entitlement)
    {
        Product product = entitlement.Product;
        var status = entitlement.Status;

        Debug.Log($"Entitlement checked: {product.definition.id}, Status: {status}");

        bool isEntitled = status == EntitlementStatus.FullyEntitled;
        if (isEntitled)
        {
            if (IsNoAdsProduct(product.definition.id))
            {
                // No Ads 구매가 확인됨.
                Debug.Log("No Ads entitlement confirmed.");
                IsNoAds = true;
            }
        }
    }

    void OnPurchasePending(PendingOrder order)
    {
        // Add your validations here before confirming the purchase.
        _onPurchaseCallback?.Invoke();

        // Before confirming the purchase, reward the entitlement to the player.
        _storeController.ConfirmPurchase(order);
    }

    void OnPurchaseConfirmed(Order order)
    {
        switch (order)
        {
            case FailedOrder failedOrder:
                Debug.Log($"Purchase confirmation failed: {failedOrder.CartOrdered.Items().First().Product.definition.id}, {failedOrder.FailureReason.ToString()}, {failedOrder.Details}");
                break;
            case ConfirmedOrder:
                Debug.Log($"Purchase completed:  {order.CartOrdered.Items().First().Product.definition.id}");
                break;
        }
    }

    private void OnStoreDisconnected(StoreConnectionFailureDescription obj)
    {
        Debug.LogWarning($"Store disconnected: {obj.Message}");
    }

    private void OnProductsFetchedFailed(ProductFetchFailed obj)
    {
        Debug.LogWarning($"Product fetch failed: {obj.FailureReason}");
    }

    private void OnPurchaseDeferred(DeferredOrder order)
    {
        Debug.LogWarning($"Purchase deferred: {order.CartOrdered.Items().First().Product.definition.id}");
    }

    private void OnPurchaseFailed(FailedOrder order)
    {
        Debug.LogWarning($"Purchase failed: {order.CartOrdered.Items().First().Product.definition.id}, {order.FailureReason}, {order.Details}");
    }

    void OnTransactionsRestored(bool success, string error)
    {
        Debug.Log("Transactions restored: " + success);
    }

}
