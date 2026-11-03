using UnityEngine;

public class GeneradorPeixos : MonoBehaviour
{
    public GameObject prefabPeix;
    public EspecieData[] especies;
    public int quantitat = 30;
    public float profunditatTotal = 24f;

    public float profunditatInicial = 1f;
    public float jitterMin = 0.35f;
    public float jitterMax = 0.65f;

    bool VivAqui(EspecieData e, float profunditat)
    {
        return profunditat >= e.profunditatMin && profunditat <= e.profunditatMax;
    }

    EspecieData TriaEspecie(float profunditat)
    {
        float sumaTotal = 0f;
        for (int j = 0; j < especies.Length; j++)
        {
            if (VivAqui(especies[j], profunditat))
            {
                sumaTotal += especies[j].probabilitat;
            }
        }

        if (sumaTotal <= 0f)
        {
            return null;
        }

        float tirada = Random.Range(0f, sumaTotal);

        float acumulat = 0f;
        for (int j = 0; j < especies.Length; j++)
        {
            if (VivAqui(especies[j], profunditat))
            {
                acumulat += especies[j].probabilitat;
                if (tirada <= acumulat)
                {
                    return especies[j];
                }
            }
        }

        return null;
    }

    public void GeneraPeixos()
    {
        foreach (Transform fill in transform)
        {
            Destroy(fill.gameObject);
        }

        float franja = (profunditatTotal - profunditatInicial) / quantitat;

        for (int i = 0; i < quantitat; i++)
        {
            float profunditat = profunditatInicial + (i + Random.Range(jitterMin, jitterMax)) * franja;

            EspecieData especieTriada = TriaEspecie(profunditat);
            if (especieTriada == null)
            {
                continue;
            }

            float x = Random.Range(-2.5f, 2.5f);
            float y = -profunditat * Escala.UnitatsPerMetre;
            Vector3 posicio = new Vector3(x, y, 0f);

            GameObject nou = Instantiate(prefabPeix, posicio, prefabPeix.transform.rotation, transform);
            PeixController pc = nou.GetComponent<PeixController>();
            pc.especie = especieTriada;
            pc.direccio = Random.value < 0.5f ? 1 : -1;
        }
    }

    void Start()
    {
        GeneraPeixos();
    }
}