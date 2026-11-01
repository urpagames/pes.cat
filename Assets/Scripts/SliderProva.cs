using UnityEngine;
using UnityEngine.EventSystems;

public class SliderProva : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData dades)
    {
        AudioManager.Click();
    }
}