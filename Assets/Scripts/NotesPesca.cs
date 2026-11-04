using System.Collections.Generic;
using UnityEngine;

public static class NotesPesca
{
    public static readonly string[] Textos =
    {
        "Per pescar a un riu o a un pantà cal una llicència de pesca recreativa. La mateixa llicència serveix per pescar al mar en superfície.",
        "Els menors de 18 anys i els majors de 65 no paguen la llicència. Els menors han de presentar l'autorització del pare, la mare o el tutor.",
        "La llicència pot ser d'un dia, de dos dies, de quinze dies o d'un a quatre anys.",
        "La llicència M permet pescar sense mort: tots els peixos es tornen a l'aigua.",
        "En una zona de pesca controlada cal, a més de la llicència, un permís personal i intransferible.",
        "Als refugis de pesca no es pot pescar, perquè protegeixen espècies protegides.",
        "Les aigües es classifiquen en salmònids d'alta muntanya, salmònids de baixa muntanya i ciprínids. Cada zona té els seus períodes de pesca, segons la biologia dels peixos.",
        "A les zones de pesca controlada de ciprínids es pot pescar tot l'any, però les espècies autòctones s'han d'alliberar sempre.",
        "Als trams intensius es fan alliberaments periòdics de truita comuna. Generalment s'hi pot pescar tot l'any.",
        "Les reserves genètiques protegeixen la truita comuna autòctona de la vessant mediterrània.",
        "Cada any la Generalitat publica una resolució que fixa els períodes, els trams, les arts, els esquers i els hams permesos.",
        "La pesca als rius i pantans es regula per la Llei 22 del 2009, d'ordenació sostenible de la pesca en aigües continentals.",
        "A les aigües privades cal l'autorització del titular, a més de la llicència.",
        "Està prohibit tornar a l'aigua els exemplars d'espècies invasores del catàleg estatal. Ho estableix el Reial decret 630 del 2013.",
        "No es poden tenir ni transportar vius exemplars d'espècies invasores. Tampoc se'n pot introduir cap al medi natural.",
        "A Catalunya hi ha uns 3.800 km pescables, però només 635 permeten matar el peix. Són dades del 2019.",
        "Des del 2012, la Generalitat cobra menys pel permís sense mort que pel permís amb mort, per fomentar la pesca responsable.",
        "El març del 2023 es va començar a retirar peix del pantà de Sau: es calculaven unes 60 tones. S'hi van detectar nou espècies invasores.",
        "Els peixos de Sau es van retirar perquè, si morien, podririen i contaminarien l'aigua que es traspassava a Susqueda.",
        "A Sau conviuen invasors com el silur, la sandra, el rutil i l'alburn amb unes poques truites i barbs autòctons."
    };

    static bool[] trobades = new bool[Textos.Length];

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

    // Retorna l'índex d'una nota nova, o -1 si ja les té totes
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