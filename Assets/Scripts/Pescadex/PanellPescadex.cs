using System;
using UnityEngine;

public class PanellPescadex : MonoBehaviour
{
    public EspecieData[] especies;
    public CartaEspecie prefabCarta;
    public RectTransform contingut;
    public TextMosaic textResum;

    EspecieData[] ordenades;
    CartaEspecie[] cartes;

    public void Obre()
    {
        gameObject.SetActive(true);
        if (cartes == null) { Crea(); }
        Actualitza();
    }

    public void Tanca()
    {
        gameObject.SetActive(false);
    }

    void Crea()
    {
        ordenades = (EspecieData[])especies.Clone();
        Array.Sort(ordenades, (a, b) => a.profunditatMin.CompareTo(b.profunditatMin));

        cartes = new CartaEspecie[ordenades.Length];
        for (int i = 0; i < ordenades.Length; i++)
        {
            cartes[i] = Instantiate(prefabCarta, contingut);
        }
    }

    void Actualitza()
    {
        int descobertes = 0;
        for (int i = 0; i < ordenades.Length; i++)
        {
            cartes[i].Omple(ordenades[i]);
            if (Pescadex.Descoberta(ordenades[i])) { descobertes++; }
        }
        textResum.Posa(descobertes + " DE " + ordenades.Length);
    }
}