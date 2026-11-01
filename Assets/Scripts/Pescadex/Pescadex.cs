using System.Collections.Generic;

public static class Pescadex
{
    public class Registre
    {
        public float recordMida;
        public float recordPes;
        public int captures;
    }

    static Dictionary<string, Registre> registres = new Dictionary<string, Registre>();

    public static bool Descoberta(EspecieData e)
    {
        return registres.ContainsKey(e.name);
    }

    public static Registre Obte(EspecieData e)
    {
        Registre r;
        registres.TryGetValue(e.name, out r);
        return r;
    }

    public static void Registra(EspecieData e, float mida, float pes, out bool nova, out bool record)
    {
        Registre r;
        nova = !registres.TryGetValue(e.name, out r);
        record = false;

        if (nova)
        {
            r = new Registre();
            r.recordMida = mida;
            r.recordPes = pes;
            r.captures = 1;
            registres[e.name] = r;
            return;
        }

        r.captures++;
        if (mida > r.recordMida) { r.recordMida = mida; record = true; }
        if (pes > r.recordPes) { r.recordPes = pes; record = true; }
    }

    public static void Esborra()
    {
        registres.Clear();
    }
}