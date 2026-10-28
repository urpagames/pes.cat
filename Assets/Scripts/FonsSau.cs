using UnityEngine;

public class FonsSau : MonoBehaviour
{
    [Header("Superfície")]
    public Transform muntanya;
    public Transform nuvols;
    public float parallaxMuntanya = 0.3f;
    public float parallaxNuvols = 0.15f;
    public float balancNuvols = 0.25f;
    public float velocitatNuvols = 0.1f;
    public SpriteRenderer[] onesSuperficie;

    [Header("Sota l'aigua")]
    public SpriteRenderer[] aiguaSubmarina;
    public Sprite onaTenue;
    public int quantes = 10;
    public float primeraY = -7f;
    public float separacio = 7f;
    public float alfaMax = 0.14f;
    public float alfaMin = 0.04f;
    public int ordre = -78;

    [Header("Transició")]
    public float camYInicial = 1.25f;
    public float inici = 0.3f;
    public float fi = 2f;

    Transform cam;
    Vector3 posMuntanya;
    Vector3 posNuvols;
    SpriteRenderer[] tenues;
    float[] alfaTenue;

    void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("FonsSau: no trobo la càmera. Posa-li el tag MainCamera.");
            enabled = false;
            return;
        }
        cam = Camera.main.transform;

        if (muntanya != null) { posMuntanya = muntanya.localPosition; }
        if (nuvols != null) { posNuvols = nuvols.localPosition; }

        if (onesSuperficie == null || onesSuperficie.Length == 0)
        {
            Debug.LogWarning("FonsSau: falta assignar 'Ones Superficie'.");
        }
        if (aiguaSubmarina == null || aiguaSubmarina.Length == 0)
        {
            Debug.LogWarning("FonsSau: falta assignar 'Aigua Submarina' (Degrade i Textura).");
        }
        else if (onaTenue == null)
        {
            Debug.LogWarning("FonsSau: falta assignar 'Ona Tenue'.");
        }
        else
        {
            CreaOnesTenues();
        }
    }

    void CreaOnesTenues()
    {
        tenues = new SpriteRenderer[quantes];
        alfaTenue = new float[quantes];
        Material material = aiguaSubmarina[0].sharedMaterial;

        for (int i = 0; i < quantes; i++)
        {
            GameObject go = new GameObject("OnaTenue_" + i);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0f, primeraY - i * separacio, 0f);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = onaTenue;
            sr.sharedMaterial = material;
            sr.flipY = (i % 2 == 1);
            sr.sortingOrder = ordre;
            sr.enabled = false;
            tenues[i] = sr;

            float p = (quantes > 1) ? i / (float)(quantes - 1) : 0f;
            alfaTenue[i] = Mathf.Lerp(alfaMax, alfaMin, p);
        }
    }

    static void PosaAlfa(SpriteRenderer sr, float alfa)
    {
        if (sr == null) { return; }
        Color c = sr.color;
        c.a = alfa;
        sr.color = c;
        sr.enabled = alfa > 0.005f;
    }

    static void PosaAlfaLlista(SpriteRenderer[] llista, float alfa)
    {
        if (llista == null) { return; }
        foreach (SpriteRenderer sr in llista) { PosaAlfa(sr, alfa); }
    }

    void LateUpdate()
    {
        if (cam == null) { return; }

        float despl = cam.position.y - camYInicial;
        float baixada = Mathf.Max(0f, -despl);
        float t = Mathf.InverseLerp(inici, fi, baixada);

        if (muntanya != null)
        {
            muntanya.localPosition = posMuntanya + new Vector3(0f, despl * parallaxMuntanya, 0f);
        }
        if (nuvols != null)
        {
            float x = Mathf.Sin(Time.time * velocitatNuvols) * balancNuvols;
            nuvols.localPosition = posNuvols + new Vector3(x, despl * parallaxNuvols, 0f);
        }

        PosaAlfaLlista(onesSuperficie, 1f - t);
        PosaAlfaLlista(aiguaSubmarina, 1f);

        if (tenues != null)
        {
            for (int i = 0; i < tenues.Length; i++) { PosaAlfa(tenues[i], alfaTenue[i] * t); }
        }
    }
}