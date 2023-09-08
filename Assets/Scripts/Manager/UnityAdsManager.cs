using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityAdsManager : MonoBehaviour {

    public Text TimerText;
    public GameObject VideoImage;
    public Text RewardText;
    public Button VideoAd;

    private float TimeRemaining;
    public float InitialTime;
    public int PointsRewarded;
    // Ads Details
    private string GameID = "3282409";
    private string rewardedVideo = "rewardedVideo";

    void Start()
    {
        Advertisement.Initialize(GameID, false);
        VideoAd.GetComponent<Button>().onClick.AddListener(() => { RewardVido(); });
        TimeRemaining = PlayerPrefs.GetFloat("TimeRemaining");
        
    }

    void Update()
    {

        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            PlayerPrefs.SetFloat("TimeRemaining", TimeRemaining);
            VideoImage.SetActive(false);
            TimerText.gameObject.SetActive(true);

            float minutes = Mathf.FloorToInt(TimeRemaining / 60);
            float seconds = Mathf.FloorToInt(TimeRemaining % 60);

            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            TimeRemaining = 0;
            PlayerPrefs.SetFloat("TimeRemaining", TimeRemaining);
            VideoImage.SetActive(true);
            TimerText.gameObject.SetActive(false);
        }
    }

    public void RewardVido()
    {
        if (TimeRemaining <=0 )
        {
            PointsRewarded = Random.Range(5,10); // How many points are rewarded
            ShowRewardedAd();
        }
    }

    //use this function for showing reward ads
    public void ShowRewardedAd()
    {
        
        if (Advertisement.IsReady(rewardedVideo))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(rewardedVideo, options);
        }
        else
        {
            Debug.Log("Ads not ready");

        }
    }

    private void HandleShowResult(ShowResult result)
    {
       switch (result)
      {
            case ShowResult.Finished:
               Debug.Log("The ad was successfully shown.");

                  
              GameManager.instance.points += PointsRewarded;
              GameManager.instance.Save();
              TimeRemaining = InitialTime;
                RewardText.text = "Reward +" + PointsRewarded + " points received";
              RewardText.gameObject.SetActive(true);
                break;
            case ShowResult.Skipped:
               Debug.Log("The ad was skipped before reaching the end.");
               break;
           case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");

                break;
        }
    }

}
