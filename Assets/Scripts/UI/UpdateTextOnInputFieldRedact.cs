using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTextOnInputFieldRedact : MonoBehaviour
{
    public TMP_Text errorText;

    public void ClearText() {
        errorText.text = "";
    }
    
}
