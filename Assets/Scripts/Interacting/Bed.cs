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
            return $"Bed \nAwake: {_playerinfo.AwakeTime}:00 \nTime: {GameInfo.CurrentTime}:00";
        }

        public bool Interactable()
        {
            return GameInfo.CurrentTime >= 20 || _playerinfo.AwakeTime > 12;
        }

        public void OnInteract()
        {
            if(Interactable())
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
