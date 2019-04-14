using UnityEngine;

public class UIPopupLivesController : Accessible<UIPopupLivesController>
{
    [Header("Reference UI")]
    [SerializeField] private Transform pointerSwitcher = null;
    [SerializeField] private Transform popUpsUseLife = null;
    [SerializeField] private Transform popUpsRefillLife = null;
    [SerializeField] private Transform popUpsUseAndRefillLife = null;
    [Header("Reference Label")]
    [SerializeField] private UILabel pointerTimeLabel = null;
    [SerializeField] private UILabel pointerAmountLabel = null;
    [Header("Reference Buttons")]
    [SerializeField] private UIButton pointerButtonClose = null;
    [SerializeField] private UIButton[] pointerButtonUseLife = null;
    [SerializeField] private UIButton[] pointerButtonRefillLife = null;

    private float amountLives = 0.0f;
    private float currentTime = 0.0f;

    [SerializeField] private bool isFullLifes = false;

    private void Start()
    {
        Init();

        if (pointerButtonClose != null)
        {
            EventDelegate.Set(pointerButtonClose.onClick, delegate () { Close(); });
        }
        for (int i = 0; i < pointerButtonUseLife.Length; i++)
        {
            if (pointerButtonUseLife != null)
            {
                EventDelegate.Set(pointerButtonUseLife[i].onClick, delegate () { LoseLife(); });
            }
        }

        for (int i = 0; i < pointerButtonRefillLife.Length; i++)
        {
            if (pointerButtonUseLife != null)
            {
                EventDelegate.Set(pointerButtonRefillLife[i].onClick, delegate () { RefillLife(); });
            }
        }
      
    }

    private void Update()
    {
        if (pointerTimeLabel != null)
        {
            string timeText;
                if (LivesManager.Current.CanRefillLives())
                {
                    CheckRefillLifes();


                currentTime += 1 * Time.deltaTime;

                // Используется в качестве заглушки, после 20 секунд таймер обновляется до 0 секунд
                string minutes = Mathf.Floor((currentTime % 3600) / 60).ToString("00"); 

                string seconds = (currentTime % 60).ToString("00");

                timeText = string.Format("{0:D2}:{1:D2}", minutes, seconds);

                }
                else
                {
                    timeText = Config.TEXT_FULL_LIVES;
                }

                pointerTimeLabel.text = timeText;
        }
    }

    // Инициализация данных
    private void Init()
    {
        amountLives = LivesManager.Current.GetCurrentLives();
        pointerTimeLabel.text = currentTime.ToString();
        pointerAmountLabel.text = amountLives.ToString();
    }

    public void Show()
    {
        pointerSwitcher.gameObject.SetActive(true);

        // Условия по которому определяется вызов необходимого содержания окна
        popUpsUseAndRefillLife.gameObject.SetActive(true);

        if (LivesManager.Current.IsFullLives)
        {
            print("Полные жизни, выводим FULL");
        }

       /* if (amountLives <= 0)
        {
            print("Полные жизни, выводим FULL");
        }*/
    }

    public void Close()
    {
        pointerSwitcher.gameObject.SetActive(false);
    }

    // Добавляем 1 жизнь каждые 20 секунд
    private void CheckRefillLifes()
    {
        if (currentTime >= Config.REFILL_LIFE_SECONDS)
        {
           currentTime = 0;
           LivesManager.Current.RefillOneLife();
        }
    }

    // Обновляет данные жизней
    private void UpdateLives()
    {
        int countLives = LivesManager.Current.GetCurrentLives();

        if (countLives < Config.MAX_LIVES)
        {
            print("В popUp жизни закончились");
            pointerAmountLabel.text = countLives.ToString();
        }

        if (countLives <= 0)
        {
            print("В popUp жизни закончились " + countLives);
        }
        else
        {
            print("В popUp жизни не закончились " + countLives);
            pointerAmountLabel.text = countLives.ToString();
        }
    }

    // Отнимает одну жизнь
    private void LoseLife()
    { 
        if (LivesManager.Current.CanLoseLife())
        {
            LivesManager.Current.LoseOneLife();
        }
    }

    // Восполнить все жизни
    private void RefillLife()
    {
        LivesManager.Current.RefillAllLife();
    }

    private void OnProfileChangeLives(int lives)
    {
        UpdateLives();
    }

    private void OnEnable()
    {
        LivesManager.OnProfileChangeLives += OnProfileChangeLives;
    }

    private void OnDisable()
    {
        LivesManager.OnProfileChangeLives -= OnProfileChangeLives;
    }
}
