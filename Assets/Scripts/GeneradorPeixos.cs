using UnityEngine;

public class GeneradorPeixos : MonoBehaviour
{
    public GameObject prefabPeix;
    public EspecieData[] especies;
    public int quantitat = 30;
    public float profunditatTotal = 24f;

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

        float franja = profunditatTotal / quantitat;

        for (int i = 0; i < quantitat; i++)
        {
            float profunditat = (i + Random.Range(0.2f, 0.8f)) * franja;

            EspecieData especieTriada = TriaEspecie(profunditat);
            if (especieTriada == null)
            {
                continue;
            }

            float x = Random.Range(-2.5f, 2.5f);
            float y = -profunditat * Escala.UnitatsPerMetre;
            Vector3 posicio = new Vector3(x, y, 0f);

            GameObject nou = Instantiate(prefabPeix, posicio, prefabPeix.transform.rotation, transform);
            nou.GetComponent<PeixController>().especie = especieTriada;
        }
    }

    void Start()
    {
        GeneraPeixos();
    }
}