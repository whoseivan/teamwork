using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.UI; // Для работы с кнопками и их спрайтами

public class UserInputValidator : MonoBehaviour
{
    public TMP_InputField emailInputField; // Поле ввода электронной почты
    public TMP_InputField usernameInputField; // Поле ввода логина
    public TMP_InputField passwordInputField; // Поле ввода пароля
    public Button registerButton; // Кнопка регистрации
    public Sprite validSprite; // Спрайт для валидного состояния
    public Sprite invalidSprite; // Спрайт для невалидного состояния
    public float shakeDuration = 0.5f; // Длительность тряски
    public float shakeMagnitude = 0.1f; // Интенсивность тряски

    private bool isEmailValid = false;
    private bool isUsernameValid = false;
    private bool isPasswordValid = false;

    void Start()
    {
        // Подписываемся на изменение полей
        emailInputField.onEndEdit.AddListener(delegate { ValidateEmail(); });
        usernameInputField.onEndEdit.AddListener(delegate { ValidateUsername(); });
        passwordInputField.onEndEdit.AddListener(delegate { ValidatePassword(); });

        // Обновляем состояние кнопки при старте
        UpdateRegisterButton();
    }

    // Валидация email
    public bool ValidateEmail()
    {
        string email = emailInputField.text;
        string emailPattern = @"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$";
        if (Regex.IsMatch(email, emailPattern))
        {
            isEmailValid = true;
        }
        else
        {
            StartCoroutine(ShakeText(emailInputField));
            isEmailValid = false;
            Debug.Log("Email not valid");
        }
        UpdateRegisterButton();
        return isEmailValid;
    }

    // Валидация логина
    public bool ValidateUsername()
    {
        string username = usernameInputField.text;
        string usernamePattern = @"^[a-zA-Z0-9]{5,}$"; // Минимум 5 символов, только цифры и буквы
        if (Regex.IsMatch(username, usernamePattern))
        {
            isUsernameValid = true;
        }
        else
        {
            StartCoroutine(ShakeText(usernameInputField));
            isUsernameValid = false;
            Debug.Log("Username not valid");
        }
        UpdateRegisterButton();
        return isUsernameValid;
    }

    // Валидация пароля
    public bool ValidatePassword()
    {
        string password = passwordInputField.text;
        string passwordPattern = @"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,}$";

        if (Regex.IsMatch(password, passwordPattern))
        {
            isPasswordValid = true;
        }
        else
        {
            StartCoroutine(ShakeText(passwordInputField));
            isPasswordValid = false;
            Debug.Log("Password not valid");
        }
        UpdateRegisterButton();
        return isPasswordValid;
    }

    // Общий метод для тряски текста в полях ввода
    private IEnumerator ShakeText(TMP_InputField inputField)
    {
        Vector3 originalPosition = inputField.textComponent.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            inputField.textComponent.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        inputField.textComponent.transform.localPosition = originalPosition; // Возвращаем на исходное место
    }

    // Метод для обновления кнопки регистрации в зависимости от валидности полей
    private void UpdateRegisterButton()
    {
        if (isEmailValid && isUsernameValid && isPasswordValid)
        {
            registerButton.interactable = true;
            registerButton.image.sprite = validSprite; // Меняем спрайт кнопки на валидный
        }
        else
        {
            registerButton.interactable = false;
            registerButton.image.sprite = invalidSprite; // Меняем спрайт кнопки на невалидный
        }
    }
}
