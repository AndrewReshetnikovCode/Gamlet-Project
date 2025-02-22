using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public PlayerStats playerStats;

    public Text strengthText;
    public Text agilityText;
    public Text enduranceText;
    public Text intelligenceText;
    public Text pointsText;

    void Update()
    {
        strengthText.text = "Сила: " + playerStats.strength;
        agilityText.text = "Ловкость: " + playerStats.agility;
        enduranceText.text = "Выносливость: " + playerStats.endurance;
        intelligenceText.text = "Интеллект: " + playerStats.intelligence;
        pointsText.text = "Очки для распределения: " + playerStats.pointsToAllocate;
    }

    public void IncreaseStrength()
    {
        playerStats.AllocatePoint("Strength");
    }

    public void IncreaseAgility()
    {
        playerStats.AllocatePoint("Agility");
    }

    public void IncreaseEndurance()
    {
        playerStats.AllocatePoint("Endurance");
    }

    public void IncreaseIntelligence()
    {
        playerStats.AllocatePoint("Intelligence");
    }
}
