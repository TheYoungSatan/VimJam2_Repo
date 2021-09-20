using Interacting;
using Sound;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    private static GUI instance;
    private enum Stat { Energy, Hunger, Thurst, Money}
    [Serializable]
    private struct VisualGUI
    {
        public Stat Stat;
        public Text Text;
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

        public void UpdateVisuals(int amount)
        {
            Visual current = Visuals.Aggregate((p, n) => amount <= n.Percentage ? p : n);

            if (!current.IsAnimation)
            {
                mainImage.sprite = current.Sprite[0];
                if(routine != null)
                    instance.StopCoroutine(routine);
            }
            else
                routine = instance.StartCoroutine(Animate(current));

            if (Text)
                Text.text = amount.ToString();
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
    [SerializeField] private InfoPanel _infoPanel;
    [SerializeField] private GameOverPanel gameOverPanel;

    private PlayerInfo playerinfo;
    private PlayerController _player = null;

    private void Start()
    {
        instance = this;
        playerinfo = FindObjectOfType<PlayerInfo>();
        playerinfo.OnUpdateValues += UpdateGUI;
        gameOverPanel.SetActive(false);
        UpdateGUI();
    }

    public static void SetActive(bool val) => instance.gameObject.SetActive(val);

    private void Update()
    {
        UpdateGUI();
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerController>();
            SetInfoObjects(null, null);
            //if (_guiPanel.activeSelf)
            //    _guiPanel.SetActive(false);
        }
        else
        {
            if(!_guiPanel.activeSelf)
                _guiPanel.SetActive(true);

            var playerVals = _player?.CheckForInteractables();
            SetInfoObjects(playerVals.Value.transform, playerVals.Value.interactable);
        }
    }

    public void UpdateGUI()
    {
        foreach (var visual in _visualGUI)
        {
            switch (visual.Stat)
            {
                case Stat.Energy:
                    visual.UpdateVisuals(playerinfo.AwakeTime);
                    break;
                case Stat.Hunger:
                    visual.UpdateVisuals(playerinfo.HungerPercentage);
                    break;
                case Stat.Thurst:
                    visual.UpdateVisuals(playerinfo.ThurstPercentage);
                    break;
                case Stat.Money:
                    visual.UpdateVisuals(GameInfo.PouchMoney);
                    break;
            }
        }
    }

    public void SetInfoObjects(Transform trans, IInteractable interactable)
    {
        if(interactable == null || !interactable.Interactable())
        {
            _interactButton.gameObject.SetActive(false);
            ShowInfoPanel(interactable);
            return;
        }

        ShowInfoPanel(interactable);

        Vector3 transpos = trans.position;
        transpos.y += .5f;
        transpos.x += .125f;
        Vector3 pos = Camera.main.WorldToScreenPoint(transpos);
        pos.z = 0;
        _interactButton.position = pos;
        _interactButton.gameObject.SetActive(true);
    }

    private void ShowInfoPanel(IInteractable interactable)
    {
        if (interactable == null)
        {
            _infoPanel.SetActive(false);
            return;
        }

        if (interactable.HasInfoPanel())
        {
            Vector3 panelPos = interactable.Position().position;
            panelPos.y += .5f;
            panelPos.x -= .25f;
            panelPos = Camera.main.WorldToScreenPoint(panelPos);
            panelPos.z = 0;
            _infoPanel.SetPosition(panelPos);
            _infoPanel.SetText(interactable.InfoText());
            _infoPanel.SetActive(true);
        }
        else
            _infoPanel.SetActive(false);
    }

    public static void OnGameOver()
    {
        instance.gameOverPanel.SetText(instance.playerinfo);
        instance.gameOverPanel.SetActive();
    }

    public void LoadMainMenu(string scenename)
    {
        StartCoroutine(LoadMain(scenename));
    }

    IEnumerator LoadMain(string sceneToLoad)
    {
        SceneTransition transition = FindObjectOfType<SceneTransition>();
        transition.FadeOut();
        yield return new WaitForSeconds(.5f);
        AudioHub.SetState(AudioHub.PlayerLife, "None");
        AudioHub.SetState(AudioHub.BackgroundMusic, "MainMenu");
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(sceneToLoad);
    }
}