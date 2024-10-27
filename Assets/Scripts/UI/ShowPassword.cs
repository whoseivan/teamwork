using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowPassword : MonoBehaviour
{
    public TMP_InputField inputField;

    private bool isShowed = false;

    public void showPassword()
    {
        isShowed = !isShowed;

        if (!isShowed)
            inputField.contentType = TMP_InputField.ContentType.Password;
        if (isShowed)
            inputField.contentType = TMP_InputField.ContentType.Standard;

        inputField.ForceLabelUpdate();
    }
}
