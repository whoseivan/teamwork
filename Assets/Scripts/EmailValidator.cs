using UnityEngine;
using TMPro;  // Необходим для работы с TextMeshPro
using System.Text.RegularExpressions;

public class EmailValidator : MonoBehaviour
{
    public TMP_InputField emailInputField;  // Поле ввода электронной почты
    public TMP_Text resultText;  // Поле для вывода результата

    // Регулярное выражение для проверки формата электронной почты
    private string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    void Start()
    {
        // Привязываем делегат к событию изменения текста в InputField
        emailInputField.onValueChanged.AddListener(delegate { ValidateEmail(); });
    }

    // Метод для проверки электронной почты
    private void ValidateEmail()
    {
        string email = emailInputField.text;

        // Проверяем формат электронной почты с помощью регулярного выражения
        if (Regex.IsMatch(email, emailPattern))
        {
            // Успешная валидация
            resultText.text = "Валидный формат почты!";
            resultText.color = Color.green;  // Цвет текста при успехе
            emailInputField.image.color = Color.white;  // Вернуть цвет поля в норму
        }
        else
        {
            // Неверный формат
            resultText.text = "Неверный формат почты!";
            resultText.color = Color.red;  // Цвет текста при ошибке
            emailInputField.image.color = Color.red;  // Подсветка поля красным
        }
    }
}
