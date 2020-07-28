using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpScript : MonoBehaviour
{
    public Animator anime;
    private Text damageText;

    void Start()
    {
        AnimatorClipInfo[] clipInfo = anime.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);

        damageText = anime.GetComponent<Text>();
    }


    public void SetText(string text)
    {
        damageText.text = text;
    }
}
