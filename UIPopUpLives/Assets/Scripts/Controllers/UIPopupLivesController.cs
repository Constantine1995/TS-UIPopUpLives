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
    [SerializeField] private UIButton pointerButtonUseLife = null;
    [SerializeField] private UIButton pointerButtonRefillLife = null;

    private int amountLives = 0;
    private int startingTime = 0;
    private float currentTime = 0.0f;

    private void Start()
    {
        currentTime = startingTime;
        amountLives = LivesManager.Current.GetCurrentLives();

        pointerTimeLabel.text = currentTime.ToString();

        pointerAmountLabel.text = amountLives.ToString();

        if (pointerButtonClose != null)
        {
            EventDelegate.Set(pointerButtonClose.onClick, delegate () { Close(); });
        }

        if (pointerButtonUseLife != null)
        {
            EventDelegate.Set(pointerButtonUseLife.onClick, delegate () { MinusLife(); });
        }

        if (pointerButtonUseLife != null)
        {
            EventDelegate.Set(pointerButtonRefillLife.onClick, delegate () { RefillLife(); });
        }
    }

    private void Update()
    {
        if (pointerTimeLabel != null)
        {
            string timeText = "";
          
            if (LivesManager.Current.CanRefillLives())
            {
                CheckRefillLifes();
                currentTime += 1 * Time.deltaTime;
                timeText = string.Format("{0:D2}:{1:D2}", 00, currentTime.ToString("##"));
            }
            else
            {
                timeText = GameManager.Current.formatTimeLiveRessurection;
            }
            pointerTimeLabel.text = timeText;
        }
    }

    private void CheckRefillLifes()
    {
        if (currentTime >= 5.0f)
        {
            LivesManager.Current.RefillOneLife();
            currentTime = 0.0f;
        }
    }

    public void Show()
    {
        pointerSwitcher.gameObject.SetActive(true);

        // Условия по которому определяется вызов необходимого содержания окна
        //  popUpsUseLife.gameObject.active = !popUpsUseLife.gameObject.active;
        popUpsUseAndRefillLife.gameObject.SetActive(true);

        if (amountLives <= 0)
        {
            print("Полные жизни, выводим FULL");
        }
    }

    public void Close()
    {
        pointerSwitcher.gameObject.SetActive(false);
    }

    private void UpdateLives()
    {
        int countLives = LivesManager.Current.GetCurrentLives();

        if (countLives < GameManager.MAX_LIVES)
        {
            print("В popUp жизни закончились");
            pointerAmountLabel.text = countLives.ToString();
        }

        if (countLives <= 0)
        {
            print("В popUp жизни закончились");
        }
        else
        {
            print("В popUp жизни не закончились");
            pointerAmountLabel.text = countLives.ToString();

        }
    }

    private void OnProfileChangeLives(int lives)
    {
        UpdateLives();
    }

    private void MinusLife()
    {
        if (amountLives != 0)
        {
            LivesManager.Current.LooseOneLife();
        }
    }

    private void RefillLife()
    {
        LivesManager.Current.RefillAllLife();
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
