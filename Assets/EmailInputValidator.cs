using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class EmailInputValidator : MonoBehaviour
{
    public TMP_InputField emailInputField; // ���� ����� ����������� �����
    public float shakeDuration = 0.5f; // ������������ ������
    public float shakeMagnitude = 0.1f; // ������������� ������

    void Start()
    {
        emailInputField.onEndEdit.AddListener(ValidateEmail);
    }

    private void ValidateEmail(string input)
    {
        // ���������� ��������� ��� �������� ������� ����������� �����
        string emailPattern = @"^[a-zA-Z0-9._]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(input, emailPattern))
        {
            // ���� ������ ������������, �������� ������ ������
            StartCoroutine(ShakeText());
        }
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
}
