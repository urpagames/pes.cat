using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CristDialeg : MonoBehaviour
{
    [Header("Elements")]
    public GameObject contingut;
    public Image fons;
    public RectTransform crist;
    public RectTransform bafarada;
    public TextMosaic text;
    public GameObject grupObjecte;
    public Image imatgeObjecte;
    public TextMosaic textNomObjecte;

    [Header("Aspecte")]
    public float opacitatFons = 0.6f;
    public float desplacamentCrist = 1400f;

    public bool Ocupat { get; private set; }

    Vector2 posCrist;
    Vector3 escalaBafarada;
    Vector3 escalaObjecte;

    void Awake()
    {
        posCrist = crist.anchoredPosition;
        escalaBafarada = bafarada.localScale;
        escalaObjecte = grupObjecte.transform.localScale;
        contingut.SetActive(false);
    }

    static float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    static void PosaAlfa(Image img, float alfa)
    {
        Color c = img.color;
        c.a = alfa;
        img.color = c;
    }

    IEnumerator Anima(float durada, Action<float> pas)
    {
        float temps = 0f;
        while (temps < durada)
        {
            temps += Time.unscaledDeltaTime;
            pas(Mathf.Clamp01(temps / durada));
            yield return null;
        }
        pas(1f);
    }

    public void Mostra(string[] frases, Action alAcabar = null, Sprite sprite = null, string nom = null)
    {
        if (Ocupat) { return; }
        StopAllCoroutines();
        StartCoroutine(Sequencia(frases, alAcabar, sprite, nom));
    }

    IEnumerator Sequencia(string[] frases, Action alAcabar, Sprite sprite, string nom)
    {
        Ocupat = true;
        contingut.SetActive(true);

        Vector2 inici = posCrist + new Vector2(0f, -desplacamentCrist);
        crist.anchoredPosition = inici;
        bafarada.localScale = Vector3.zero;
        text.gameObject.SetActive(false);
        grupObjecte.SetActive(false);
        PosaAlfa(fons, 0f);

        AudioManager.RiureCrist();

        yield return Anima(0.2f, t => PosaAlfa(fons, opacitatFons * t));
        yield return Anima(0.45f, t => crist.anchoredPosition = Vector2.Lerp(inici, posCrist, 1f - (1f - t) * (1f - t)));
        yield return Anima(0.3f, t => bafarada.localScale = escalaBafarada * EaseOutBack(t));

        if (sprite != null)
        {
            grupObjecte.SetActive(true);
            imatgeObjecte.sprite = sprite;
            imatgeObjecte.SetNativeSize();
            grupObjecte.transform.localScale = Vector3.zero;
            textNomObjecte.gameObject.SetActive(true);
            textNomObjecte.Posa((nom != null ? nom : "").ToUpperInvariant());
            yield return Anima(0.4f, t => grupObjecte.transform.localScale = escalaObjecte * EaseOutBack(t));
        }

        for (int i = 0; i < frases.Length; i++)
        {
            text.gameObject.SetActive(true);
            text.Posa(frases[i].ToUpperInvariant());
            text.Cop(0.25f);
            yield return null;
            while (!Input.GetMouseButtonDown(0)) { yield return null; }
        }

        Vector2 actual = crist.anchoredPosition;
        Vector3 escalaB = bafarada.localScale;
        text.gameObject.SetActive(false);
        yield return Anima(0.35f, t =>
        {
            float e = t * t;
            crist.anchoredPosition = Vector2.Lerp(actual, inici, e);
            bafarada.localScale = escalaB * (1f - e);
            PosaAlfa(fons, opacitatFons * (1f - t));
            grupObjecte.transform.localScale = escalaObjecte * (1f - e);
        });

        contingut.SetActive(false);
        Ocupat = false;
        if (alAcabar != null) { alAcabar(); }
    }
}