using UnityEngine;

public class CamaraSegueix : MonoBehaviour
{
    public Transform objectiu;
    public float desplacament = -2f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = objectiu.position.y - desplacament;
        transform.position = pos;
    }
}