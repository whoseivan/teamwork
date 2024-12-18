using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.UI; // ??? ?????? ? ???????? ? ?? ?????????

public class UserInputValidator : MonoBehaviour
{
    [Header("UI elements")]
    public TMP_InputField emailInputField; // ???? ????? ??????????? ?????
    public TMP_InputField usernameInputField; // ???? ????? ??????
    public TMP_InputField passwordInputField; // ???? ????? ??????
    public Button registerButton; // ?????? ???????????

    [Header("Only for registration")]
    public bool registration;
    public Sprite validSprite; // ?????? ??? ????????? ?????????
    public Sprite invalidSprite; // ?????? ??? ??????????? ?????????
    public float shakeDuration = 0.5f; // ???????????? ??????
    public float shakeMagnitude = 0.1f; // ????????????? ??????

    [Header("Input fields in usage")]
    public bool usingEmail;
    public bool usingUsername;
    public bool usingPassword;
    

    private bool isEmailValid = false;
    private bool isUsernameValid = false;
    private bool isPasswordValid = false;

    void Start()
    {
        // ????????????? ?? ????????? ?????
        if (usingEmail)
            emailInputField.onEndEdit.AddListener(delegate { ValidateEmail(); });
        if (usingUsername)
            usernameInputField.onEndEdit.AddListener(delegate { ValidateUsername(); });
        if (usingPassword)
            passwordInputField.onEndEdit.AddListener(delegate { ValidatePassword(); });

        // ????????? ????????? ?????? ??? ??????
        if (registration)
            UpdateRegisterButton();
    }

    // ????????? email
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
        if (registration)
            UpdateRegisterButton();
        return isEmailValid;
    }

    // ????????? ??????
    public bool ValidateUsername()
    {
        string username = usernameInputField.text;
        string usernamePattern = @"^[a-zA-Z0-9]{5,}$"; // ??????? 5 ????????, ?????? ????? ? ?????
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
        if (registration)
            UpdateRegisterButton();
        return isUsernameValid;
    }

    // ????????? ??????
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
        if (registration)
            UpdateRegisterButton();
        return isPasswordValid;
    }

    // ????? ????? ??? ?????? ?????? ? ????? ?????
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

        inputField.textComponent.transform.localPosition = originalPosition; // ?????????? ?? ???????? ?????
    }

    // ????? ??? ?????????? ?????? ??????????? ? ??????????? ?? ?????????? ?????
    private void UpdateRegisterButton()
    {
        if (isEmailValid && isUsernameValid && isPasswordValid)
        {
            registerButton.interactable = true;
            registerButton.image.sprite = validSprite; // ?????? ?????? ?????? ?? ????????
        }
        else
        {
            registerButton.interactable = false;
            registerButton.image.sprite = invalidSprite; // ?????? ?????? ?????? ?? ??????????
        }
    }
}
