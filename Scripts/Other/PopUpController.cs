using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpController : MonoBehaviour
{
    public Animator popUpAnime;

    public void PlayPopUpAnime(string amount)
    {
        popUpAnime.GetComponent<Text>().text = amount.ToString();
        popUpAnime.SetTrigger("damage");
    }

    private void LateUpdate()
    {
        if(popUpAnime != null)
        {
            popUpAnime.transform.parent.LookAt(Camera.main.transform);
            popUpAnime.transform.parent.Rotate(0, 180, 0);
        }
    }

}