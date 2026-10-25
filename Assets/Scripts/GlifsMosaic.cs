using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlifsMosaic", menuName = "Pescat/Glifs mosaic")]
public class GlifsMosaic : ScriptableObject
{
    public Sprite[] glifs;

    public float baseDigits = 90f;
    public float baseLletres = 70f;

    Dictionary<string, Sprite> diccionari;

    void OnValidate()
    {
        diccionari = null;
    }

    void Construeix()
    {
        diccionari = new Dictionary<string, Sprite>();
        foreach (Sprite s in glifs)
        {
            if (s != null) { diccionari[s.name] = s; }
        }
    }

    static string Clau(char c)
    {
        if (c >= '0' && c <= '9') { return "digit_" + c; }
        if (c >= 'A' && c <= 'Z') { return "lletra_" + c; }

        switch (c)
        {
            case '.': return "simbol_punt";
            case ',': return "simbol_coma";
            case '+': return "simbol_mes";
            case '!': return "simbol_exclamacio";
            case '?': return "simbol_interrogant";
            case ':': return "simbol_dospunts";
            case '-': return "simbol_guio";
            case '\'': return "simbol_apostrof";
            case '%': return "simbol_percent";
            case 'À': return "lletra_A_grave";
            case 'È': return "lletra_E_grave";
            case 'É': return "lletra_E_agut";
            case 'Í': return "lletra_I_agut";
            case 'Ï': return "lletra_I_dieresi";
            case 'Ò': return "lletra_O_grave";
            case 'Ó': return "lletra_O_agut";
            case 'Ú': return "lletra_U_agut";
            case 'Ü': return "lletra_U_dieresi";
            case 'Ç': return "lletra_C_cedilla";
        }
        return null;
    }

    static char SenseAccent(char c)
    {
        switch (c)
        {
            case 'À': return 'A';
            case 'È': case 'É': return 'E';
            case 'Í': case 'Ï': return 'I';
            case 'Ò': case 'Ó': return 'O';
            case 'Ú': case 'Ü': return 'U';
            case 'Ç': return 'C';
        }
        return c;
    }

    public Sprite Busca(char c, out float linia)
    {
        if (diccionari == null) { Construeix(); }

        c = char.ToUpperInvariant(c);
        linia = baseLletres;

        string clau = Clau(c);
        if (clau == null) { return null; }

        Sprite s;
        if (!diccionari.TryGetValue(clau, out s))
        {
            // Si falta la lletra amb accent, fem servir la que no en té
            char alternativa = SenseAccent(c);
            if (alternativa == c) { return null; }
            if (!diccionari.TryGetValue(Clau(alternativa), out s)) { return null; }
        }

        if ((c >= '0' && c <= '9') || c == '+') { linia = baseDigits; }
        return s;
    }
}
