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
        imatge.sprite = c.Imatge;
        imatge.preserveAspect = true;
        imatge.color = tinguda ? Color.white : Color.black;

        nom.Posa(tinguda ? c.nom : "PER DESCOBRIR");

        descripcio.gameObject.SetActive(tinguda);
        if (tinguda) { descripcio.Posa(c.descripcio.ToUpperInvariant()); }

        int mostrats = tinguda ? c.punts : Mathf.Min(punts, c.punts);
        float fraccio = Mathf.Clamp01((float)mostrats / c.punts);

        omple.anchorMin = new Vector2(0f, 0f);
        omple.anchorMax = new Vector2(fraccio, 1f);
        omple.offsetMin = Vector2.zero;
        omple.offsetMax = Vector2.zero;

        textPunts.Posa(TextMosaic.Formata(mostrats) + " / " + TextMosaic.Formata(c.punts) + " PUNTS");
    }
}