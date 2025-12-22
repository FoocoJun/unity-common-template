using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "IAPConfig", menuName = "Data/IAPConfig")]
public class IAPConfig : ScriptableObject
{
    [Header("Product Definitions")]
    [SerializeField]
    private List<ProductData> products = new List<ProductData>
    {
        new ProductData { productId = "rookiss_s2_100gold", productType = ProductType.Consumable },
        new ProductData { productId = "rookiss_s2_500gold", productType = ProductType.Consumable },
        new ProductData { productId = "rookiss_s2_noads", productType = ProductType.NonConsumable }
    };

    public List<ProductDefinition> GetProductDefinitions()
    {
        var productDefinitions = new List<ProductDefinition>();
        foreach (var product in products)
        {
            productDefinitions.Add(new ProductDefinition(product.productId, product.productType));
        }
        return productDefinitions;
    }
}

[System.Serializable]
public class ProductData
{
    public string productId;
    public ProductType productType;
    // TODO
}
