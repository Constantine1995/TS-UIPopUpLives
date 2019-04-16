using UnityEngine.Events;

public class LivesManager
{
    public static UnityAction<int> OnProfileChangeLives = delegate { };

    public int currentLives = Config.MAX_LIVES;

    // Проверка на полные жизни
    public bool IsFullLives
    {
        get
        {
            return currentLives >= getMaxNumberOfLives();
        }
    }

    // Проверка на полное отсутствие жизней
    public bool IsNoLives
    {
        get
        {
            return currentLives == 0;
        }
    }

    // Обычное состяоние для pop-up Use Refil Life
    public bool IsNormalState
    {
        get
        {
            return !IsNoLives && !IsFullLives;
        }
    }

    // Проверка на возможность заполнить жизни
    public bool CanRefillLives()
    {
        return currentLives < getMaxNumberOfLives();
    }

    // Проверка на возможность отнять жизнь
    public bool CanLoseLife()
    {
        return currentLives > 0;
    }

    // Получение текущих жизней
    public int GetCurrentLives()
    {
        return currentLives;
    }

    // Потеря жизни
    public void LoseOneLife()
    {
        if (currentLives <= 0)
            return;
        currentLives--;
        OnProfileChangeLives(currentLives);
    }

    // Добавление 1 жизни
    public void RefillOneLife()
    {
       if (currentLives == getMaxNumberOfLives())
            return;
        currentLives++;
        OnProfileChangeLives(currentLives);
    }

    // Восполнение всех жизней
    public void RefillAllLife()
    {
        if (currentLives == getMaxNumberOfLives())
            return;

        currentLives = Config.MAX_LIVES;
        OnProfileChangeLives(currentLives);
    }

    // Максимальное количество жизней
    private int getMaxNumberOfLives()
    {
        return Config.MAX_LIVES;
    }

}
