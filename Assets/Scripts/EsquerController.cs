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
    public float[] multPuntsEsquer = { 1.5f, 1.25f, 1f };
    public float[] multVelocitatPeixos = { 0.75f, 1f, 1.2f };
    public Sprite[] spritesEsquer;

    public float yAigua = -1.25f;
    bool splashFet;

    enum Estat { Preparat, Baixant, Pujant, Resultat, Dialeg }
    Estat estat = Estat.Preparat;

    public CristDialeg crist;

    bool notaPendent;

    GameObject peixCapturat;
    PeixController peixDades;
    float profunditatCaptura;
    int puntsTotals = 0;

    public GeneradorPeixos generador;

    public TextMosaic hudPunts;
    public TextMosaic hudProfunditat;
    int puntsMostrats = 0;
    int metresMostrats = -1;

    public TextMosaic textResultat;

    public RevealPanel reveal;
    void Awake()
    {
        Partida.multVelocitatPeixos = multVelocitatPeixos[Partida.esquerTriat - 1];
    }
    void Start()
    {
        esquer = Partida.esquerTriat;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null && spritesEsquer != null && spritesEsquer.Length >= esquer)
        {
            sr.sprite = spritesEsquer[esquer - 1];
        }
        puntsTotals = Progres.Punts;
        puntsMostrats = puntsTotals;
        textResultat.Posa("TOCA PER LLANÇAR");
        textResultat.gameObject.SetActive(true);
        hudPunts.sufix = " PUNTS";
        ActualitzaHUD();

        if (!Partida.tutorialVist)
        {
            Partida.tutorialVist = true;
            estat = Estat.Dialeg;
            crist.Mostra(FrasesTutorial(), AcabaDialeg);
        }
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

            IniciaPujada();
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

        return Mathf.RoundToInt(100f * multZona * multMida * multRaresa * multPuntsEsquer[esquer - 1]);
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

    void IniciaDescens()
    {
        AudioManager.CanyaBaixa();
        splashFet = false;
        estat = Estat.Baixant;
    }

    void SoSplash()
    {
        AudioManager.Splash();
    }

    void IniciaPujada()
    {
        AudioManager.CanyaPuja();
        estat = Estat.Pujant;
    }

    bool CalRegals()
    {
        if (notaPendent)
        {
            notaPendent = false;
            int idx = NotesPesca.Desbloqueja();
            if (idx >= 0)
            {
                estat = Estat.Dialeg;
                string[] frases =
                {
                "Nota de pesca " + (idx + 1) + " de " + NotesPesca.Textos.Length,
                NotesPesca.Textos[idx]
            };
                crist.Mostra(frases, TornaDelsRegals);
                return true;
            }
        }

        Curiositats.Dades c = Curiositats.Pendent(puntsTotals);
        if (c != null)
        {
            Curiositats.Entrega();
            estat = Estat.Dialeg;
            string[] frases = { "Té! Una curiositat per al museu.", c.descripcio };
            crist.Mostra(frases, TornaDelsRegals, c.Imatge, c.nom);
            return true;
        }

        return false;
    }

    void TornaDelsRegals()
    {
        if (CalRegals()) { return; }
        generador.GeneraPeixos();
        IniciaDescens();
    }

    string[] FrasesTutorial()
    {
        return new string[]
        {
          "Benvingut al pantà de Sau, pescador!",
          "Toca la pantalla per llançar l'esquer i mou el dit per guiar-lo cap als peixos.",
          "Captura el primer peix que toquis. Les espècies autòctones s'alliberen i les invasores es retiren.",
          "Cada esquer té el seu caràcter: l'anzol és tranquil i paga més, el senyol és ràpid i arriba al fons.",
          "Els peixos amb halo porten una nota de pesca. Guarda-les al quadern!",
          "Quan sumis punts et regalaré curiositats pel museu. Bona pesca!"
        };
    }

    void AcabaDialeg()
    {
        estat = Estat.Preparat;
    }
    void Update()
    {
        ActualitzaProfunditat();

        if (estat == Estat.Preparat)
        {
            if (Input.GetMouseButtonDown(0))
            {
                textResultat.gameObject.SetActive(false);
                IniciaDescens();
            }
        }
        else if (estat == Estat.Baixant)
        {
            transform.position += Vector3.down * velocitatsBaixada[esquer - 1] * Time.deltaTime;

            if (!splashFet && transform.position.y <= yAigua)
            {
                splashFet = true;
                AudioManager.Splash();
            }

            Vector3 puntMon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xObjectiu = Mathf.Clamp(puntMon.x, -limitX, limitX);
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, xObjectiu, velocitatLateral * Time.deltaTime);
            transform.position = pos;

            if (transform.position.y <= -profunditatsMaximes[esquer - 1] * Escala.UnitatsPerMetre)
            {
                IniciaPujada();
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

                    bool nova;
                    bool record;
                    Pescadex.Registra(peixDades.especie, peixDades.mida, peixDades.pes, out nova, out record);

                    AudioManager.ResultatBo();
                    reveal.Mostra(peixDades.especie, peixDades.mida, peixDades.pes, punts, nova, record);

                    notaPendent = peixDades.portaNota;

                    Destroy(peixCapturat);
                    peixCapturat = null;
                    peixDades = null;
                }
                else
                {
                    AudioManager.ResultatDolent();
                    textResultat.Posa("NO HAS PESCAT RES");
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
                    if (CalRegals()) { return; }
                    generador.GeneraPeixos();
                    IniciaDescens();
                }
            }
        }
    }
}