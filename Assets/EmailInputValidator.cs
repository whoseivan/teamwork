using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class EmailInputValidator : MonoBehaviour
{
    public TMP_InputField emailInputField; // ���� ����� ����������� �����
    public Button registerButton; // ������ �����������
    public float shakeDuration = 0.5f; // ������������ ������
    public float shakeMagnitude = 0.1f; // ������������� ������
    private bool isEmailValid = false; // ���� �������� �����

    void Start()
    {
        emailInputField.onEndEdit.AddListener(ValidateEmail);
        UpdateRegisterButton(); // ��������� ������ ������ ����������� ��� ������
    }

    private void ValidateEmail(string input)
    {
        // ���������� ��������� ��� �������� ������� ����������� �����
        string emailPattern = @"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$";
        if (System.Text.RegularExpressions.Regex.IsMatch(input, emailPattern))
        {
            // ���� ����� ����������, ������������� ����
            isEmailValid = true;
        }
        else
        {
            // ���� ������ ������������, �������� ������ ������ � ���� � false
            StartCoroutine(ShakeText());
            isEmailValid = false;
        }

        UpdateRegisterButton(); // ��������� ������ ������ ����� ���������
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

        emailInputField.textComponent.transform.localPosition = originalPosition; // ������� � �������� ���������
    }

    // ����� ���������� ��������� ������ �����������
    private void UpdateRegisterButton()
    {
        // ������ ����� ������� ������ ���� email ������ ���������
        registerButton.interactable = isEmailValid;
    }
}
