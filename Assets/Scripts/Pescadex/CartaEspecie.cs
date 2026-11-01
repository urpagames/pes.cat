using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CartaEspecie : MonoBehaviour
{
    public Image imatge;
    public TextMosaic nom;
    public TextMosaic mida;
    public TextMosaic pes;

    static string Decimal(float valor, string format)
    {
        return valor.ToString(format, CultureInfo.InvariantCulture).Replace('.', ',');
    }

    public void Omple(EspecieData e)
    {
        imatge.sprite = e.imatge;
        imatge.preserveAspect = true;

        Pescadex.Registre r = Pescadex.Obte(e);

        if (r == null)
        {
            imatge.color = Color.black;
            nom.Posa("PER DESCOBRIR");
            mida.gameObject.SetActive(false);
            pes.gameObject.SetActive(false);
        }
        else
        {
            imatge.color = Color.white;
            nom.Posa(e.nom.ToUpperInvariant());
            mida.gameObject.SetActive(true);
            pes.gameObject.SetActive(true);
            mida.Posa("MIDA " + Decimal(r.recordMida, "F1") + " CM");
            pes.Posa("PES " + Decimal(r.recordPes, "F2") + " KG");
        }
    }
}