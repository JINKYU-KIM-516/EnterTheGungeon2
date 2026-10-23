using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlinder : MonoBehaviour
{
    [SerializeField] protected GameObject BlinderDisplay;
    [SerializeField] protected Image BlinderImage;
    private Color OriginalColor;

    [SerializeField] protected float alpha = 0f;

    private void Awake()
    {
        BlinderDisplay = this.transform.GetChild(0).gameObject;
        BlinderImage = BlinderDisplay.GetComponent<Image>();
        OriginalColor = BlinderImage.color;
    }

    private void Update()
    {
        alpha = BlinderImage.color.a;
    }

    public void StartBlind()
    {
        StartCoroutine(BlindScreen());
    }

    public void StartShow()
    {
        StartCoroutine(ShowScreen());
    }
    
    public void SetBlind()
    {
        Color color = BlinderImage.color;
        color.a = 1f;
        BlinderImage.color = color;
    }

    public void SetShow()
    {
        Color color = BlinderImage.color;
        color.a = 0f;
        BlinderImage.color = color;
    }

    IEnumerator BlindScreen()
    {
        bool Isblinded = false;
        while(!Isblinded)
        {
            Color color = BlinderImage.color;
            color.a += 0.1f;
            BlinderImage.color = color;
            if (color.a >= 1 )
            {
                Isblinded = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
        SetBlind();
    }

    IEnumerator ShowScreen()
    {
        bool Isblinded = true;
        while (Isblinded)
        {
            Color color = BlinderImage.color;
            color.a -= 0.1f;
            BlinderImage.color = color;
            if (color.a <= 0)
            {
                Isblinded = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
        SetShow();
    }
}
