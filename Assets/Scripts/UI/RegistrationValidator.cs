using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class RegistrationValidator : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField loginInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button registerButton;

    [Header("Error Messages")]
    public TMP_Text loginErrorText;
    public TMP_Text emailErrorText;
    public TMP_Text passwordErrorText;

    [Header("Button Images")]
    public Sprite invalidInputSprite;
    public Sprite validInputSprite;

    [Header("Colors")]
    public Color validColor = Color.white;
    public Color invalidColor = Color.red;

    private bool isLoginValid;
    private bool isEmailValid;
    private bool isPasswordValid;

    private void Start()
    {
        // ????????? ????????? ??????
        registerButton.interactable = false;
        registerButton.image.sprite = invalidInputSprite;

        // ????????? ????????? ??? ???????? ??????? ???? ?? ?????????? ??????????????
        loginInput.onEndEdit.AddListener(delegate { ValidateLoginInput(); });
        emailInput.onEndEdit.AddListener(delegate { ValidateEmailInput(); });
        passwordInput.onEndEdit.AddListener(delegate { ValidatePasswordInput(); });

        // ???????? ????????? ?? ???????
        ClearErrorMessages();
    }

    private void ValidateLoginInput()
    {
        isLoginValid = ValidateLogin(loginInput.text);
        UpdateInputField(loginInput, isLoginValid, loginErrorText, "????? ?????? ????????? ?? 6 ?? 30 ????????");
        UpdateRegisterButtonState();
    }

    private void ValidateEmailInput()
    {
        isEmailValid = ValidateEmail(emailInput.text);
        UpdateInputField(emailInput, isEmailValid, emailErrorText, "????? ??????? ???????????");
        UpdateRegisterButtonState();
    }

    private void ValidatePasswordInput()
    {
        isPasswordValid = ValidatePassword(passwordInput.text);
        UpdateInputField(passwordInput, isPasswordValid, passwordErrorText, "?????? ?????? ????????? ?? 6 ?? 30 ????????, ????????? ?????, ????? ? ??????");
        UpdateRegisterButtonState();
    }

    private void UpdateInputField(TMP_InputField inputField, bool isValid, TMP_Text errorText, string errorMessage)
    {
        inputField.image.color = isValid ? validColor : invalidColor;
        errorText.text = errorMessage;
    }

    private void UpdateRegisterButtonState()
    {
        // ?????? ??????? ?????? ???? ??? ???? ???????
        bool areAllValid = isLoginValid && isEmailValid && isPasswordValid;
        registerButton.interactable = areAllValid;
        registerButton.image.sprite = areAllValid ? validInputSprite : invalidInputSprite;
    }

    private bool ValidateLogin(string login)
    {
        return login.Length >= 6 && login.Length <= 30;
    }

    private bool ValidateEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    private bool ValidatePassword(string password)
    {
        if (password.Length < 6 || password.Length > 30)
            return false;

        bool hasUppercase = Regex.IsMatch(password, @"[A-Z]");
        bool hasDigit = Regex.IsMatch(password, @"\d");
        bool hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

        return hasUppercase && hasDigit && hasSpecialChar;
    }

    private void ClearErrorMessages()
    {
        loginErrorText.text = "";
        emailErrorText.text = "";
        passwordErrorText.text = "";
    }
}
