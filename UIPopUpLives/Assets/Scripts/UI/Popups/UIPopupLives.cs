using System.Collections;
using UnityEngine;

public class UIPopupLives : Accessible<UIPopupLives>
{
    enum PopUpState { UseLife, RefillLife, UseRefillLife }

    PopUpState currentState;

    private void SetCurrentState(PopUpState state)
    {
        currentState = state;
    }

    [Header("Reference UI")]
    [SerializeField] private Animator currentAnimator = null;
    [SerializeField] private Transform pointerSwitcher = null;
    [SerializeField] private Transform popUpsUseLife = null;
    [SerializeField] private Transform popUpsRefillLife = null;
    [SerializeField] private Transform popUpsUseAndRefillLife = null;
    [SerializeField] private UIPanel BackgroundFade = null;
    [Header("Reference Label")]
    [SerializeField] private UILabel pointerTimeBarLabel = null;
    [SerializeField] private UILabel pointerAmountLifeBarLabel = null;

    // Таймер
    [SerializeField] private UILabel pointerUseAndRefillTimeLabel = null;
    [SerializeField] private UILabel pointerRefillTimeLabel = null;

    // Количество жизней
    [SerializeField] private UILabel pointerUseAmountLabel = null;
    [SerializeField] private UILabel pointerRefillAmountLabel = null;
    [SerializeField] private UILabel pointerUseAndRefillAmountLabel = null;

    [Header("Reference Buttons")]
    [SerializeField] private UIButton pointerButtonClose = null;
    [SerializeField] private UIButton[] pointerButtonUseLife = null;
    [SerializeField] private UIButton[] pointerButtonRefillLife = null;

    private int amountLives = 0;
    private float currentTime = 0.0f;

    string minutes, seconds;

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


            if (BackgroundFade != null)
            {
                EventDelegate.Set(BackgroundFade.GetComponent<UIButton>().onClick, delegate () { ClickBackground(); });
            }
    }

    private void Update()
    {
        if (pointerTimeBarLabel != null)
        {
            SwichState();

            string timeText;

                if (LivesManager.Current.CanRefillLives())
                {
                    CheckRefillLifes();

                    currentTime += 1 * Time.deltaTime;

                    // Используется в качестве заглушки, после 20 секунд, таймер обновляется до 0 секунд
                    minutes = Mathf.Floor((currentTime % 3600) / 60).ToString("00"); 

                    seconds = (currentTime % 60).ToString("00");

                    timeText = CustomFormatTime.formatTimeLive(minutes, seconds);
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

    private IEnumerator Process(bool isOpen)
    {
        currentAnimator.Play(Animator.StringToHash(isOpen ? "Open" : "Close"));
        currentAnimator.Update(0f);

        float animationLength = currentAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        if (isOpen)
        {
            OnShowComplete();
        }
        else
        {
            OnCloseComplete();
        }
    }

    private void OnShowComplete()
    {
      
    }

    private void OnCloseComplete()
    {
        pointerSwitcher.gameObject.SetActive(false);
    }

    public void Show()
    {
  
        pointerSwitcher.gameObject.SetActive(true);
        FadeBackground(0.5f);
        ChangeState();
        StartCoroutine(Process(true));
    }

    public void Close()
    {
        FadeBackground(0.0f);
        StartCoroutine(Process(false));
    }

    private void ClickBackground()
    {
        Close();
    }

    private void FadeBackground(float alpha)
    {
        float animationLength = currentAnimator.GetCurrentAnimatorStateInfo(0).length;
        TweenAlpha.Begin(BackgroundFade.gameObject, animationLength, alpha);
    }

    private void SwichState()
    {
       int countLives = LivesManager.Current.GetCurrentLives();

        switch (currentState)
        {
            // Обычное состояние
            case PopUpState.UseRefillLife:
                pointerUseAndRefillAmountLabel.text = countLives.ToString();
                pointerUseAndRefillTimeLabel.text = CustomFormatTime.formatTimeLive(minutes, seconds);

                popUpsUseAndRefillLife.gameObject.SetActive(true);
                popUpsRefillLife.gameObject.SetActive(false);
                popUpsUseLife.gameObject.SetActive(false);
            break;

            // Нету жизней
            case PopUpState.RefillLife:
                pointerRefillAmountLabel.text = countLives.ToString();
                pointerRefillTimeLabel.text = CustomFormatTime.formatTimeLive(minutes, seconds);

                popUpsRefillLife.gameObject.SetActive(true);
                popUpsUseLife.gameObject.SetActive(false);
                popUpsUseAndRefillLife.gameObject.SetActive(false);
            break;

            // Максимум жизней
            case PopUpState.UseLife:
                currentTime = 0;
                pointerUseAmountLabel.text = countLives.ToString();
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
