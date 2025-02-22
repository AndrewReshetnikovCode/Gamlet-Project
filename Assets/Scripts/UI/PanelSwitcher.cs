using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class PanelButtonPair
    {
        public Button button; // ������, ��������� � �������
        public GameObject panel; // ������, ������� ������ ����� ���������
    }

    public PanelButtonPair[] panelsAndButtons; // ������ ������ � ��������������� �������

    private GameObject currentActivePanel; // ������� �������� ������

    void Start()
    {
        // ��������� ����������� ������� �������
        foreach (var pair in panelsAndButtons)
        {
            pair.button.onClick.AddListener(() => SwitchPanel(pair.panel));
        }

        // ���������� ������� �������� ������
        foreach (var pair in panelsAndButtons)
        {
            if (pair.panel.activeSelf)
            {
                currentActivePanel = pair.panel;
                break;
            }
        }

        // ���� �������� ������ �� �������, �������� ��� ������
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
        // ��������� ������� �������� ������, ���� ��� ����
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
        }

        // ���������� ����� ������
        newPanel.SetActive(true);
        currentActivePanel = newPanel;
    }
}




