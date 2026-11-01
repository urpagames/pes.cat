using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class AjustaCanvas : MonoBehaviour
{
    CanvasScaler escalador;
    int ample = -1;
    int alt = -1;

    void Awake()
    {
        escalador = GetComponent<CanvasScaler>();
        Aplica();
    }

    void Update()
    {
        if (Screen.width != ample || Screen.height != alt)
        {
            Aplica();
        }
    }

    void Aplica()
    {
        ample = Screen.width;
        alt = Screen.height;
        if (alt <= 0) { return; }

        float aspecte = (float)ample / alt;

        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1080f, 1920f);
        escalador.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        escalador.matchWidthOrHeight = (aspecte < 1080f / 1920f) ? 0f : 1f;
    }
}