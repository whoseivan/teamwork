using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHint : MonoBehaviour
{
    public GameObject objectToShow;
    private bool isShowed = false;

    public void ButtonAction()
    {
        isShowed = !isShowed;

        if (isShowed)
            objectToShow.SetActive(true);
        else
            objectToShow.SetActive(false);
    }
}
