using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class PanelButtonPair
    {
        public Button button; // Кнопка, связанная с панелью
        public GameObject panel; // Панель, которую кнопка будет открывать
    }

    public PanelButtonPair[] panelsAndButtons; // Массив кнопок и соответствующих панелей

    private GameObject currentActivePanel; // Текущая активная панель

    void Start()
    {
        // Назначаем обработчики событий кнопкам
        foreach (var pair in panelsAndButtons)
        {
            pair.button.onClick.AddListener(() => SwitchPanel(pair.panel));
        }

        // Определяем текущую активную панель
        foreach (var pair in panelsAndButtons)
        {
            if (pair.panel.activeSelf)
            {
                currentActivePanel = pair.panel;
                break;
            }
        }

        // Если активная панель не найдена, скрываем все панели
        if (currentActivePanel == null)
        {
            foreach (var pair in panelsAndButtons)
            {
                pair.panel.SetActive(false);
            }
        }
    }
    public void SwitchPanel(GameObject newPanel)
    {
        // Отключаем текущую активную панель, если она есть
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
        }

        // Активируем новую панель
        newPanel.SetActive(true);
        currentActivePanel = newPanel;
    }
}




