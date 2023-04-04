using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionsLabel : MonoBehaviour
{
    [SerializeField] private TextMeshPro daysLeftText;
    [SerializeField] private TextMeshPro dayActionsLeftText;
    [SerializeField] private TextMeshPro actionsPerDayText;
    
    public void SetDaysLeft(int daysLeft)
    {
        daysLeftText.text = daysLeft.ToString();
    }
    
    public void SetActionsLeft(int actionsLeft)
    {
        dayActionsLeftText.text = actionsLeft.ToString();
    }
    
    public void SetActionsPerDay(int actionsPerDay)
    {
        actionsPerDayText.text = actionsPerDay.ToString();
    }
}
