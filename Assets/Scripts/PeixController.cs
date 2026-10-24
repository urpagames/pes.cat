using UnityEngine;

public class PeixController : MonoBehaviour
{
    public int direccio = 1;
    public float limitESQ = -2.5f;
    public float limitDRET = 2.5f;

    public EspecieData especie;

    public float mida;
    public float pes;

    SpriteRenderer sr;

    void Start()
    {
        float velocitat = especie.velocitat;
        mida = Random.Range(especie.midaMin, especie.midaMax);
        float proporcio = Mathf.InverseLerp(especie.midaMin, especie.midaMax, mida);
        pes = Mathf.Lerp(especie.pesMin, especie.pesMax, proporcio);

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = especie.imatge;

        float meitat = sr.sprite.bounds.extents.x;
        limitDRET = 2.8f - meitat;
        limitESQ = -(2.8f - meitat);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, limitESQ, limitDRET), transform.position.y, 0f);

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.size = sr.sprite.bounds.size * 0.6f;
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
        sr.flipX = direccio < 0;
    }
}