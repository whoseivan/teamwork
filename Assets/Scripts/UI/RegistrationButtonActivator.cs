using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegistrationButtonActivator : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;
    public Button registerButton;

    [Header("Button Images")]
    public Sprite defaultButtonImage;
    public Sprite activeButtonImage;

    private void Start()
    {
        // Установка начального состояния кнопки
        registerButton.image.sprite = defaultButtonImage;
        registerButton.interactable = false;

        // Подписываемся на события изменения текста для полей ввода
        loginInput.onValueChanged.AddListener(delegate { ValidateInputs(); });
        passwordInput.onValueChanged.AddListener(delegate { ValidateInputs(); });
    }

    private void ValidateInputs()
    {
        // Проверяем, что оба поля не пустые
        bool areInputsFilled = !string.IsNullOrEmpty(loginInput.text) && !string.IsNullOrEmpty(passwordInput.text);

        // Устанавливаем доступность кнопки и её изображение
        registerButton.interactable = areInputsFilled;
        registerButton.image.sprite = areInputsFilled ? activeButtonImage : defaultButtonImage;
    }
}
