using UnityEngine;

public class EsquerController : MonoBehaviour
{
    public float velocitatBaixada = 3f;

    void Update()
    {
        transform.position += Vector3.down * velocitatBaixada * Time.deltaTime;
    }
}