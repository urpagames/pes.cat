using UnityEngine;

public class EsquerController : MonoBehaviour
{
    public float velocitatBaixada = 1.5f;
    public float profunditatMaxima = 8f;
    public float velocitatPujada = 10f;
    bool pujant = false;
    GameObject peixCapturat;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!pujant)
        {
            PeixController peix = other.GetComponent<PeixController>();
            peixCapturat = other.gameObject;

            Debug.Log("Has capturat una " + peix.especie.nom + " de " + peix.mida.ToString("F1") + " cm de longitud " + "i " + peix.pes.ToString("F2") + "kg de pes.");

            peix.enabled = false;
            other.transform.SetParent(transform);

            pujant = true;
        }
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
                    Destroy(peixCapturat);
                    peixCapturat = null;
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