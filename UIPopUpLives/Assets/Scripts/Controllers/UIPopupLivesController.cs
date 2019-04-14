using UnityEngine;

public class UIPopupLivesController : Accessible<UIPopupLivesController>
{
    [Header("Reference UI")]
    [SerializeField] private Transform pointerSwitcher;
    [SerializeField] private Transform popUpsUseLife;
    [SerializeField] private Transform popUpsRefillLife;
    [SerializeField] private Transform popUpsUseAndRefillLife;

    [Header("Reference Buttons")]
    [SerializeField] private UIButton pointerButtonClose = null;

    private void Start()
    {
        if (pointerButtonClose != null)
        {
            EventDelegate.Set(pointerButtonClose.onClick, delegate () { Close(); });
        }
    }

    public void Show()
    {
        pointerSwitcher.gameObject.SetActive(true);
        
        // Условия по которому определяется вызов необходимого содержания окна
        popUpsUseLife.gameObject.active = !popUpsUseLife.gameObject.active; 
    }

    public void Close()
    {
        pointerSwitcher.gameObject.SetActive(false);
    }
}
