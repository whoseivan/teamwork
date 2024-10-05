using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;

    [Header("Button Colors")]
    public Color idleColor;
    public Color enteredColor;
    public Color selectedColor;
    public Color idleTextColor;
    public Color selectedTextColor;

    [Space(10)]
    public TabButton selectedTab;

    public PanelGroup panelGroup; // Связь с PanelGroup для управления панелями

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
        ResetTabs();
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.GetComponent<Image>().color = enteredColor;

            // Изменение цвета текста
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.color = enteredColor;
            }
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.GetComponent<Image>().color = selectedColor;

        // Изменение цвета текста
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.color = selectedTextColor;
        }

        // Передача индекса кнопки для отображения нужной панели
        if (panelGroup != null)
        {
            int buttonIndex = button.transform.GetSiblingIndex(); // Получаем индекс кнопки
            panelGroup.SetPageIndex(buttonIndex); // Устанавливаем индекс панели
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            
            button.GetComponent<Image>().color = idleColor;
            
            // Изменение цвета текста
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.color = idleTextColor;
            }

        }
    }
}
