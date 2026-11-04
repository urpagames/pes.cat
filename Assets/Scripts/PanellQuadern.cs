using UnityEngine;

public class PanellQuadern : MonoBehaviour
{
    public CartaNota prefabCarta;
    public RectTransform contingut;
    public TextMosaic textResum;

    CartaNota[] cartes;

    public void Obre()
    {
        gameObject.SetActive(true);

        if (cartes == null)
        {
            cartes = new CartaNota[NotesPesca.Textos.Length];
            for (int i = 0; i < cartes.Length; i++)
            {
                cartes[i] = Instantiate(prefabCarta, contingut);
            }
        }

        for (int i = 0; i < cartes.Length; i++) { cartes[i].Omple(i); }
        textResum.Posa(NotesPesca.Quantes + " DE " + NotesPesca.Textos.Length);
    }

    public void Tanca()
    {
        gameObject.SetActive(false);
    }
}