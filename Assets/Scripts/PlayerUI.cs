using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text healthText;
    public Text levelText;
    public Text xpText;

    private PlayerProgression playerProgression;
    private Combat playerCombat;

    void Start()
    {
        playerProgression = GetComponent<PlayerProgression>();
        playerCombat = GetComponent<Combat>();
    }

    void Update()
    {
        healthText.text = "Здоровье: " + playerCombat.currentHealth;
        levelText.text = "Уровень: " + playerProgression.level;
        xpText.text = "Опыт: " + playerProgression.currentXP + "/" + playerProgression.xpToNextLevel;
    }
}
