using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Menú")]
    public GameObject grupBotons;

    [Header("Mapa i esquers")]
    public GameObject panellEsquers;
    public Image filtre;
    public Image imatgeMapa;
    public RectTransform pin;
    public Button[] botonsEsquer;
    public GameObject[] bloquejos;
    public TextMosaic[] textsBloqueig;
    public float opacitatFiltre = 0.75f;
    public PanellPescadex panellPescadex;

    public GameObject panellAjustaments;
    public Slider sliderEfectes;
    public Slider sliderMusica;

    Vector3[] escalaEsquer;
    Vector3 escalaPin;

    public GameObject panellEnciclopedia;
    public PanellQuadern panellQuadern;
    public PanellMuseu panellMuseu;

    void Start()
    {
        escalaEsquer = new Vector3[botonsEsquer.Length];
        for (int i = 0; i < botonsEsquer.Length; i++)
        {
            escalaEsquer[i] = botonsEsquer[i].transform.localScale;
        }
        escalaPin = pin.localScale;
        panellEsquers.SetActive(false);
        panellEnciclopedia.SetActive(false);
        panellAjustaments.SetActive(false);
        sliderEfectes.onValueChanged.AddListener(AudioManager.PosaVolumEfectes);
        sliderMusica.onValueChanged.AddListener(AudioManager.PosaVolumMusica);
    }

    public void Pescar()
    {
        ActualitzaEsquers();
        grupBotons.SetActive(false);
        panellEsquers.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ObreMapa());
    }

    public void TancaEsquers()
    {
        StopAllCoroutines();
        panellEsquers.SetActive(false);
        grupBotons.SetActive(true);
    }

    public void TriaEsquer(int numero)
    {
        if (!Progres.EsquerDesbloquejat(numero)) { return; }
        Partida.esquerTriat = numero;
        SceneManager.LoadScene("Game");
    }

    public void ObreEnciclopedia() { panellEnciclopedia.SetActive(true); }
    public void TancaEnciclopedia() { panellEnciclopedia.SetActive(false); }
    public void ObrePescadex() { panellEnciclopedia.SetActive(false); panellPescadex.Obre(); }
    public void ObreQuadern() { panellEnciclopedia.SetActive(false); panellQuadern.Obre(); }
    public void ObreMuseu() { panellEnciclopedia.SetActive(false); panellMuseu.Obre(); }

    public void TornaAlHub()
    {
        panellPescadex.gameObject.SetActive(false);
        panellQuadern.gameObject.SetActive(false);
        panellMuseu.gameObject.SetActive(false);
        panellEnciclopedia.SetActive(true);
    }

    public void ObreAjustaments()
    {
        sliderEfectes.SetValueWithoutNotify(AudioManager.volumEfectes);
        sliderMusica.SetValueWithoutNotify(AudioManager.volumMusica);
        panellAjustaments.SetActive(true);
    }

    public void TancaAjustaments()
    {
        panellAjustaments.SetActive(false);
    }

    void ActualitzaEsquers()
    {
        for (int i = 0; i < botonsEsquer.Length; i++)
        {
            bool obert = Progres.EsquerDesbloquejat(i + 1);
            botonsEsquer[i].interactable = obert;
            bloquejos[i].SetActive(!obert);

            if (!obert)
            {
                string punts = Progres.llindars[i].ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.');
                textsBloqueig[i].Posa(punts);
            }
        }
    }

    static void PosaAlfa(Image img, float alfa)
    {
        Color c = img.color;
        c.a = alfa;
        img.color = c;
    }

    static float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
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

    IEnumerator ObreMapa()
    {
        PosaAlfa(filtre, 0f);
        PosaAlfa(imatgeMapa, 0f);
        imatgeMapa.rectTransform.localScale = Vector3.one * 0.94f;
        pin.localScale = Vector3.zero;
        for (int i = 0; i < botonsEsquer.Length; i++)
        {
            botonsEsquer[i].transform.localScale = Vector3.zero;
        }

        yield return Anima(0.3f, t =>
        {
            PosaAlfa(filtre, opacitatFiltre * t);
            PosaAlfa(imatgeMapa, t);
            imatgeMapa.rectTransform.localScale = Vector3.one * Mathf.Lerp(0.94f, 1f, t);
        });

        yield return Anima(0.3f, t => pin.localScale = escalaPin * EaseOutBack(t));

        for (int i = 0; i < botonsEsquer.Length; i++)
        {
            int k = i;
            StartCoroutine(Anima(0.35f, t => botonsEsquer[k].transform.localScale = escalaEsquer[k] * EaseOutBack(t)));
            yield return new WaitForSecondsRealtime(0.08f);
        }
    }

    // això ho haig de treure!

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { Progres.Esborra(); Pescadex.Esborra(); ActualitzaEsquers(); Curiositats.Esborra(); NotesPesca.Esborra(); }
        if (Input.GetKeyDown(KeyCode.P)) { Progres.Punts += 5000; ActualitzaEsquers(); }
    }
}