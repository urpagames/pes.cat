using UnityEngine;

[CreateAssetMenu(fileName = "NovaEspecie", menuName = "Pescat/Especie")]
public class EspecieData : ScriptableObject
{
    public string id;
    public string nom;
    public string nomCientific;
    public Sprite imatge;
    [TextArea] public string habitat;
    [TextArea] public string descripcio;

    public float midaMin = 20f;
    public float midaMax = 45f;
    public float pesMin = 0.2f;
    public float pesMax = 1.5f;

    public float profunditatMin = 1f;
    public float profunditatMax = 8f;

    public float probabilitat = 1f;
    public float velocitat = 2f;
}