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
        // ��������� ���������� ��������� ������
        registerButton.image.sprite = defaultButtonImage;
        registerButton.interactable = false;

        // ������������� �� ������� ��������� ������ ��� ����� �����
        loginInput.onValueChanged.AddListener(delegate { ValidateInputs(); });
        passwordInput.onValueChanged.AddListener(delegate { ValidateInputs(); });
    }

    private void ValidateInputs()
    {
        // ���������, ��� ��� ���� �� ������
        bool areInputsFilled = !string.IsNullOrEmpty(loginInput.text) && !string.IsNullOrEmpty(passwordInput.text);

        // ������������� ����������� ������ � � �����������
        registerButton.interactable = areInputsFilled;
        registerButton.image.sprite = areInputsFilled ? activeButtonImage : defaultButtonImage;
    }
}
