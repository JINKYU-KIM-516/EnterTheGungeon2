using System.Collections;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] protected GameObject BlinderCanvas;
    [SerializeField] protected GameObject StartButtonCanvas;
    private ScreenBlinder m_ScreenBlinder;

    private void Awake()
    {
        m_ScreenBlinder = BlinderCanvas.GetComponent<ScreenBlinder>();
        StartButtonCanvas.SetActive(false);
    }
    void Start()
    {
        StartCoroutine(ShowButton());   
    }

    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(1f);
        m_ScreenBlinder.StartShow();
        yield return new WaitForSeconds(1f);
        BlinderCanvas.SetActive(false);
        StartButtonCanvas.SetActive(true);
    }
}
