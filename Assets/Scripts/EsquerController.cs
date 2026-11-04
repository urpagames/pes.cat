using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Canvi d'esquer")]
    public Button[] botonsEsquer;
    public string[] nomsEsquer = { "l'ham", "la cullera", "el peix artificial" };
    public string[] descripcionsEsquer =
    {
        "L'ham senzil és tranquil i paga més punts.",
        "La cullera baixa més fons i va més ràpida, però paga una mica menys.",
        "El peix artificial arriba al fons del pantà, però els peixos van més ràpids."
    };
    const string pistaCanvi = " Canvia d'esquer amb els botons de dalt.";

    public RectTransform[] zonesSenseLlancament;

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

    SpriteRenderer srEsquer;

    void Awake()
    {
        Partida.multVelocitatPeixos = multVelocitatPeixos[Partida.esquerTriat - 1];
    }

    void Start()
    {
        esquer = Partida.esquerTriat;
        srEsquer = GetComponentInChildren<SpriteRenderer>();
        AplicaSpriteEsquer();
        Partida.esquerAvisat = Mathf.Max(Partida.esquerAvisat, esquer);

        if (botonsEsquer != null)
        {
            for (int i = 0; i < botonsEsquer.Length; i++)
            {
                int n = i + 1;
                if (botonsEsquer[i] != null)
                {
                    botonsEsquer[i].onClick.AddListener(() => TriaEsquer(n));
                }
            }
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

    void AplicaSpriteEsquer()
    {
        if (srEsquer != null && spritesEsquer != null && spritesEsquer.Length >= esquer)
        {
            srEsquer.sprite = spritesEsquer[esquer - 1];
        }
    }

    void EquipaEsquer(int n)
    {
        esquer = n;
        Partida.esquerTriat = n;
        Partida.multVelocitatPeixos = multVelocitatPeixos[n - 1];
        AplicaSpriteEsquer();
    }

    void TriaEsquer(int n)
    {
        if (estat != Estat.Preparat) { return; }
        if (!Progres.EsquerDesbloquejat(n)) { return; }
        if (n == esquer) { return; }
        EquipaEsquer(n);
    }

    int MaxEsquerDesbloquejat()
    {
        int max = 1;
        for (int n = 1; n <= Progres.llindars.Length; n++)
        {
            if (Progres.EsquerDesbloquejat(n)) { max = n; }
        }
        return max;
    }

    void ActualitzaBotonsEsquer()
    {
        if (botonsEsquer == null) { return; }

        bool mostra = (estat == Estat.Preparat);
        for (int i = 0; i < botonsEsquer.Length; i++)
        {
            if (botonsEsquer[i] == null) { continue; }
            int n = i + 1;
            bool visible = mostra && n != esquer && Progres.EsquerDesbloquejat(n);
            if (botonsEsquer[i].gameObject.activeSelf != visible)
            {
                botonsEsquer[i].gameObject.SetActive(visible);
            }
        }
    }

    bool TocSobreUI()
    {
        if (botonsEsquer != null)
        {
            for (int i = 0; i < botonsEsquer.Length; i++)
            {
                if (botonsEsquer[i] == null || !botonsEsquer[i].gameObject.activeInHierarchy) { continue; }
                RectTransform rt = (RectTransform)botonsEsquer[i].transform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, null))
                {
                    return true;
                }
            }
        }

        if (zonesSenseLlancament != null)
        {
            for (int i = 0; i < zonesSenseLlancament.Length; i++)
            {
                if (zonesSenseLlancament[i] == null || !zonesSenseLlancament[i].gameObject.activeInHierarchy) { continue; }
                if (RectTransformUtility.RectangleContainsScreenPoint(zonesSenseLlancament[i], Input.mousePosition, null))
                {
                    return true;
                }
            }
        }

        return false;
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
        if (MaxEsquerDesbloquejat() > Partida.esquerAvisat)
        {
            Partida.esquerAvisat++;
            int n = Partida.esquerAvisat;
            EquipaEsquer(n);
            estat = Estat.Dialeg;

            string[] frasesEsquer =
            {
                "Esquer nou: " + nomsEsquer[n - 1] + "! Te l'he equipat.",
                descripcionsEsquer[n - 1] + pistaCanvi
            };
            Sprite spriteNou = (spritesEsquer != null && spritesEsquer.Length >= n) ? spritesEsquer[n - 1] : null;
            crist.Mostra(frasesEsquer, TornaDelsRegals, spriteNou, null);
            return true;
        }

        if (notaPendent)
        {
            notaPendent = false;
            int idx = NotesPesca.Desbloqueja();
            if (idx >= 0)
            {
                estat = Estat.Dialeg;
                string[] frases =
                {
                    "Has aconseguit una nota de pesca! " + ComptadorNotes(),
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
            string[] frases = { "Té! Una curiositat per al museu: " + c.nom, c.descripcio };
            crist.Mostra(frases, TornaDelsRegals, c.Imatge, null);
            return true;
        }

        return false;
    }

    void TornaDelsRegals()
    {
        if (CalRegals()) { return; }
        generador.GeneraPeixos();
        textResultat.gameObject.SetActive(true);
        textResultat.Posa("TOCA PER LLANÇAR");
        estat = Estat.Preparat;
    }

    static readonly string[] nombresFemenins =
{
        "cap", "una", "dues", "tres", "quatre", "cinc", "sis", "set", "vuit", "nou", "deu",
        "onze", "dotze", "tretze", "catorze", "quinze", "setze", "disset", "divuit", "dinou", "vint"
    };

    string NomNumero(int n)
    {
        return (n >= 0 && n < nombresFemenins.Length) ? nombresFemenins[n] : n.ToString();
    }

    string ComptadorNotes()
    {
        int tinc = NotesPesca.Quantes;
        int total = NotesPesca.Textos.Length;
        if (tinc >= total) { return "Ja les tens totes!"; }
        return "Ja en tens " + NomNumero(tinc) + " de " + NomNumero(total) + ".";
    }

    string[] FrasesTutorial()
    {
        return new string[]
        {
          "Benvingut al pantà de Sau, pescador!",
          "Necessito la teva ajuda. Resulta que em vaig prendre massa seriosament allò de multiplicar peixos...",
          "I ara el pantà està desbordat d'espècies invasores.",
          "Necessito algú que conegui el poder de la pesca recreativa. M'has d'ajudar.",
          "Toca la pantalla per llançar l'esquer i mou el dit per guiar-lo cap als peixos.",
          "Captura el primer peix que toquis. Les espècies autòctones s'alliberen i les invasores es retiren.",
          "Cada esquer té el seu caràcter: l'ham és tranquil i paga més, però el peix artificial és ràpid i arriba al fons.",
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
        ActualitzaBotonsEsquer();

        if (estat == Estat.Preparat)
        {
            if (Input.GetMouseButtonDown(0) && !TocSobreUI())
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
            if (Input.GetMouseButtonDown(0) && !TocSobreUI())
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