using UnityEngine;

public class PeixController : MonoBehaviour
{
    public int direccio = 1;
    public float limitESQ = -5f;
    public float limitDRET = 5f;

    public EspecieData especie;

    public float mida;
    public float pes;

    void Start()
    {
        mida = Random.Range(especie.midaMin, especie.midaMax);
        float proporcio = Mathf.InverseLerp(especie.midaMin, especie.midaMax, mida);
        pes = Mathf.Lerp(especie.pesMin, especie.pesMax, proporcio);
    }

    void Update()
    {
        if (direccio>0)
        {
            transform.position += Vector3.right * especie.velocitat * Time.deltaTime;

            if (transform.position.x >= limitDRET)
            {
                //girar l'sprite 180 graus 
                direccio = -1;
            }
        }
        else
        {
            transform.position += Vector3.left * especie.velocitat * Time.deltaTime;

            if (transform.position.x <= limitESQ)
            {
                //girar l'sprite 180 graus 
                direccio = 1;
            }
        }
    }
}