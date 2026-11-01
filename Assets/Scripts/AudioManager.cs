using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia;

    [Header("Efectes")]
    public AudioClip click;
    public AudioClip splash;
    public AudioClip canyaBaixa;
    public AudioClip canyaPuja;
    public AudioClip resultatBo;
    public AudioClip resultatDolent;
    public AudioClip riureCrist;

    [Header("Música")]
    public AudioClip musicaMenu;
    public AudioClip musicaJoc;

    public static float volumEfectes = 1f;
    public static float volumMusica = 0.6f;

    AudioSource fontEfectes;
    AudioSource fontMusica;

    void Awake()
    {
        if (Instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);

        fontEfectes = gameObject.AddComponent<AudioSource>();
        fontEfectes.playOnAwake = false;

        fontMusica = gameObject.AddComponent<AudioSource>();
        fontMusica.playOnAwake = false;
        fontMusica.loop = true;
        fontMusica.volume = volumMusica;

        SceneManager.sceneLoaded += AlCarregarEscena;
    }

    void AlCarregarEscena(Scene escena, LoadSceneMode mode)
    {
        AudioClip pista = (escena.name == "Game") ? musicaJoc : musicaMenu;
        if (pista != null && (fontMusica.clip != pista || !fontMusica.isPlaying))
        {
            fontMusica.clip = pista;
            fontMusica.Play();
        }

        Button[] botons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < botons.Length; i++)
        {
            botons[i].onClick.AddListener(ClickBoto);
        }
    }

    void ClickBoto() { Reprodueix(click); }

    void Reprodueix(AudioClip clip)
    {
        if (clip == null) { return; }
        fontEfectes.PlayOneShot(clip, volumEfectes);
    }
    public static void Click() { if (Instancia != null) Instancia.Reprodueix(Instancia.click); }
    public static void Splash() { if (Instancia != null) Instancia.Reprodueix(Instancia.splash); }
    public static void CanyaBaixa() { if (Instancia != null) Instancia.Reprodueix(Instancia.canyaBaixa); }
    public static void CanyaPuja() { if (Instancia != null) Instancia.Reprodueix(Instancia.canyaPuja); }
    public static void ResultatBo() { if (Instancia != null) Instancia.Reprodueix(Instancia.resultatBo); }
    public static void ResultatDolent() { if (Instancia != null) Instancia.Reprodueix(Instancia.resultatDolent); }
    public static void RiureCrist() { if (Instancia != null) Instancia.Reprodueix(Instancia.riureCrist); }

    public static void PosaVolumEfectes(float v) { volumEfectes = v; }

    public static void PosaVolumMusica(float v)
    {
        volumMusica = v;
        if (Instancia != null) { Instancia.fontMusica.volume = v; }
    }
}