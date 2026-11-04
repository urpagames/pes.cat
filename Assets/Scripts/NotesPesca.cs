using System.Collections.Generic;
using UnityEngine;

public static class NotesPesca
{
    public const int MaxCaracters = 125;

    public static readonly string[] Textos =
    {
        "Per pescar en un riu o pantà cal una llicència de pesca recreativa. Serveix també per pescar al mar en superfície.",
        "Els menors i la gent gran no paguen la llicència. Els menors necessiten l'autorització del pare, la mare o el tutor.",
        "La llicència pot ser d'un dia, de dos dies, de quinze dies o d'un a quatre anys.",
        "La llicència M permet pescar sense mort: tots els peixos es tornen a l'aigua.",
        "En una zona de pesca controlada cal, a més de la llicència, un permís personal i intransferible.",
        "Als refugis de pesca no es pot pescar: serveixen per protegir les espècies que hi viuen.",
        "Les aigües es classifiquen en salmònids d'alta muntanya, de baixa muntanya i ciprínids. Cada tipus té els seus períodes.",
        "A les zones de pesca controlada de ciprínids es pot pescar tot l'any, però les espècies autòctones s'han d'alliberar sempre.",
        "Als trams intensius es fan alliberaments periòdics de truita comuna. Generalment s'hi pot pescar tot l'any.",
        "Les reserves genètiques protegeixen la truita comuna autòctona de la vessant mediterrània.",
        "Cada any la Generalitat publica una resolució amb els períodes, trams, arts, esquers i hams permesos.",
        "La pesca als rius i pantans es regula per una llei d'ordenació sostenible de la pesca en aigües continentals.",
        "A les aigües privades cal l'autorització del titular, a més de la llicència.",
        "Està prohibit tornar a l'aigua els exemplars d'espècies invasores del catàleg estatal.",
        "No es poden tenir ni transportar vius exemplars d'espècies invasores. Tampoc se'n pot introduir cap al medi natural.",
        "Només una petita part dels trams pescables de Catalunya permet matar el peix. La resta és pesca sense mort.",
        "La Generalitat cobra menys pel permís sense mort que pel permís amb mort, per fomentar la pesca responsable.",
        "Al pantà de Sau s'ha retirat peix invasor: es calculaven unes seixanta tones i s'hi van detectar nou espècies invasores.",
        "Els peixos de Sau es van retirar perquè, si morien, es podririen i contaminarien l'aigua que es traspassava a Susqueda.",
        "A Sau conviuen invasors com el silur, la sandra, el rutil i l'alburn amb unes poques truites i barbs autòctons."
    };

    static bool[] trobades = new bool[Textos.Length];

    static NotesPesca()
    {
        for (int i = 0; i < Textos.Length; i++)
        {
            if (Textos[i].Length > MaxCaracters)
            {
                Debug.LogWarning("NotesPesca: la nota " + (i + 1) + " passa de " + MaxCaracters + " caràcters.");
            }
        }
    }

    public static int Quantes
    {
        get
        {
            int n = 0;
            for (int i = 0; i < trobades.Length; i++) { if (trobades[i]) { n++; } }
            return n;
        }
    }

    public static bool Completa { get { return Quantes >= Textos.Length; } }

    public static bool Trobada(int i) { return trobades[i]; }

    public static int Desbloqueja()
    {
        List<int> lliures = new List<int>();
        for (int i = 0; i < trobades.Length; i++) { if (!trobades[i]) { lliures.Add(i); } }
        if (lliures.Count == 0) { return -1; }

        int triada = lliures[Random.Range(0, lliures.Count)];
        trobades[triada] = true;
        return triada;
    }

    public static void Esborra() { trobades = new bool[Textos.Length]; }
}