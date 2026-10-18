using UnityEngine;

public class GeneradorPeixos : MonoBehaviour
{
    public GameObject prefabPeix;
    public EspecieData[] especies;
    public int quantitat = 10;

    EspecieData TriaEspecie()
    {
        float sumaTotal = 0f;
        for (int j = 0; j < especies.Length; j++)
        {
            sumaTotal += especies[j].probabilitat;
        }

        float tirada = Random.Range(0f, sumaTotal);

        float acumulat = 0f;
        for (int j = 0; j < especies.Length; j++)
        {
            acumulat += especies[j].probabilitat;
            if (tirada <= acumulat)
            {
                return especies[j];
            }
        }

        return especies[0];
    }

    public void GeneraPeixos()
    {
        foreach (Transform fill in transform)
        {
            Destroy(fill.gameObject);
        }

        for (int i = 0; i < quantitat; i++)
        {
            EspecieData especieTriada = TriaEspecie();

            float x = Random.Range(-2.5f, 2.5f);
            float y = -Random.Range(especieTriada.profunditatMin, especieTriada.profunditatMax);
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