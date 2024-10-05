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

    public PanelGroup panelGroup; // ����� � PanelGroup ��� ���������� ��������

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

            // ��������� ����� ������
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

        // ��������� ����� ������
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.color = selectedTextColor;
        }

        // �������� ������� ������ ��� ����������� ������ ������
        if (panelGroup != null)
        {
            int buttonIndex = button.transform.GetSiblingIndex(); // �������� ������ ������
            panelGroup.SetPageIndex(buttonIndex); // ������������� ������ ������
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            
            button.GetComponent<Image>().color = idleColor;
            
            // ��������� ����� ������
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.color = idleTextColor;
            }

        }
    }
}
