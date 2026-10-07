using UnityEngine;

public class PeixController : MonoBehaviour
{
    public float velocitat = 3f;
    public int direccio = 1;
    public float limitESQ = -5f;
    public float limitDRET = 5f;

    public string nom = "Truita";
    public float midaMin = 20f;
    public float midaMax = 45f;
    public float pesMin = 0.2f;
    public float pesMax = 1.5f;

    public float mida;
    public float pes;

    void Start()
    {
        mida = Random.Range(midaMin, midaMax);
        float proporcio = Mathf.InverseLerp(midaMin, midaMax, mida);
        pes = Mathf.Lerp(pesMin, pesMax, proporcio);
    }

    void Update()
    {
        if (direccio>0)
        {
            transform.position += Vector3.right * velocitat * Time.deltaTime;

            if (transform.position.x >= limitDRET)
            {
                //girar l'sprite 180 graus 
                direccio = -1;
            }
        }
        else
        {
            transform.position += Vector3.left * velocitat * Time.deltaTime;

            if (transform.position.x <= limitESQ)
            {
                //girar l'sprite 180 graus 
                direccio = 1;
            }
        }
    }
}