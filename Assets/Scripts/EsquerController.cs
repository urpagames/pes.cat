using UnityEngine;
using TMPro;

public class EsquerController : MonoBehaviour
{
    public int esquer = 1;
    public float[] profunditatsMaximes = { 8f, 16f, 24f };
    public float[] velocitatsBaixada = { 2f, 2.6f, 3.2f };
    public float velocitatPujada = 15f;
    public float velocitatLateral = 20f;
    public float limitX = 2.4f;

    enum Estat { Preparat, Baixant, Pujant, Resultat }
    Estat estat = Estat.Preparat;

    GameObject peixCapturat;
    PeixController peixDades;
    float profunditatCaptura;
    int puntsTotals = 0;

    public GeneradorPeixos generador;

    public TextMosaic hudPunts;
    public TextMosaic hudProfunditat;
    int puntsMostrats = 0;
    int metresMostrats = -1;

    public TextMeshProUGUI textResultat;

    public RevealPanel reveal;

    void Start()
    {
        esquer = Partida.esquerTriat;
        puntsTotals = Progres.Punts;
        puntsMostrats = puntsTotals;
        textResultat.text = "TOCA PER LLANÇAR";
        textResultat.gameObject.SetActive(true);
        ActualitzaHUD();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (estat == Estat.Baixant)
        {
            PeixController peix = other.GetComponent<PeixController>();
            if (peix == null) { return; }

            peixCapturat = other.gameObject;
            peixDades = peix;
            profunditatCaptura = -other.transform.position.y / Escala.UnitatsPerMetre;

            peix.enabled = false;
            other.transform.SetParent(transform);

            estat = Estat.Pujant;
        }
    }

    float MultiplicadorZona(float profunditat)
    {
        if (profunditat < 8f)
        {
            return 1f;
        }
        else if (profunditat < 16f)
        {
            return 2f;
        }
        else
        {
            return 3f;
        }
    }

    int CalculaPunts()
    {
        EspecieData e = peixDades.especie;

        float multZona = MultiplicadorZona(profunditatCaptura);
        float proporcio = Mathf.InverseLerp(e.midaMin, e.midaMax, peixDades.mida);
        float multMida = 1f + proporcio;
        float multRaresa = 10f / e.probabilitat;

        return Mathf.RoundToInt(100f * multZona * multMida * multRaresa);
    }

    void ActualitzaHUD()
    {
        hudPunts.ContaFins(puntsTotals, 0.6f, puntsMostrats);
        puntsMostrats = puntsTotals;
    }

    void ActualitzaProfunditat()
    {
        int metres = Mathf.Clamp(Mathf.FloorToInt(-transform.position.y / Escala.UnitatsPerMetre), 0, 99);
        if (metres == metresMostrats) { return; }
        metresMostrats = metres;
        hudProfunditat.Posa(metres + " M");
    }

    void Update()
    {
        ActualitzaProfunditat();

        if (estat == Estat.Preparat)
        {
            if (Input.GetMouseButtonDown(0))
            {
                textResultat.gameObject.SetActive(false);
                estat = Estat.Baixant;
            }
        }
        else if (estat == Estat.Baixant)
        {
            transform.position += Vector3.down * velocitatsBaixada[esquer - 1] * Time.deltaTime;

            Vector3 puntMon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xObjectiu = Mathf.Clamp(puntMon.x, -limitX, limitX);
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, xObjectiu, velocitatLateral * Time.deltaTime);
            transform.position = pos;

            if (transform.position.y <= -profunditatsMaximes[esquer - 1] * Escala.UnitatsPerMetre)
            {
                estat = Estat.Pujant;
            }
        }
        else if (estat == Estat.Pujant)
        {
            Vector3 desti = new Vector3(transform.position.x, 0f, 0f);
            transform.position = Vector3.MoveTowards(transform.position, desti, velocitatPujada * Time.deltaTime);

            if (transform.position.y >= 0f)
            {
                if (peixCapturat != null)
                {
                    int punts = CalculaPunts();
                    puntsTotals += punts;
                    Progres.Punts = puntsTotals;

                    reveal.Mostra(peixDades.especie, peixDades.mida, peixDades.pes, punts);

                    Destroy(peixCapturat);
                    peixCapturat = null;
                    peixDades = null;
                }
                else
                {
                    textResultat.text = "No has pescat res\n\nToca per llançar";
                    textResultat.gameObject.SetActive(true);
                }

                estat = Estat.Resultat;
            }
        }
        else if (estat == Estat.Resultat)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (reveal.Ocupat)
                {
                    reveal.Salta();
                }
                else
                {
                    if (reveal.Visible) { reveal.Amaga(); }
                    textResultat.gameObject.SetActive(false);
                    ActualitzaHUD();
                    generador.GeneraPeixos();
                    estat = Estat.Baixant;
                }
            }
        }
    }
}