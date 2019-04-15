using UnityEngine;

public class UIPopupBarLife : MonoBehaviour
{
    [Header("Reference Buttons")]
    [SerializeField] private UIButton pointerButtonLifeBar;

    void Start()
    {
        if (pointerButtonLifeBar != null)
        {
            EventDelegate.Set(pointerButtonLifeBar.onClick, delegate () { Show(); });
        }
    }

    public void Show()
    {
        UIPopupLives.Current.Show();
    }
}
