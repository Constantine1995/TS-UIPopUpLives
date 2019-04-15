using UnityEngine;

public class UIPopupLivesController : Accessible<UIPopupLivesController>
{
    [Header("Reference UI")]
    [SerializeField] private Transform pointerSwitcher = null;
    [SerializeField] private Transform popUpsUseLife = null;
    [SerializeField] private Transform popUpsRefillLife = null;
    [SerializeField] private Transform popUpsUseAndRefillLife = null;
    [Header("Reference Label")]
    [SerializeField] private UILabel pointerTimeBarLabel = null;
    [SerializeField] private UILabel pointerAmountLifeBarLabel = null;
    [SerializeField] private UILabel pointerUseAndRefillTimeLabel = null;
    [SerializeField] private UILabel pointerUseAndRefillAmountLabel = null;
    [Header("Reference Buttons")]
    [SerializeField] private UIButton pointerButtonClose = null;
    [SerializeField] private UIButton[] pointerButtonUseLife = null;
    [SerializeField] private UIButton[] pointerButtonRefillLife = null;

    private float amountLives = 0.0f;
    private float currentTime = 0.0f;

    [SerializeField] private bool isFullLifes = false;

    enum PopUpState { UseLife, RefillLife, UseRefillLife }

    PopUpState currentState;

    private void SetCurrentState(PopUpState state)
    {
        currentState = state;
    }

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
        SwichState();
        if (pointerTimeBarLabel != null)
        {
            string timeText;
                if (LivesManager.Current.CanRefillLives())
                {
                    CheckRefillLifes();


                currentTime += 1 * Time.deltaTime;

                // Используется в качестве заглушки, после 20 секунд, таймер обновляется до 0 секунд
                string minutes = Mathf.Floor((currentTime % 3600) / 60).ToString("00"); 

                string seconds = (currentTime % 60).ToString("00");

                timeText = string.Format("{0:D2}:{1:D2}", minutes, seconds);

                }
                else
                {
                    timeText = Config.TEXT_FULL_LIVES;
                }

                pointerTimeBarLabel.text = timeText;
        }
       
    }

    // Инициализация данных
    private void Init()
    {
        SetCurrentState(PopUpState.UseLife);
        amountLives = LivesManager.Current.GetCurrentLives();
        pointerTimeBarLabel.text = currentTime.ToString();
        pointerAmountLifeBarLabel.text = amountLives.ToString();
    }

    public void Show()
    {
        pointerSwitcher.gameObject.SetActive(true);
        ChangeState();
    }

    private void SwichState()
    {
        switch (currentState)
        {
            // Обычное состояние
            case PopUpState.UseRefillLife:
                popUpsUseAndRefillLife.gameObject.SetActive(true);
                popUpsRefillLife.gameObject.SetActive(false);
                popUpsUseLife.gameObject.SetActive(false);
            break;

            // Нету жизней
            case PopUpState.RefillLife:
                popUpsRefillLife.gameObject.SetActive(true);
                popUpsUseLife.gameObject.SetActive(false);
                popUpsUseAndRefillLife.gameObject.SetActive(false);
            break;

            // Максимум жизней
            case PopUpState.UseLife:
                currentTime = 0;
                popUpsUseLife.gameObject.SetActive(true);
                popUpsRefillLife.gameObject.SetActive(false);
                popUpsUseAndRefillLife.gameObject.SetActive(false);
            break;
        }
    }

    private void ChangeState()
    {
        // Обычное состояние
        if (LivesManager.Current.IsNormalState)
        {
            SetCurrentState(PopUpState.UseRefillLife);
        }

        // Нету жизней
        if (LivesManager.Current.IsNoLives)
        {
            SetCurrentState(PopUpState.RefillLife);
        }

        // Максимум жизней
        if (LivesManager.Current.IsFullLives)
        {
            SetCurrentState(PopUpState.UseLife);
        }
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
        pointerAmountLifeBarLabel.text = countLives.ToString();

        ChangeState();
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
