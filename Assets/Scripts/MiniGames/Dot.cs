using Sound;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniGame
{
    public class Dot : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text _text;

        public event Action<Dot> OnClicked;
        [HideInInspector] public int Number;
        public Vector3 Position => transform.position;

        public void SetText(bool value = true) 
        {
            _text.text = Number.ToString();
            _text.enabled = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioHub.PlaySound(AudioHub.ConnectTheDots);
            OnClicked?.Invoke(this);
        }
    }
}
