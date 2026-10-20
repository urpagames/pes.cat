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

    int ronda = 1;
    public int rondesTotals = 3;
    string resum = "";

    GameObject peixCapturat;
    PeixController peixDades;
    float profunditatCaptura;
    int puntsTotals = 0;

    public GeneradorPeixos generador;

    public TextMeshProUGUI textHUD;
    public TextMeshProUGUI textResultat;

    void Start()
    {
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

            Debug.Log("Has capturat una " + peix.especie.nom + " de " + peix.mida.ToString("F1") + " cm de longitud " + "i " + peix.pes.ToString("F2") + " kg de pes.");

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
        textHUD.text = "RONDA " + ronda + " de " + rondesTotals + "\nPUNTS: " + puntsTotals;
    }

    void AcabaRonda()
    {
        if (ronda >= rondesTotals)
        {
            estat = Estat.Resultat;
            textResultat.text = "CONCURS ACABAT\n\n" + resum + "\nTOTAL: " + puntsTotals + "\n\nToca per continuar";
            textResultat.gameObject.SetActive(true);
        }
        else
        {
            ronda++;
            estat = Estat.Baixant;
        }

        ActualitzaHUD();
    }

    void Update()
    {
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
                Debug.Log("No has pescat res");
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
                    resum += "Ronda " + ronda + ": " + peixDades.especie.nom + "\n" + peixDades.mida.ToString("F1") + " cm · " + peixDades.pes.ToString("F2") + " kg · " + punts + " punts\n\n";

                    Destroy(peixCapturat);
                    peixCapturat = null;
                    peixDades = null;
                }
                else
                {
                    resum += "Ronda " + ronda + ": sense captura\n";
                }

                AcabaRonda();
            }
        }
        else if (estat == Estat.Resultat)
        {
            if (Input.GetMouseButtonDown(0))
            {
                generador.GeneraPeixos();
                ronda = 1;
                puntsTotals = 0;
                resum = "";
                textResultat.text = "TOCA PER LLANÇAR";
                estat = Estat.Preparat;
                ActualitzaHUD();
            }
        }
    }
}