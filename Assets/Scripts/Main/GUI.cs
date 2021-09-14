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
    [SerializeField] private VisualGUI[] _visualGUI;
    private PlayerInfo _playerinfo;

    private void Awake()
    {
        instance = this;
        _playerinfo = FindObjectOfType<PlayerInfo>();
        _playerinfo.OnUpdateValues += UpdateGUI;
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
}