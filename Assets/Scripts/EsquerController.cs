using UnityEngine;

public class EsquerController : MonoBehaviour
{
    public float velocitatBaixada = 1.5f;
    public float profunditatMaxima = 8f;
    public float velocitatPujada = 10f;
    bool pujant = false;
    GameObject peixCapturat;

    PeixController peixDades;
    float profunditatCaptura;
    int puntsTotals = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!pujant)
        {
            PeixController peix = other.GetComponent<PeixController>();
            peixCapturat = other.gameObject;

            Debug.Log("Has capturat una " + peix.especie.nom + " de " + peix.mida.ToString("F1") + " cm de longitud " + "i " + peix.pes.ToString("F2") + " kg de pes.");

            peixDades = peix;
            profunditatCaptura = -other.transform.position.y;

            peix.enabled = false;
            other.transform.SetParent(transform);

            pujant = true;
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

    void Update()
    {
        if (pujant)
        {
            transform.position += Vector3.up * velocitatPujada * Time.deltaTime;

            if (transform.position.y >= 0)
            {
                if (peixCapturat != null)
                {
                    int punts = CalculaPunts();
                    puntsTotals += punts;
                    Debug.Log("Punts: " + punts + " | Total: " + puntsTotals);

                    Destroy(peixCapturat);
                    peixCapturat = null;
                    peixDades = null;
                }
                transform.position = Vector3.zero;
                pujant = false;
                Debug.Log("Has tornat a la superfície");
            }
        }
        else
        {
            transform.position += Vector3.down * velocitatBaixada * Time.deltaTime;

            Vector3 puntMon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 pos = transform.position;
            pos.x = puntMon.x;
            transform.position = pos;

            if (transform.position.y <= -profunditatMaxima)
            {
                Debug.Log("No has pescat res");
                pujant = true;
            }
        }
    }
}