using Assets.StatSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text descriptionText; // UI текст для отображения описания
    [SerializeField] private GameObject descriptionPanel; // Панель описания
    [SerializeField] private RectTransform descriptionContainer; // Контейнер для динамической подстройки
    [SerializeField] private float lineHeight = 20f; // Высота строки в пикселях
    [SerializeField] private Vector2 offset = new Vector2(10f, 10f); // Смещение от курсора
    [SerializeField] private RectTransform canvas;

    private void Awake()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }

    public void ShowDescription(string description)
    {
        if (description == null || description == string.Empty)
        {
            return;
        }
        if (descriptionPanel != null && descriptionText != null && descriptionContainer != null && canvas != null)
        {
            descriptionText.text = description;

            // Изменение размера контейнера на основе количества строк
            int lineCount = descriptionText.text.Split('\n').Length;
            Vector2 sizeDelta = descriptionContainer.sizeDelta;
            sizeDelta.y = descriptionText.preferredHeight;//lineCount * lineHeight;
            descriptionContainer.sizeDelta = sizeDelta;
            
            // Позиционирование панели относительно курсора
            Vector2 mousePosition = Input.mousePosition;     
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas, mousePosition, Camera.main, out Vector2 localPoint);

            Vector2 panelPosition = mousePosition + offset;
            panelPosition.y -= sizeDelta.y;
            // Проверка выхода за границы экрана
            RectTransform canvasRect = canvas.transform as RectTransform;
            Vector2 canvasSize = canvasRect.sizeDelta;

            //if (panelPosition.x + descriptionContainer.sizeDelta.x > canvasSize.x / 2)
            //{
            //    panelPosition.x = canvasSize.x / 2 - descriptionContainer.sizeDelta.x;
            //}
            //else if (panelPosition.x < -canvasSize.x / 2)
            //{
            //    panelPosition.x = -canvasSize.x / 2;
            //}

            //if (panelPosition.y - descriptionContainer.sizeDelta.y < -canvasSize.y / 2)
            //{
            //    panelPosition.y = -canvasSize.y / 2 + descriptionContainer.sizeDelta.y;
            //}

            descriptionContainer.anchoredPosition = panelPosition;
            descriptionPanel.SetActive(true);
        }
    }

    public void HideDescription()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }
}