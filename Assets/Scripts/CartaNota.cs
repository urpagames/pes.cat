using UnityEngine;
using UnityEngine.UI;

public class CartaNota : MonoBehaviour
{
    public Image fons;
    public TextMosaic titol;
    public TextMosaic text;

    public void Omple(int i)
    {
        bool trobada = NotesPesca.Trobada(i);
        fons.color = trobada ? Color.white : new Color(0.55f, 0.55f, 0.55f, 1f);
        titol.Posa("NOTA " + (i + 1));
        text.Posa(trobada ? NotesPesca.Textos[i].ToUpperInvariant() : "PER DESCOBRIR");
    }
}