using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class DisplayParentName : MonoBehaviour
{
    // ������ �� ��������� TextMeshPro
    private TextMeshProUGUI tmpText;

    void Awake()
    {
        // �������� ��������� TextMeshProUGUI �� ������� �������
        tmpText = GetComponent<TextMeshProUGUI>();

        // ���� ��������� ������ � ������ ����� ��������
        if (tmpText != null && transform.parent != null)
        {
            // ������������� ����� ������ ����� ������������� �������
            tmpText.text = transform.parent.name;
        }
    }

    // ���� ����� ���������� ��� ��������� �������� � ���������� ��� ��� ����������� ���������� � ������ ��������������
    void OnValidate()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        if (tmpText != null && transform.parent != null)
        {
            tmpText.text = transform.parent.name;
        }
    }
}
