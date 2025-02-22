using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Максимальное здоровье
    [SerializeField] private float passiveHealRate = 1f; // Скорость пассивного восстановления здоровья в секунду
    [SerializeField] private float healInterval = 1f; // Интервал восстановления здоровья

    [SerializeField] private AudioSource damageAudioSource; // Звук получения урона
    [SerializeField] private AudioClip damageSound; // Аудиоклип для урона
    [SerializeField] private Image damageOverlay; // UI-элемент для эффекта покраснения экрана
    [SerializeField] private float overlayFadeSpeed = 2f; // Скорость затухания покраснения
    [SerializeField] private Color overlayColor = new Color(1, 0, 0, 0.5f); // Цвет покраснения (красный с альфа 0.5)

    private float currentHealth; // Текущее здоровье
    private float overlayAlpha = 0f; // Текущая альфа-версия покраснения

    void Start()
    {
        currentHealth = maxHealth;

        // Запускаем пассивное восстановление здоровья
        InvokeRepeating("PassiveHeal", healInterval, healInterval);

        // Убедимся, что начальная прозрачность покраснения равна 0
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, 0f);
        }
    }

    void Update()
    {
        // Плавное затухание покраснения экрана
        if (damageOverlay != null && overlayAlpha > 0f)
        {
            overlayAlpha -= Time.deltaTime * overlayFadeSpeed;
            overlayAlpha = Mathf.Clamp(overlayAlpha, 0f, overlayColor.a); // Ограничение альфа-канала
            damageOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, overlayAlpha);
        }
    }

    // Метод для получения урона
    public void GetDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player took damage. Current health: " + currentHealth);

        // Воспроизведение звука урона
        PlayDamageSound();

        // Включение покраснения экрана
        ShowDamageOverlay();

        // Проверка на смерть
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Воспроизведение звука при получении урона
    private void PlayDamageSound()
    {
        if (damageAudioSource != null && damageSound != null)
        {
            damageAudioSource.PlayOneShot(damageSound);
        }
    }

    // Показ покраснения экрана
    private void ShowDamageOverlay()
    {
        if (damageOverlay != null)
        {
            overlayAlpha = overlayColor.a; // Устанавливаем начальное значение альфа-канала
            damageOverlay.color = new Color(overlayColor.r, overlayColor.g, overlayColor.b, overlayAlpha);
        }
    }

    // Пассивное восстановление здоровья
    private void PassiveHeal()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += passiveHealRate;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            Debug.Log("Player is healing. Current health: " + currentHealth);
        }
    }

    // Метод для обработки смерти игрока
    private void Die()
    {
        Debug.Log("Player died.");
        // Логика для смерти игрока
        SceneManager.LoadScene("Death Scene");
    }

    public static PlayerHealth instance; // Ссылка на синглтон
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
