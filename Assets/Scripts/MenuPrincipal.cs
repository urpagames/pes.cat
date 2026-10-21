using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject panellEsquers;

    void Start()
    {
        panellEsquers.SetActive(false);
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
}