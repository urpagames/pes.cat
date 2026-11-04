using UnityEngine;

public class PanellMuseu : MonoBehaviour
{
    public CartaCuriositat prefabCarta;
    public RectTransform contingut;
    public TextMosaic textResum;

    CartaCuriositat[] cartes;

    public void Obre()
    {
        gameObject.SetActive(true);
        Curiositats.Sincronitza(Progres.Punts);

        if (cartes == null)
        {
            cartes = new CartaCuriositat[Curiositats.Llista.Length];
            for (int i = 0; i < cartes.Length; i++)
            {
                cartes[i] = Instantiate(prefabCarta, contingut);
            }
        }

        int tingudes = 0;
        for (int i = 0; i < cartes.Length; i++)
        {
            bool tinguda = Curiositats.Tinguda(i);
            if (tinguda) { tingudes++; }
            cartes[i].Omple(Curiositats.Llista[i], tinguda, Progres.Punts);
        }
        textResum.Posa(tingudes + " / " + Curiositats.Llista.Length);
    }

    public void Tanca()
    {
        gameObject.SetActive(false);
    }
}