using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* Apache License. Copyright (C) Bobardo Studio - All Rights Reserved.
 * Unauthorized publishing the plugin with different name is strictly prohibited.
 * This plugin is free and no one has right to sell it to others.
 * http://bobardo.com
 * http://opensource.org/licenses/Apache-2.0
 */

[RequireComponent(typeof(StoreHandler))]
public class InAppStore : MonoBehaviour
{
    public Product[] products;


    private int coin = 0;
    private int gem = 0;

    public GameObject purchasedFailed_panel;
    public GameObject purchasedSuccessful_panel;

    private int selectedProductIndex;

    void Start()
    {

    }

    public void purchasedSuccessful(Purchase purchase)
    {
        purchasedSuccessful_panel.SetActive(true);

        switch (selectedProductIndex)
        {
            case 0: 
                increse_Gem(80);
                break;
            case 1:
                increse_Gem(200);
                break;
            case 2:
                increse_Gem(360);
                break;
            case 3:
                increse_Gem(1000);
                break;
            case 4:
                increse_Coin(1000);
                break;
            case 5:
                increse_Coin(10000);
                break;
            case 6:
                increse_Coin(100000);
                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
        }
        
    }

    public void purchasedFailed(int errorCode, string info)
    {
        // purchase failed. show user the proper message
        purchasedFailed_panel.SetActive(true);
        switch (errorCode)
        {
            case 1: // error connecting cafeBazaar
            case 2: // error connecting cafeBazaar
            case 4: // error connecting cafeBazaar
            case 5: // error connecting cafeBazaar

                break;
            case 6: // user canceled the purchase

                break;
            case 7: // purchase failed

                break;
            case 8: // failed to consume product. but the purchase was successful.

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }

    }

    public void userHasThisProduct(Purchase purchase)
    {
        // user already has this product
        switch (selectedProductIndex)
        {
            case 0: // first product

                break;
            case 1: // second product

                break;
            default:
                throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
        }
    }

    public void failToGetUserInventory(int errorCode, string info)
    {
        // user has not this product or some error happened
        switch (errorCode)
        {
            case 3:  // error connecting cafeBazaar
            case 10: // error connecting cafeBazaar

                break;
            case 9: // user didn't login to cafeBazaar

                break;
            case 11: // user has not this product

                break;
            case 12: // error setup cafebazaar billing
            case 13: // error setup cafebazaar billing
            case 14: // error setup cafebazaar billing

                break;
            case 15: // you should enter your public key

                break;
            case 16: // unkown error happened

                break;
            case 17: // the result from cafeBazaar is not valid.

                break;
        }

    }

    public void purchaseProduct(int productIndex)
    {
        selectedProductIndex = productIndex;
        Product product = products[productIndex];
        if (product.type == Product.ProductType.Consumable)
        {
            GetComponent<StoreHandler>().BuyAndConsume(product.productId);
        }
        else if (product.type == Product.ProductType.NonConsumable)
        {
            GetComponent<StoreHandler>().BuyProduct(product.productId);
        }
    }

    public void checkIfUserHasProduct(int productIndex)
    {
        selectedProductIndex = productIndex;
        GetComponent<StoreHandler>().CheckInventory(products[productIndex].productId);
    }


    void increse_Gem(int some_gem)
    {
        int gem = PlayerPrefs.GetInt("gem");
        gem += some_gem;
        PlayerPrefs.SetInt("gem", gem);
    }
    void increse_Coin(int some_coin)
    {
        int coin = PlayerPrefs.GetInt("coin");
        coin += some_coin;
        PlayerPrefs.SetInt("coin", coin);
    }
}



