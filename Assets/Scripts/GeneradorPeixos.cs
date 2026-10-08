using UnityEngine;

public class GeneradorPeixos : MonoBehaviour
{
    public GameObject prefabPeix;
    public int quantitat = 6;

    void Start()
    {
        for (int i = 0; i < quantitat; i++)
        {
            float x = Random.Range(-2.5f, 2.5f);
            float y = -Random.Range(1f, 8f);
            Vector3 posicio = new Vector3(x, y, 0f);
            Instantiate(prefabPeix, posicio, Quaternion.identity);
        }
    }
}