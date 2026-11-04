using UnityEngine;

public static class Curiositats
{
    public class Dades
    {
        public string nom;
        public string descripcio;
        public int punts;
        public string fitxer;
        Sprite sprite;

        public Dades(string nom, string descripcio, int punts, string fitxer)
        {
            this.nom = nom;
            this.descripcio = descripcio;
            this.punts = punts;
            this.fitxer = fitxer;
        }

        public Sprite Imatge
        {
            get
            {
                if (sprite == null)
                {
                    sprite = Resources.Load<Sprite>("Curiositats/" + fitxer);
                    if (sprite == null) { Debug.LogWarning("Curiositats: falta l'sprite " + fitxer); }
                }
                return sprite;
            }
        }
    }

    public static readonly Dades[] Llista =
    {
        new Dades("DRAC DOLENT",
            "Segons diuen, aquest famós drac fa por a tota la gent, i per això sempre se li demana que se'n vagi dels llocs.",
            2000, "curiositat_4"),
        new Dades("ESCUT DE GUIFRÉ EL PILÓS",
            "Això sí que son barres.",
            4000, "curiositat_3"),
        new Dades("BOMBES CATALANES",
            "Les dues bombes catalanes més famoses: la bomba Orsini, de l'atemptat al Liceu i la bomba calòrica, el panellet.",
            6000, "curiositat_6"),
        new Dades("MIRALL TRENCAT",
            "Ostres com el del llibre, què bo, tu.",
            8000, "curiositat_7"),
        new Dades("ROSA DE SANT JORDI",
            "Que l'amor s'hi posa...",
            10000, "curiositat_5"),
        new Dades("ESCAFANDRA ABISSAL",
            "Cansat de baixar vint-i-quatre metres? Endinsa't en aigües profundes al videojoc ABISMO, amb demo ja disponible a Steam.",
            13000, "curiositat_2"),
        new Dades("CAP DE BRAU",
            "Va pertànyer a un dels personatges del veritable premi Ludi de fa dos anys... Lo Foraster. Disponible a Itch.io",
            15000, "curiositat_8"),
        new Dades("HAS PESCAT AQUEST HOME?",
            "Cada dia, a pantans d'arreu del mon, pesquen aquest home. Per saber-ne més, juga EVERDREAM a Steam.",
            20000, "curiositat_1"),
    };

    public static int entregades = 0;

    public static Dades Pendent(int punts)
    {
        if (entregades < Llista.Length && punts >= Llista[entregades].punts)
        {
            return Llista[entregades];
        }
        return null;
    }

    public static void Entrega() { entregades++; }

    public static bool Tinguda(int i) { return i < entregades; }

    public static void Sincronitza(int punts)
    {
        while (entregades < Llista.Length && punts >= Llista[entregades].punts) { entregades++; }
    }

    public static void Esborra() { entregades = 0; }
}