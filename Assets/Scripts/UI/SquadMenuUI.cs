using UnityEngine;

public class SquadMenuUI : MonoBehaviour
{
    public void CloseSquadMenuClick()
    {
        GameplayUI.Instance.SquadMenuClose();
    }
}