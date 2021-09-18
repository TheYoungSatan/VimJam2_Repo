using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject _previewMiniGame;



    public void OnPointerEnter(PointerEventData eventData)
    {
        // enable preview of hovered button
        if (_previewMiniGame != null)
        {
            _previewMiniGame.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // disable preview of hovered button
        if (_previewMiniGame != null)
        {
            _previewMiniGame.SetActive(false);
        }          
    }


    public void OnButtonClickedPart1DisableCurrent(GameObject panelToDisable)
    {
        if (_previewMiniGame != null) // cn't forget to disable this !!
        {
            _previewMiniGame.SetActive(false);
        }

        panelToDisable.SetActive(false);
    }
    public void OnButtonClickedPart2EnableNew(GameObject panelToEnable)
    {
        panelToEnable.SetActive(true);
    }

    public void LoadMiniGame(GameObject Minigame)
    {
        
    }

}
