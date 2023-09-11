using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string iOSURL = "";
    public string ANDROIDURL = "";
    public string fbPage = "";

    private AudioSource sound;


    public Text bestScore;
    [SerializeField]
    private Sprite[] soundBtnSprites; //1 for off and 0 for on
    public Button playBtn, rateBtn, fbLikeBtn, soundBtn, slideBtn;
    public string gameScene;

    [SerializeField]
    private Animator slideButtonAnim;

    private bool hidden;
    private bool canTouchSlideButton;

 
    private void Start()
    {
        bestScore.text = "Рекорд" + "\n" + GameManager.instance.hiScore;
        canTouchSlideButton = true;
        hidden = true;
        sound = GetComponent<AudioSource>();
        playBtn.GetComponent<Button>().onClick.AddListener(() => { PlayBtn(); });    //срабатывание метода PlayBtn() при клике по кнопке Play

        soundBtn.GetComponent<Button>().onClick.AddListener(() => { SoundBtn(); });    //sound


        
        if (PlayerPrefs.GetInt("gameMuted") == 0)
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[0];
            AudioListener.volume = 1f;
        }
        else if (PlayerPrefs.GetInt("gameMuted") == 1)
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[1];
            AudioListener.volume = 0f;

        }
    }
   

    private void PlayBtn()
    {
        sound.Play();
        SceneManager.LoadScene(gameScene);
    }

    private void RateBtn()
    {
        sound.Play();
#if UNITY_IPHONE
		Application.OpenURL(iOSURL);
#endif

#if UNITY_ANDROID
        Application.OpenURL(ANDROIDURL);
#endif
        GameManager.instance.showRate = false;
        GameManager.instance.Save();
    }

   private  void SoundBtn()  //включает выключает звук
    {
        sound.Play();

        if (GameManager.instance.isMusicOn)
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[1];
            GameManager.instance.isMusicOn = false;
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("gameMuted", 1);
            GameManager.instance.Save();
        }
        else
        {
            soundBtn.transform.GetChild(0).GetComponent<Image>().sprite = soundBtnSprites[0];
            GameManager.instance.isMusicOn = true;
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("gameMuted", 0);
            GameManager.instance.Save();

        }
    }

    void FBlikeBtn()
    {
        sound.Play();
        Application.OpenURL(fbPage);
    }

    void SlideBtn()
    {
        sound.Play();
        StartCoroutine(DisableSlideBtnWhilePlayingAnimation());
    }

    IEnumerator DisableSlideBtnWhilePlayingAnimation()
    {
        if (canTouchSlideButton)
        {
            if (hidden)
            {
                canTouchSlideButton = false;
                slideButtonAnim.Play("SlideIn");
                hidden = false;
                yield return new WaitForSeconds(1.2f);
                canTouchSlideButton = true;

            }
            else
            {
                canTouchSlideButton = false;
                slideButtonAnim.Play("SlideOut");
                hidden = true;
                yield return new WaitForSeconds(1.2f);
                canTouchSlideButton = true;
                slideButtonAnim.Play("SlideIdle");

            }

        }
    }

}