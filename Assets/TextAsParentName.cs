using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class DisplayParentName : MonoBehaviour
{
    // Ссылка на компонент TextMeshPro
    private TextMeshProUGUI tmpText;

    void Awake()
    {
        // Получаем компонент TextMeshProUGUI на текущем объекте
        tmpText = GetComponent<TextMeshProUGUI>();

        // Если компонент найден и объект имеет родителя
        if (tmpText != null && transform.parent != null)
        {
            // Устанавливаем текст равным имени родительского объекта
            tmpText.text = transform.parent.name;
        }
    }

    // Этот метод вызывается при изменении значения в инспекторе или при модификации компонента в режиме редактирования
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
