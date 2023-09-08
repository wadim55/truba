using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameSceneAdsManager : MonoBehaviour
{
    // Ads Details
    private string GameID = "3282409";
    private string BannerAd = "BannerAd";

    private int i;
    private float TimeRemaining;
    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(GameID, false);
        i = 0;
        TimeRemaining = PlayerPrefs.GetFloat("TimeRemaining");
    }

    // Update is called once per frame
    void Update()
    {
        
        if (GameManager.instance.isGameOver == true && GameManager.instance.canShowAds)
        {
            if (i == 0 && PlayerPrefs.GetInt("NbTimePlayed") % 3 == 0) // show ads after playing 3 times
            {
                ShowAd();
                i++;
            }
        }


        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            PlayerPrefs.SetFloat("TimeRemaining", TimeRemaining);
        }
        else
        {
            TimeRemaining = 0;
            PlayerPrefs.SetFloat("TimeRemaining", TimeRemaining);
        }
    }

    public void ShowAd()
    {
        //import unity ads

        if (Advertisement.IsReady(BannerAd))
        {
            Advertisement.Show(BannerAd);
        }
    }
}
