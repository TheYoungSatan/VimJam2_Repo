using UnityEngine;

namespace Wwise
{
    public class KeepAKBankActive : MonoBehaviour
    {
        AkBank _bank;
        private void Awake()
        {
            _bank = GetComponent<AkBank>();
        }

        private void Update()
        {
            if (!_bank.enabled)
                _bank.enabled = true;
        }
    }
}
