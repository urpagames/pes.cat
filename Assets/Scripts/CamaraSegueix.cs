using UnityEngine;

public class CamaraSegueix : MonoBehaviour
{
    public Transform objectiu;
    void LateUpdate()
    {
        Vector3 puntObj = objectiu.transform.position;
        Vector3 pos = transform.position;
        pos.y = puntObj.y;
        transform.position = pos;
    }
}
