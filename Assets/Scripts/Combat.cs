using UnityEngine;

public class Combat : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        maxHealth += playerStats.endurance * 10; // Начальное здоровье с учетом выносливости
        currentHealth = maxHealth;
    }

    public void Attack(EnemyAI enemy)
    {
        int damage = 10 + playerStats.strength * 2; // Базовый урон + бонус от силы
        enemy.TakeDamage(damage);
        Debug.Log("Вы нанесли " + damage + " урона.");
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - playerStats.endurance, 1); // Выносливость уменьшает получаемый урон
        currentHealth -= finalDamage;
        Debug.Log(gameObject.name + " получил урон: " + finalDamage + ". Оставшееся здоровье: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " был убит.");
        Destroy(gameObject);
    }
}
