using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class EmailInputValidator : MonoBehaviour
{
    public TMP_InputField emailInputField; // Поле ввода электронной почты
    public Button registerButton; // Кнопка регистрации
    public float shakeDuration = 0.5f; // Длительность тряски
    public float shakeMagnitude = 0.1f; // Интенсивность тряски
    private bool isEmailValid = false; // Флаг проверки почты

    void Start()
    {
        emailInputField.onEndEdit.AddListener(ValidateEmail);
        UpdateRegisterButton(); // Обновляем статус кнопки регистрации при старте
    }

    private void ValidateEmail(string input)
    {
        // Регулярное выражение для проверки формата электронной почты
        string emailPattern = @"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$";
        if (System.Text.RegularExpressions.Regex.IsMatch(input, emailPattern))
        {
            // Если почта правильная, устанавливаем флаг
            isEmailValid = true;
        }
        else
        {
            // Если формат неправильный, вызываем эффект тряски и флаг в false
            StartCoroutine(ShakeText());
            isEmailValid = false;
        }

        UpdateRegisterButton(); // Обновляем статус кнопки после валидации
    }

    private IEnumerator ShakeText()
    {
        Vector3 originalPosition = emailInputField.textComponent.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            emailInputField.textComponent.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        emailInputField.textComponent.transform.localPosition = originalPosition; // Вернуть в исходное положение
    }

    // Метод обновления состояния кнопки регистрации
    private void UpdateRegisterButton()
    {
        // Кнопка будет активна только если email введен правильно
        registerButton.interactable = isEmailValid;
    }
}
