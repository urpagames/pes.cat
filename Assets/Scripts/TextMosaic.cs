using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextMosaic : MonoBehaviour
{
    public GlifsMosaic glifs;
    public string textInicial = "";

    [Header("Aspecte")]
    public float escala = 0.4f;
    public float espai = 4f;
    public float espaiParaula = 70f;
    public Color color = Color.white;
    public string sufix = "";
    public float ampladaMax = 0f;
    public float alcadaLinia = 0f;

    [Header("Moviment")]
    public float onda = 6f;
    public float velocitatOnada = 4f;
    public float desfasPerLletra = 0.5f;

    List<Image> imatges = new List<Image>();
    List<float> posX = new List<float>();
    List<float> posY = new List<float>();
    int quantes = 0;
    float cop = 0f;

    public bool Comptant { get; private set; }
    Vector3 escalaInicial;

    void Awake()
    {
        escalaInicial = transform.localScale;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform fill = transform.GetChild(i);
            if (fill.name == "glif") { Destroy(fill.gameObject); }
        }
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

        List<Sprite> sprites = new List<Sprite>();
        List<float> linies = new List<float>();
        List<float> separacions = new List<float>();
        List<bool> salts = new List<bool>();

        float espaiPendent = 0f;
        bool saltPendent = false;
        foreach (char c in text)
        {
            if (c == '\n')
            {
                saltPendent = true;
                espaiPendent = 0f;
                continue;
            }

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
            salts.Add(saltPendent);
            espaiPendent = 0f;
            saltPendent = false;
        }

        int n = sprites.Count;

        while (imatges.Count < n)
        {
            imatges.Add(CreaImatge());
        }

        // 1. Repartim els glifs en linies, sense partir paraules
        int[] liniaDe = new int[n];
        int liniaActual = 0;
        float ampleLinia = 0f;
        float limit = (ampladaMax > 0f) ? ampladaMax / escala : float.MaxValue;
        float maxAlt = 0f;

        int inici = 0;
        while (inici < n)
        {
            int fi = inici + 1;
            while (fi < n && separacions[fi] <= 0f && !salts[fi]) { fi++; }

            float ampleParaula = 0f;
            for (int k = inici; k < fi; k++)
            {
                ampleParaula += sprites[k].rect.width + espai;
                if (sprites[k].rect.height > maxAlt) { maxAlt = sprites[k].rect.height; }
            }

            bool nouSalt = inici > 0 && salts[inici];
            if (nouSalt || (ampleLinia > 0f && ampleLinia + separacions[inici] + ampleParaula - espai > limit))
            {
                liniaActual++;
                ampleLinia = 0f;
            }

            if (ampleLinia <= 0f) { separacions[inici] = 0f; }
            ampleLinia += separacions[inici] + ampleParaula;

            for (int k = inici; k < fi; k++) { liniaDe[k] = liniaActual; }
            inici = fi;
        }

        int nLinies = (n > 0) ? liniaActual + 1 : 0;
        float[] amples = new float[Mathf.Max(nLinies, 1)];
        for (int i = 0; i < n; i++)
        {
            amples[liniaDe[i]] += separacions[i] + sprites[i].rect.width + espai;
        }
        for (int l = 0; l < nLinies; l++) { amples[l] -= espai; }

        float alt = (alcadaLinia > 0f) ? alcadaLinia : maxAlt * 1.15f;
        float yBloc = (nLinies - 1) * alt * escala * 0.5f;

        // 2. Posicionem cada glif
        posX.Clear();
        posY.Clear();
        int liniaAnterior = -1;
        float x = 0f;

        for (int i = 0; i < imatges.Count; i++)
        {
            Image img = imatges[i];

            if (i >= n)
            {
                img.gameObject.SetActive(false);
                continue;
            }

            Sprite s = sprites[i];
            int l = liniaDe[i];

            if (l != liniaAnterior)
            {
                x = -amples[l] * escala * 0.5f;
                liniaAnterior = l;
            }
            x += separacions[i] * escala;

            img.gameObject.SetActive(true);
            img.sprite = s;
            img.color = color;

            RectTransform rt = img.rectTransform;
            rt.sizeDelta = new Vector2(s.rect.width, s.rect.height) * escala;
            rt.pivot = new Vector2(0f, linies[i] / s.rect.height);

            posX.Add(x);
            posY.Add(yBloc - l * alt * escala);
            x += (s.rect.width + espai) * escala;
        }

        quantes = n;
    }

    public void PosaNumero(int n)
    {
        Posa(n.ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.') + sufix);
    }

    public void Cop(float forca = 0.3f)
    {
        cop = forca;
    }

    public void ContaFins(int objectiu, float durada = 1f, int inici = 0)
    {
        StopAllCoroutines();
        StartCoroutine(Conta(objectiu, durada, inici));
    }

    public void Salta(int valor)
    {
        StopAllCoroutines();
        Comptant = false;
        PosaNumero(valor);
        Cop(0.4f);
    }

    IEnumerator Conta(int objectiu, float durada, int inici)
    {
        Comptant = true;
        float temps = 0f;
        int ultim = -1;

        while (temps < durada)
        {
            temps += Time.unscaledDeltaTime;
            float p = Mathf.SmoothStep(0f, 1f, temps / durada);
            int valor = Mathf.RoundToInt(Mathf.Lerp(inici, objectiu, p));

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
        Comptant = false;
    }

    [ContextMenu("Prova: contar fins a 12.500")]
    void Prova()
    {
        ContaFins(12500, 1.5f);
    }

    [ContextMenu("Prova: text llarg")]
    void ProvaLlarg()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Fes aquesta prova amb el Play en marxa.");
            return;
        }
        if (ampladaMax <= 0f) { ampladaMax = 700f; }
        Posa("AQUEST ES UN TEXT MOLT LLARG PER PROVAR EL SALT DE LINIA AUTOMATIC\nI AQUESTA ES UNA ALTRA LINIA");
    }

    void Update()
    {
        float t = Time.unscaledTime * velocitatOnada;
        for (int i = 0; i < quantes; i++)
        {
            float y = Mathf.Sin(t + i * desfasPerLletra) * onda;
            imatges[i].rectTransform.anchoredPosition = new Vector2(posX[i], posY[i] + y);
        }

        cop = Mathf.Lerp(cop, 0f, 10f * Time.unscaledDeltaTime);
        transform.localScale = escalaInicial * (1f + cop);
    }
}
