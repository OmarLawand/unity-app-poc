using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{
    LevelController loadingCanvas;
    AudioSource source;
    //GameObject childText;

    public void setLevel(string sceneName)
    {
        loadingCanvas = GameObject.Find("LoadingCanvas").GetComponent<LevelController>();
        loadingCanvas.SetScene(sceneName);
    }

    public void playSound()
    {
        if(GetComponent<AudioSource>() != null)
            source = GetComponent<AudioSource>();

        source.Play();
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    childText.SetActive(true);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    childText.SetActive(false);
    //}
}
