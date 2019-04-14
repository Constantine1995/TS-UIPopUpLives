using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LivesManager : Accessible<LivesManager> {

    private int currentLives;

    private int extraLives = 5;

    private string regenerationTimestamp;

    private string unlimitedTimestamp;

    private int REFILL_LIFE_SECONDS = 20;

    public static UnityAction<int> OnProfileChangeLives = delegate { };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Load local variables with correct values
        currentLives = 5;
        extraLives = 5;

        // If you dont have full lives, start the refill timer
        if (currentLives < getMaxNumberOfLives())
            InvokeRepeating("checkRegenerationTime", 0.0f, 1.0f);

        // If you have unlimited lives, start the unlimited lives timer
        if (isUnlimitedLives())
            InvokeRepeating("checkUnlimitedTime", 0.0f, 1.0f);
    }

    public bool IsFullLives
    {
        get
        {
            return currentLives >= getMaxNumberOfLives();
        }
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public bool CanRefillLives()
    {
        return currentLives < getMaxNumberOfLives() && !isUnlimitedLives();
    }

    public void LooseOneLife()
    {
        Debug.Log("LooseOneLife currentLives: " + currentLives);

        // If you already have 0 lives, do nothing
        if (currentLives <= 0)
            return;

        // Update current lives variable
        currentLives--;
        OnProfileChangeLives(currentLives);
        // If you had full lives, start the refill timer
      /*  if (currentLives == getMaxNumberOfLives() - 1)
        {
          //  setLifeRegenerationTimer();
            InvokeRepeating("checkRegenerationTime", 0.0f, 1.0f);
            print("----");
        }*/
    }

    void checkRegenerationTime()
    {
      //  double refillSecondsLeft = GetRefillSecondsLeft();
/*
        if (refillSecondsLeft <= 0)
        {
            int numberOfLivesToRestore = 1;
            numberOfLivesToRestore += (int)Mathf.Abs((float)refillSecondsLeft) / REFILL_LIFE_SECONDS;
            RefillXLives(numberOfLivesToRestore);

            //Overwrite regeneration time stamp with seconds left
            int secondsLeft = (int)Mathf.Abs((float)refillSecondsLeft) % REFILL_LIFE_SECONDS;
            if (secondsLeft > 0)
            {
                regenerationTimestamp = (getCurrentTimeInSeconds() + REFILL_LIFE_SECONDS - secondsLeft).ToString();
            }
        }*/

     //   if (currentLives == getMaxNumberOfLives())
       // {
       //     CancelInvoke("checkRegenerationTime");
       //     regenerationTimestamp = "0";

      //  }
        //updateUserInterface();
    }

    public void RefillXLives(int livesToAdd)
    {
        for (int i = 0; i < livesToAdd; i++)
            RefillOneLife();
    }

    // Add 1 life
    public void RefillOneLife()
    {
        // If you already have full lives, do nothing
       if (currentLives == getMaxNumberOfLives())
            return;
        currentLives++;
        OnProfileChangeLives(currentLives);
    }

    // Add all life
    public void RefillAllLife()
    {
        if (currentLives == getMaxNumberOfLives())
            return;
        currentLives = GameManager.MAX_LIVES;
        OnProfileChangeLives(currentLives);
    }


    //void setLifeRegenerationTimer()
    // {
    //     regenerationTimestamp = (getCurrentTimeInSeconds() + REFILL_LIFE_SECONDS//LMConfig.REFILL_LIFE_SECONDS).ToString();
    // }

    public double GetRefillSecondsLeft()
    {
        return getCurrentTimeInSeconds();
    }


    private double getUnlimitedSecondsLeft()
    {
        if (!string.IsNullOrEmpty(unlimitedTimestamp))
        {
            double unlimitedTimestampValue;
            if (double.TryParse(unlimitedTimestamp, out unlimitedTimestampValue))
            {
                return unlimitedTimestampValue - getCurrentTimeInSeconds();
            }
        }
        return 0;
    }

    private double getCurrentTimeInSeconds()
    {
        var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
        return timestamp;
    }


    public bool CanLooseLife()
    {
        return currentLives > 0 && !isUnlimitedLives();
    }

    private int getMaxNumberOfLives()
    {
        return  extraLives;
    }

    private bool isUnlimitedLives()
    {
        return getUnlimitedSecondsLeft() > 0;
    }

}
