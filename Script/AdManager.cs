using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;
using TapsellSDK;
using TapsellSimpleJSON;
using ArabicSupport;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    public static bool available = false;
    public static bool bannerIsHidden = true;
    public static TapsellAd ad = null;
    public static TapsellNativeBannerAd nativeAd = null;
    
    public string appkey = "irfacdremcbehfifpkggeccecjgersagdtnlmjgdekorddhkpsafahaaehsadbqgqlppoq";
    public string bannerZoneId = "5c7a8aba482e9a0001582961";
    public string InterstitialID = "";
    public string RewardID = "";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {



        // Use your tapsell key for initialization
        Tapsell.initialize(appkey);

        Debug.Log("Tapsell Version: " + Tapsell.getVersion());
        Tapsell.setDebugMode(false);
        Tapsell.setPermissionHandlerConfig(Tapsell.PERMISSION_HANDLER_AUTO);
        Tapsell.setRewardListener(
            (TapsellAdFinishedResult result) =>
            {
                // onFinished, you may give rewards to user if result.completed and result.rewarded are both True
                Debug.Log("onFinished, adId:" + result.adId + ", zoneId:" + result.zoneId + ", completed:" + result.completed + ", rewarded:" + result.rewarded);

                // You can validate suggestion from you server by sending a request from your game server to tapsell, passing adId to validate it
                if (result.completed && result.rewarded)
                {
                    validateSuggestion(result.adId);
                }
            }
        );
        
        Tapsell.requestBannerAd(bannerZoneId, BannerType.BANNER_320x50, Gravity.BOTTOM, Gravity.CENTER,
        (string zoneId) => {
            Debug.Log("Action: onBannerRequestFilledAction");
            bannerIsHidden = false;
        },
        (string zoneId) => {
            Debug.Log("Action: onNoBannerAdAvailableAction");
        },
        (TapsellError tapsellError) => {
            Debug.Log("Action: onBannerAdErrorAction");
        },
        (string zoneId) => {
            Debug.Log("Action: onNoNetworkAction");
        },
        (string zoneId) => {
            Debug.Log("Action: onHideBannerAction");
            bannerIsHidden = true;
        });
        RequestRewardedVideo();
    }

    public void validateSuggestion(string suggestionId)
    {
        try
        {
            string ourPostData = "{\"suggestionId\":\"" + suggestionId + "\"}";
            System.Collections.Generic.Dictionary<string, string> headers = new System.Collections.Generic.Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");

            byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());

            WWW api = new WWW("http://api.tapsell.ir/v2/suggestions/validate-suggestion", pData, headers);
            StartCoroutine(WaitForRequest(api));
        }
        catch (UnityException ex)
        {
            Debug.Log(ex.Message);
        }
        return;
    }

    IEnumerator WaitForRequest(WWW data)
    {
        Debug.Log("my start waiting...");
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            Debug.Log("my server error is " + data.error);
        }
        else
        {
            Debug.Log("my server result is " + data.text);

            JSONNode node = JSON.Parse(data.text);
            bool valid = node["valid"].AsBool;
            if (valid)
            {
                // if suggestion is valid, you can give in game gifts to the user
                Debug.Log("Ad is valid");
            }
            else
            {
                Debug.Log("Ad is not valid");
            }
        }
    }

    private void requestAd(string zone, bool cached)
    {
        Tapsell.requestAd(zone, cached,
            (TapsellAd result) => {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                available = true;
                ad = result;
            },

            (string zoneId) => {
                // onNoAdAvailable
                Debug.Log("No Ad Available");
            },

            (TapsellError error) => {
                // onError
                Debug.Log(error.error);
            },

            (string zoneId) => {
                // onNoNetwork
                Debug.Log("No Network: " + zoneId);
            },

            (TapsellAd result) => {
                //onExpiring
                Debug.Log("Expiring");
                available = false;
                ad = null;
                requestAd(result.zoneId, false);
            }

        );
    }

    void RequestInterstitial()
    {
        requestAd(InterstitialID, false);
    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        RequestInterstitial();
    }

    public void ShowInterstitial()
    {
        RequestInterstitial();
        available = false;
        TapsellShowOptions options = new TapsellShowOptions();
        options.backDisabled = false;
        options.immersiveMode = false;
        options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_LANDSCAPE;
        options.showDialog = true;
        Tapsell.showAd(ad, options);
    }

    void RequestRewardedVideo()
    {
        requestAd(RewardID, false);

        Tapsell.setRewardListener((TapsellAdFinishedResult result) =>
        {
            int coin = PlayerPrefs.GetInt("coin");
            coin += 100;
            PlayerPrefs.SetInt("coin", coin);

        });
    }

    public void ShowRewardedVideo()
    {
        RequestRewardedVideo();
        Debug.Log("ShowRewardedVideo");
        available = false;
        TapsellShowOptions options = new TapsellShowOptions();
        options.backDisabled = false;
        options.immersiveMode = false;
        options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_LANDSCAPE;
        options.showDialog = true;
        Tapsell.showAd(ad, options);
    }
    public void hideBanner_area()
    {
        Tapsell.hideBannerAd(bannerZoneId);
    }
}
