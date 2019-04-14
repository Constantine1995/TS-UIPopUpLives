using UnityEngine;

public class UIPopupBarLifeController : MonoBehaviour
{
    [Header("Reference Buttons")]
    [SerializeField] private UIButton pointerButtonLifeBar;

    void Start()
    {
        if (pointerButtonLifeBar != null)
        {
            EventDelegate.Set(pointerButtonLifeBar.onClick, delegate () { OpenPopUp(); });
        }
    }

    public void OpenPopUp()
    {
        UIPopupLivesController.Current.Show();
    }

}
