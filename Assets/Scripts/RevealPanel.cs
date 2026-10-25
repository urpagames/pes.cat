using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class RevealPanel : MonoBehaviour
{
    [Header("Elements")]
    public GameObject contingut;
    public Image fons;
    public RectTransform halo;
    public Image peix;
    public TextMosaic textNom;
    public TextMosaic textDetall;
    public TextMosaic textEstat;
    public TextMosaic textPunts;
    public TextMosaic textContinuar;

    [Header("Aspecte")]
    public float velocitatHalo = 25f;
    public float opacitatFons = 0.6f;
    public Color colorPunts = new Color(1f, 0.85f, 0.3f);
    public Color colorRetirada = new Color(1f, 0.6f, 0.3f);
    public Color colorAlliberada = new Color(0.5f, 0.9f, 1f);

    public bool Ocupat { get; private set; }
    public bool Visible { get { return contingut.activeSelf; } }

    bool saltar;
    bool repos;
    Vector2 posPeix;

    void Awake()
    {
        posPeix = peix.rectTransform.anchoredPosition;
    }

    // ---------- Utilitats ----------

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

    static string Decimal(float valor, string format)
    {
        return valor.ToString(format, CultureInfo.InvariantCulture).Replace('.', ',');
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

    void AmagaTextos()
    {
        textNom.gameObject.SetActive(false);
        textDetall.gameObject.SetActive(false);
        textEstat.gameObject.SetActive(false);
        textPunts.gameObject.SetActive(false);
        textContinuar.gameObject.SetActive(false);
    }

    // ---------- API pública ----------

    public void Mostra(EspecieData especie, float mida, float pes, int punts)
    {
        StopAllCoroutines();
        contingut.SetActive(true);
        StartCoroutine(Sequencia(especie, mida, pes, punts));
    }

    public void Salta()
    {
        saltar = true;
    }

    public void Amaga()
    {
        StopAllCoroutines();
        StartCoroutine(Surt());
    }

    // ---------- Seqüències ----------

    IEnumerator Sequencia(EspecieData especie, float mida, float pes, int punts)
    {
        Ocupat = true;
        saltar = false;
        repos = false;

        // Preparació
        Sprite s = especie.imatge;
        float ample = Mathf.Clamp(s.rect.width * 1.3f, 500f, 900f);
        float alt = ample * s.rect.height / s.rect.width;

        peix.sprite = s;
        peix.rectTransform.sizeDelta = new Vector2(ample, alt);
        peix.rectTransform.anchoredPosition = posPeix;
        peix.rectTransform.localScale = Vector3.zero;
        peix.rectTransform.localRotation = Quaternion.identity;
        halo.localScale = Vector3.zero;
        PosaAlfa(fons, 0f);
        AmagaTextos();

        // 1. Fons i halo
        yield return Anima(0.2f, t => PosaAlfa(fons, opacitatFons * t));
        yield return Anima(0.35f, t => halo.localScale = Vector3.one * EaseOutBack(t));

        // 2. El peix apareix
        yield return Anima(0.45f, t =>
        {
            peix.rectTransform.localScale = Vector3.one * EaseOutBack(t);
            peix.rectTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(-14f, 0f, t));
        });
        repos = true;

        // 3. Nom, mida i destí
        textNom.gameObject.SetActive(true);
        textNom.Posa(especie.nom.ToUpperInvariant());
        textNom.Cop(0.35f);
        yield return new WaitForSecondsRealtime(0.15f);

        textDetall.gameObject.SetActive(true);
        textDetall.Posa(Decimal(mida, "F1") + " CM   " + Decimal(pes, "F2") + " KG");
        textDetall.Cop(0.3f);
        yield return new WaitForSecondsRealtime(0.15f);

        bool autoctona = especie.origen == Origen.Autoctona;
        textEstat.gameObject.SetActive(true);
        textEstat.color = autoctona ? colorAlliberada : colorRetirada;
        textEstat.Posa(autoctona ? "ALLIBERADA" : "RETIRADA");
        textEstat.Cop(0.35f);
        yield return new WaitForSecondsRealtime(0.25f);

        // 4. Els punts compten
        textPunts.gameObject.SetActive(true);
        textPunts.color = colorPunts;
        textPunts.ContaFins(punts, 1.2f);
        while (textPunts.Comptant && !saltar)
        {
            yield return null;
        }
        if (saltar) { textPunts.Salta(punts); }

        textPunts.Cop(0.5f);
        StartCoroutine(PolsHalo());

        textContinuar.gameObject.SetActive(true);
        textContinuar.Posa("TOCA PER CONTINUAR");
        Ocupat = false;
    }

    IEnumerator PolsHalo()
    {
        yield return Anima(0.25f, t => halo.localScale = Vector3.one * (1f + 0.15f * Mathf.Sin(t * Mathf.PI)));
    }

    IEnumerator Surt()
    {
        repos = false;
        Ocupat = false;
        AmagaTextos();

        Vector2 inici = peix.rectTransform.anchoredPosition;
        float escalaHalo = halo.localScale.x;

        yield return Anima(0.45f, t =>
        {
            float e = t * t;
            peix.rectTransform.anchoredPosition = inici + new Vector2(0f, e * 2200f);
            peix.rectTransform.localRotation = Quaternion.Euler(0f, 0f, e * 25f);
            PosaAlfa(fons, opacitatFons * (1f - t));
            halo.localScale = Vector3.one * escalaHalo * (1f - e);
        });

        contingut.SetActive(false);
    }

    // ---------- Moviment continu ----------

    void Update()
    {
        if (!contingut.activeSelf) { return; }

        halo.Rotate(0f, 0f, -velocitatHalo * Time.unscaledDeltaTime);

        if (repos)
        {
            float t = Time.unscaledTime;
            peix.rectTransform.anchoredPosition = posPeix + new Vector2(0f, Mathf.Sin(t * 2f) * 12f);
            peix.rectTransform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Sin(t * 1.3f) * 2.5f);
        }
    }
}
