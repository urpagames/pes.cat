using UnityEngine;

public class LiniesProfunditat : MonoBehaviour
{
    public Canvas canvas;
    public Transform[] linies;
    public RectTransform[] etiquetes;
    public float desplacamentX = 110f;
    public float desplacamentY = 30f;

    RectTransform rectCanvas;
    Camera cam;

    void Start()
    {
        rectCanvas = canvas.GetComponent<RectTransform>();
        cam = Camera.main;
    }

    void LateUpdate()
    {
        for (int i = 0; i < linies.Length; i++)
        {
            Vector3 pantalla = cam.WorldToScreenPoint(linies[i].position);
            bool visible = pantalla.y > -60f && pantalla.y < Screen.height + 60f;
            etiquetes[i].gameObject.SetActive(visible);
            if (!visible) { continue; }

            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectCanvas, new Vector2(0f, pantalla.y), null, out local);
            etiquetes[i].anchoredPosition = new Vector2(local.x + desplacamentX, local.y + desplacamentY);
        }
    }
}