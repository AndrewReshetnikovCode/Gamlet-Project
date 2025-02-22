using UnityEngine;

public class PlayerProgression : MonoBehaviour
{
    public int level = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>(); 
    }

    public void GainXP(int xp)
    {
        currentXP += xp;
        Debug.Log("Получено " + xp + " XP.");

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentXP = 0;
        xpToNextLevel += 50;
        playerStats.LevelUp(); // Увеличиваем количество очков для распределения

        Debug.Log("Поздравляем! Вы достигли уровня " + level);
    }
}
