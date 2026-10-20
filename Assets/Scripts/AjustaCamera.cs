using UnityEngine;

public class AjustaCamera : MonoBehaviour
{
    public float mitjaAmplada = 2.8f;
    public float mitjaAlcada = 5f;

    void Update()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = Mathf.Max(mitjaAlcada, mitjaAmplada / cam.aspect);
    }
}