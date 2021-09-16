using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    private static GUI instance;
    private enum Stat { Energy, Hunger, Thurst}
    [Serializable]
    private struct VisualGUI
    {
        public Stat Stat;
        public Image mainImage;

        [Serializable]
        public struct Visual
        {
            public Sprite[] Sprite;
            public bool IsAnimation => Sprite.Length > 1;
            public int Percentage;
        }

        public Visual[] Visuals;
        private Coroutine routine;

        public void UpdateVisuals(int percentage)
        {
            Visual current = Visuals.Aggregate((p, n) => percentage <= p.Percentage ? p : n);
            if (!current.IsAnimation)
            {
                mainImage.sprite = current.Sprite[0];
                if(routine != null)
                    instance.StopCoroutine(routine);
            }
            else
                routine = instance.StartCoroutine(Animate(current));
        }
        IEnumerator Animate(Visual v)
        {
            int current = 0;
            while (true)
            {
                yield return new WaitForSeconds(.1f);
                current ++;
                if (current == v.Sprite.Length)
                    current = 0;
                mainImage.sprite = v.Sprite[current];
            }
        }
    }

    [SerializeField] private GameObject _guiPanel;
    [SerializeField] private VisualGUI[] _visualGUI;
    [SerializeField] private RectTransform _interactButton;

    private PlayerInfo _playerinfo;
    private PlayerController _player = null;

    private void Start()
    {
        instance = this;
        _playerinfo = FindObjectOfType<PlayerInfo>();
        _playerinfo.OnUpdateValues += UpdateGUI;
        UpdateGUI();
    }

    private void Update()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerController>();
            if (_guiPanel.activeSelf)
                _guiPanel.SetActive(false);
        }
        else
        {
            if(!_guiPanel.activeSelf)
                _guiPanel.SetActive(true);
            SetInteractButton(_player?.CheckForInteractables());
        }
    }

    private void UpdateGUI()
    {
        foreach (var visual in _visualGUI)
        {
            switch (visual.Stat)
            {
                case Stat.Energy:
                    visual.UpdateVisuals(_playerinfo.AwakeTime);
                    break;
                case Stat.Hunger:
                    visual.UpdateVisuals(_playerinfo.HungerPercentage);
                    break;
                case Stat.Thurst:
                    visual.UpdateVisuals(_playerinfo.ThurstPercentage);
                    break;
            }
        }
    }

    public void SetInteractButton(Transform trans)
    {
        if(trans == null)
        {
            _interactButton.gameObject.SetActive(false);
            return;
        }

        Vector3 transpos = trans.position;
        transpos.y += .5f;
        transpos.x += .125f;
        Vector3 pos = Camera.main.WorldToScreenPoint(transpos);
        pos.z = 0;
        _interactButton.position = pos;
        _interactButton.gameObject.SetActive(true);
    }
}