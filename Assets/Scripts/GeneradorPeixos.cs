using UnityEngine;

public class GeneradorPeixos : MonoBehaviour
{
    public GameObject prefabPeix;
    public EspecieData[] especies;
    public int quantitat = 6;

    void Start()
    {
        for (int i = 0; i < quantitat; i++)
        {
            int index = Random.Range(0, especies.Length);
            EspecieData especieTriada = especies[index];

            float x = Random.Range(-2.5f, 2.5f);
            float y = -Random.Range(especieTriada.profunditatMin, especieTriada.profunditatMax);
            Vector3 posicio = new Vector3(x, y, 0f);

            GameObject nou = Instantiate(prefabPeix, posicio, prefabPeix.transform.rotation);
            nou.GetComponent<PeixController>().especie = especieTriada;
        }
    }
}