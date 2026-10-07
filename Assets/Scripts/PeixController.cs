using UnityEngine;

public class PeixController : MonoBehaviour
{
    public float velocitat = 3f;
    public int direccio = 1;
    public float limitESQ = -5f;
    public float limitDRET = 5f;

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