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

    public bool portaNota;
    public Sprite spriteHalo;
    public Color colorHalo = new Color(1f, 0.85f, 0.3f, 0.85f);
    Transform halo;

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

        if (portaNota && spriteHalo != null) { CreaHalo(); }
    }

    void CreaHalo()
    {
        GameObject go = new GameObject("Halo");
        go.transform.SetParent(transform, false);

        SpriteRenderer h = go.AddComponent<SpriteRenderer>();
        h.sprite = spriteHalo;
        h.color = colorHalo;
        h.sortingLayerID = sr.sortingLayerID;
        h.sortingOrder = sr.sortingOrder - 1;

        float amplePeix = sr.bounds.size.x;
        float ampleHalo = spriteHalo.bounds.size.x * transform.lossyScale.x;
        go.transform.localScale = Vector3.one * (amplePeix * 1.3f / ampleHalo);
        halo = go.transform;
    }

    void Update()
    {
        if (direccio>0)
        {
            transform.position += Vector3.right * especie.velocitat * Time.deltaTime * Partida.multVelocitatPeixos;

            if (transform.position.x >= limitDRET)
            {
                //girar l'sprite 180 graus 
                direccio = -1;
            }
        }
        else
        {
            transform.position += Vector3.left * especie.velocitat * Time.deltaTime * Partida.multVelocitatPeixos;

            if (transform.position.x <= limitESQ)
            {
                //girar l'sprite 180 graus 
                direccio = 1;
            }
        }
        sr.flipX = direccio < 0;
        if (halo != null) { halo.Rotate(0f, 0f, -40f * Time.deltaTime); }
    }
}