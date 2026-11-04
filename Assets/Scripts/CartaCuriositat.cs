using UnityEngine;
using UnityEngine.UI;

public class CartaCuriositat : MonoBehaviour
{
    public Image imatge;
    public TextMosaic nom;
    public TextMosaic descripcio;
    public RectTransform omple;
    public TextMosaic textPunts;
    public bool mostraDescripcio = true;

    public void Omple(Curiositats.Dades c, bool tinguda, int punts)
    {
        Sprite s = c.Imatge;
        imatge.enabled = s != null;
        imatge.sprite = s;
        imatge.preserveAspect = true;
        imatge.color = tinguda ? Color.white : Color.black;

        nom.Posa(tinguda ? c.nom : "PER DESCOBRIR");

        bool veuDescripcio = tinguda && mostraDescripcio;
        descripcio.gameObject.SetActive(veuDescripcio);
        if (veuDescripcio) { descripcio.Posa(c.descripcio.ToUpperInvariant()); }

        int mostrats = tinguda ? c.punts : Mathf.Min(punts, c.punts);
        float fraccio = Mathf.Clamp01((float)mostrats / c.punts);

        omple.anchorMin = new Vector2(0f, 0f);
        omple.anchorMax = new Vector2(fraccio, 1f);
        omple.offsetMin = Vector2.zero;
        omple.offsetMax = Vector2.zero;

        textPunts.Posa(TextMosaic.Formata(mostrats) + " DE " + TextMosaic.Formata(c.punts) + " PUNTS");
    }
}