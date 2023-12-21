using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class ADManager : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private bool IsAdShow;
    public int adsCounter = 2;

    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        this.RequestBanner();
        this.Request�nterstitial();
    }

    private void Update()
    {
        if (!IsAdShow)
        {
            if (this.interstitial.IsLoaded())
            {
                if (GameManager.Instance.adCounter >= adsCounter)
                {
                    this.interstitial.Show();

                    //IsAdShow = true;
                    GameManager.Instance.adCounter = 0;

                    Request�nterstitial();

                    Debug.Log("Reklam G�sterildi ve y�klendi");
                }

            }
            else
            {
                Debug.Log("FullScreenAd daha y�klenmedi!!");
            }
        }

    }

    void RequestBanner()
    {
#if UNITY_ANDROID
        string reklamID = "ca-app-pub-3940256099942544/6300978111";
#else
        string reklamID = "unexpected_platform";
#endif


        this.bannerView = new BannerView(reklamID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);
    }

    void Request�nterstitial()
    {
#if UNITY_ANDROID
        string reklamID = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string reklamID = "unexpected_platform";
#endif


        this.interstitial = new InterstitialAd(reklamID);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

}
