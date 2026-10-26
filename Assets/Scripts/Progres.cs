public static class Progres
{
    public static int[] llindars = { 0, 2500, 6000 };

    public static int Punts = 0;

    public static bool EsquerDesbloquejat(int esquer)
    {
        return Punts >= llindars[esquer - 1];
    }

    public static void Esborra()
    {
        Punts = 0;
    }
}