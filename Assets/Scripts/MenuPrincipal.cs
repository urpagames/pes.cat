using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Pescar()
    {
        SceneManager.LoadScene("Game");
    }

    public void ObreEnciclopedia()
    {
        Debug.Log("Has obert la enciclopedia.");
    }

    public void ObreAjustaments()
    {
        Debug.Log("Has obert els ajustaments.");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
