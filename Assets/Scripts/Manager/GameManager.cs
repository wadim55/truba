using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// Этот скрипт помогает сохранять и загружать данные в устройство
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private GameData data;

    //данные, которые не хранятся на устройстве, но передаются во время игры
    public bool isGameOver = false;
    public int currentScore;

    //данные для хранения на устройстве
    public bool isGameStartedFirstTime;
    public bool isMusicOn;
    public int hiScore, points, textureStyle;
    public bool canShowAds;//когда значение no Ads равно false, мы показываем рекламу, а когда это правда, мы ее не показываем
    public bool showRate;
    public bool[] textureUnlocked;
    

    void Awake()
    {
        MakeInstance();
        InitializeVariables();
        
    }


    void MakeInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //здесь мы инициализируем переменные
    void InitializeVariables()
    {
        //сначала мы загружаем любые доступные данные
        Load();
        if (data != null)
        {
            isGameStartedFirstTime = data.getIsGameStartedFirstTime();
        }
        else
        {
            isGameStartedFirstTime = true;
        }
        if (isGameStartedFirstTime)
        {
            //когда игра запускается в первый раз на устройстве, мы устанавливаем начальные значения
            isGameStartedFirstTime = false;
            hiScore = 0;
            points = 0;
            textureStyle = 0;
            textureUnlocked = new bool[4];
            textureUnlocked[0] = true;
            for (int i = 1; i < textureUnlocked.Length; i++)
            {
                textureUnlocked[i] = false;
            }
            isMusicOn = true;
            canShowAds = true;
            showRate = true;
            data = new GameData();

            //хранение данных
            data.setIsGameStartedFirstTime(isGameStartedFirstTime);
            data.setIsMusicOn(isMusicOn);
            data.setHiScore(hiScore);
            data.setPoints(points);
            data.setTexture(textureStyle);
            data.setTextureUnlocked(textureUnlocked);
            data.setCanShowAds(canShowAds);
            data.setShowRate(showRate);
            Save();
            Load();
        }
        else
        {
            //получение данных
            isGameStartedFirstTime = data.getIsGameStartedFirstTime();
            isMusicOn = data.getIsMusicOn();
            hiScore = data.getHiScore();
            points = data.getPoints();
            textureStyle = data.getTexture();
            textureUnlocked = data.getTextureUnlocked();
            canShowAds = data.getCanShowAds();
            showRate = data.getShowRate();      
        }
    }

    
    //метод сохранения данных
    public void Save()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "/GameInfo.dat");
            if (data != null)
            {
                data.setIsGameStartedFirstTime(isGameStartedFirstTime);
                data.setHiScore(hiScore);
                data.setPoints(points);
                data.setTexture(textureStyle);
                data.setTextureUnlocked(textureUnlocked);
                data.setIsMusicOn(isMusicOn);
                data.setCanShowAds(canShowAds);
                data.setShowRate(showRate);
                bf.Serialize(file, data);
            }
        }
        catch (Exception e)
        { }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    //способ загрузки данных
    public void Load()
    {
        FileStream file = null;
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "/GameInfo.dat", FileMode.Open);//здесь мы получаем сохраненный файл
            data = (GameData)bf.Deserialize(file);
        }
        catch (Exception e) { }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }
}

[Serializable]
class GameData
{
    private bool isGameStartedFirstTime;
    private bool isMusicOn;
    private int hiScore, points, textureStyle;
    private bool[] textureUnlocked;
    private bool canShowAds;
    private bool showRate;

    //игра запущена в 1-й раз
    public void setIsGameStartedFirstTime(bool isGameStartedFirstTime)
    {
        this.isGameStartedFirstTime = isGameStartedFirstTime;
    }

    public bool getIsGameStartedFirstTime()
    {
        return isGameStartedFirstTime;
    }

    //ads
    public void setCanShowAds(bool canShowAds)
    {
        this.canShowAds = canShowAds;
    }

    public bool getCanShowAds()
    {
        return canShowAds;
    }
    
    public void setShowRate(bool showRate)
    {
        this.showRate = showRate;
    }

    public bool getShowRate()
    {
        return showRate;
    }

    //музыка
    public void setIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }

    public bool getIsMusicOn()
    {
        return isMusicOn;
    }

    //добавыление счета
    public void setHiScore(int hiScore)
    {
        this.hiScore = hiScore;
    }

    public int getHiScore()
    {
        return hiScore;
    }

    //запись счета
    public void setPoints(int points)
    {
        this.points = points;
    }

    public int getPoints()
    {
        return points;
    }
    
    public void setTexture(int textureStyle)
    {
        this.textureStyle = textureStyle;
    }

    public int getTexture()
    {
        return textureStyle;
    }
    
    public void setTextureUnlocked(bool[] textureUnlocked)
    {
        this.textureUnlocked = textureUnlocked;
    }

    public bool[] getTextureUnlocked()
    {
        return textureUnlocked;
    }
}