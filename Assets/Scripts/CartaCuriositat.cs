using UnityEngine;
using UnityEngine.UI;

public class CartaCuriositat : MonoBehaviour
{
    public Image imatge;
    public TextMosaic nom;
    public TextMosaic descripcio;
    public RectTransform omple;
    public TextMosaic textPunts;

    public void Omple(Curiositats.Dades c, bool tinguda, int punts)
    {
        Sprite s = c.Imatge;
        imatge.enabled = s != null;
        imatge.sprite = s;
        imatge.preserveAspect = true;
        imatge.color = tinguda ? Color.white : Color.black;

        GameObject barra = omple.parent.gameObject;

        nom.gameObject.SetActive(tinguda);
        descripcio.gameObject.SetActive(tinguda);
        barra.SetActive(!tinguda);
        textPunts.gameObject.SetActive(!tinguda);

        if (tinguda)
        {
            nom.Posa(c.nom);
            descripcio.Posa(c.descripcio.ToUpperInvariant());
            return;
        }

        int mostrats = Mathf.Min(punts, c.punts);
        float fraccio = Mathf.Clamp01((float)mostrats / c.punts);

        omple.anchorMin = new Vector2(0f, 0f);
        omple.anchorMax = new Vector2(fraccio, 1f);
        omple.offsetMin = Vector2.zero;
        omple.offsetMax = Vector2.zero;

        textPunts.Posa(TextMosaic.Formata(mostrats) + " DE " + TextMosaic.Formata(c.punts));
    }
}