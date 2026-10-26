using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Globalization;

public class MenuPrincipal : MonoBehaviour
{
    public Button[] botonsEsquer;

    public GameObject panellEsquers;

    void Start()
    {
        panellEsquers.SetActive(false);
        ActualitzaEsquers();
    }

    public void Pescar()
    {
        panellEsquers.SetActive(true);
    }

    public void TancaEsquers()
    {
        panellEsquers.SetActive(false);
    }

    public void TriaEsquer(int numero)
    {
        Partida.esquerTriat = numero;
        SceneManager.LoadScene("Game");
    }

    public void ObreEnciclopedia()
    {
        Debug.Log("Enciclopèdia");
    }

    public void ObreAjustaments()
    {
        Debug.Log("Ajustaments");
    }

    void ActualitzaEsquers()
    {
        for (int i = 0; i < botonsEsquer.Length; i++)
        {
            bool obert = Progres.EsquerDesbloquejat(i + 1);
            botonsEsquer[i].interactable = obert;

            TextMeshProUGUI text = botonsEsquer[i].GetComponentInChildren<TextMeshProUGUI>();
            if (obert) { text.text = "ESQUER " + (i + 1) + "  ·  0-" + (8 * (i + 1)) + " m"; }
            else { text.text = "BLOQUEJAT  ·  " + Progres.llindars[i].ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.') + " pts"; }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { Progres.Esborra(); ActualitzaEsquers(); }
        if (Input.GetKeyDown(KeyCode.P)) { Progres.Punts += 5000; ActualitzaEsquers(); }
    }
}