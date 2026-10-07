using UnityEngine;

public class EsquerController : MonoBehaviour
{
    public float velocitatBaixada = 1.5f;
    public float profunditatMaxima = 8f;
    public float velocitatPujada = 10f;
    bool pujant = false;

    void Update()
    {
        if (pujant)
        {
            transform.position += Vector3.up * velocitatPujada * Time.deltaTime;

            if (transform.position.y >= 0)
            {
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