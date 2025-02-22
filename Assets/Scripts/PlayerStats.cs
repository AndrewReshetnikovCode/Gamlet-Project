using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int strength = 1;
    public int agility = 1;
    public int endurance = 1;
    public int intelligence = 1;

    public int pointsToAllocate = 0;

    private Combat playerCombat;
    private PlayerProgression playerProgression;

    void Start()
    {
        playerCombat = GetComponent<Combat>();
        playerProgression = GetComponent<PlayerProgression>();
    }

    public void AllocatePoint(string stat)
    {
        if (pointsToAllocate > 0)
        {
            switch (stat)
            {
                case "Strength":
                    strength++;
                    break;
                case "Agility":
                    agility++;
                    break;
                case "Endurance":
                    endurance++;
                    playerCombat.maxHealth += 10; // увеличиваем здоровье при прокачке выносливости
                    break;
                case "Intelligence":
                    intelligence++;
                    break;
            }

            pointsToAllocate--;
            Debug.Log($"Повышена {stat}. Осталось очков: {pointsToAllocate}");
        }
        else
        {
            Debug.Log("Нет доступных очков для распределения.");
        }
    }

    public void LevelUp()
    {
        pointsToAllocate += 2; // Даем игроку 2 очка при каждом уровне
        Debug.Log("Повышен уровень! Доступные очки для распределения: " + pointsToAllocate);
    }
}
