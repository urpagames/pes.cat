using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextMosaic : MonoBehaviour
{
    public GlifsMosaic glifs;
    public string textInicial = "PES 1.250 PTS";

    [Header("Aspecte")]
    public float escala = 0.4f;
    public float espai = 4f;
    public float espaiParaula = 70f;
    public Color color = Color.white;

    [Header("Moviment")]
    public float onda = 6f;
    public float velocitatOnada = 4f;
    public float desfasPerLletra = 0.5f;

    List<Image> imatges = new List<Image>();
    List<float> posX = new List<float>();
    int quantes = 0;
    float cop = 0f;
    Vector3 escalaInicial;

    void Awake()
    {
        escalaInicial = transform.localScale;
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(textInicial)) { Posa(textInicial); }
    }

    Image CreaImatge()
    {
        GameObject go = new GameObject("glif", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(transform, false);

        Image img = go.GetComponent<Image>();
        img.raycastTarget = false;

        RectTransform rt = img.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        return img;
    }

    public void Posa(string text)
    {
        if (glifs == null)
        {
            Debug.LogWarning("TextMosaic: falta assignar els Glifs.");
            return;
        }

        // 1. Recollim els glifs que cal dibuixar
        List<Sprite> sprites = new List<Sprite>();
        List<float> linies = new List<float>();
        List<float> separacions = new List<float>();

        float espaiPendent = 0f;
        foreach (char c in text)
        {
            if (c == ' ')
            {
                espaiPendent += espaiParaula;
                continue;
            }

            float linia;
            Sprite s = glifs.Busca(c, out linia);
            if (s == null)
            {
                Debug.LogWarning("TextMosaic: falta el glif de '" + c + "'");
                continue;
            }

            sprites.Add(s);
            linies.Add(linia);
            separacions.Add(espaiPendent);
            espaiPendent = 0f;
        }

        while (imatges.Count < sprites.Count)
        {
            imatges.Add(CreaImatge());
        }

        float total = 0f;
        for (int i = 0; i < sprites.Count; i++)
        {
            total += separacions[i] + sprites[i].rect.width + espai;
        }
        if (sprites.Count > 0) { total -= espai; }

        posX.Clear();
        float x = -total * escala * 0.5f;
        for (int i = 0; i < imatges.Count; i++)
        {
            Image img = imatges[i];

            if (i >= sprites.Count)
            {
                img.gameObject.SetActive(false);
                continue;
            }

            Sprite s = sprites[i];
            x += separacions[i] * escala;

            img.gameObject.SetActive(true);
            img.sprite = s;
            img.color = color;

            RectTransform rt = img.rectTransform;
            rt.sizeDelta = new Vector2(s.rect.width, s.rect.height) * escala;
            rt.pivot = new Vector2(0f, linies[i] / s.rect.height);

            posX.Add(x);
            x += (s.rect.width + espai) * escala;
        }

        quantes = sprites.Count;
    }

    public void PosaNumero(int n)
    {
        Posa(n.ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.'));
    }

    public void Cop(float forca = 0.3f)
    {
        cop = forca;
    }

    public void ContaFins(int objectiu, float durada = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(Conta(objectiu, durada));
    }

    IEnumerator Conta(int objectiu, float durada)
    {
        float temps = 0f;
        int ultim = -1;

        while (temps < durada)
        {
            temps += Time.unscaledDeltaTime;
            float p = Mathf.SmoothStep(0f, 1f, temps / durada);
            int valor = Mathf.RoundToInt(Mathf.Lerp(0f, objectiu, p));

            if (valor != ultim)
            {
                PosaNumero(valor);
                Cop(0.1f);
                ultim = valor;
            }
            yield return null;
        }

        PosaNumero(objectiu);
        Cop(0.4f);
    }

    [ContextMenu("Prova: comptar fins a 12.500")]
    void Prova()
    {
        ContaFins(12500, 1.5f);
    }

    void Update()
    {
        float t = Time.unscaledTime * velocitatOnada;
        for (int i = 0; i < quantes; i++)
        {
            float y = Mathf.Sin(t + i * desfasPerLletra) * onda;
            imatges[i].rectTransform.anchoredPosition = new Vector2(posX[i], y);
        }

        cop = Mathf.Lerp(cop, 0f, 10f * Time.unscaledDeltaTime);
        transform.localScale = escalaInicial * (1f + cop);
    }
}
