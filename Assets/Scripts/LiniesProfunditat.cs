using UnityEngine;

public class LiniesProfunditat : MonoBehaviour
{
    public Canvas canvas;
    public Transform[] linies;
    public RectTransform[] etiquetes;
    public float desplacamentX = 110f;
    public float desplacamentY = 30f;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        float factor = canvas.scaleFactor;

        for (int i = 0; i < linies.Length; i++)
        {
            Vector3 pantalla = cam.WorldToScreenPoint(linies[i].position);
            bool visible = pantalla.y > -60f && pantalla.y < Screen.height + 60f;
            etiquetes[i].gameObject.SetActive(visible);
            if (!visible) { continue; }

            etiquetes[i].position = new Vector3(desplacamentX * factor, pantalla.y + desplacamentY * factor, 0f);
        }
    }
}