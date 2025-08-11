using TMPro;
using UnityEngine;

namespace Code.UI
{
    /// <summary>
    /// Holds references to core UI elements in the game.
    /// </summary>
    public sealed class UiRegistry : MonoBehaviour
    {
        [Header("Player UI Elements")]
        [SerializeField] 
        private TMP_Text _balanceText;
        public TMP_Text BalanceText => _balanceText;

        [Header("Business UI Elements")]
        [SerializeField] 
        private UiBusiness[] _businesses;
        public UiBusiness[] Businesses => _businesses;
    }
}