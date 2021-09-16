using UnityEngine;

namespace Interacting
{
    public class Bed : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _wakeupTime = 10;
        private PlayerInfo _playerinfo;

        private void Awake()
        {
            _playerinfo = FindObjectOfType<PlayerInfo>();
        }

        public bool HasInfoPanel()
        {
            return true;
        }

        public string InfoText()
        {
            return $"Bed \nAwake: {_playerinfo.AwakeTime} \nTime: {GameInfo.CurrentTime}";
        }

        public bool Interactable()
        {
            return GameInfo.CurrentTime >= 20;
        }

        public void OnInteract()
        {
            if(GameInfo.CurrentTime >= 20)
            {
                GameInfo.SetTime(_wakeupTime);
            }
        }

        public Transform Position()
        {
            return transform;
        }
    }
}
