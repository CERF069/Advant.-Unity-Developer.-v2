using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Code.UI
{
    public class UiBusiness : MonoBehaviour
    {
        [Header("Business Identification")]
        [SerializeField] private string _businessNameID;
        public string BusinessNameID => _businessNameID;

        [Header("Main UI Elements")]
        [SerializeField] private TMP_Text _businessNameText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _incomeText;

        [Header("Level Upgrade")]
        [SerializeField] private Button _levelUpButton;
        public Button LevelUpButton => _levelUpButton;

        [SerializeField] private TMP_Text _levelUpButtonText;

        [Header("Upgrades")]
        [SerializeField] private Button _upgradeButton1;
        [SerializeField] private TMP_Text _upgradeButton1Text;

        [SerializeField] private Button _upgradeButton2;
        [SerializeField] private TMP_Text _upgradeButton2Text;

        public Button UpgradeButton1 => _upgradeButton1;
        public Button UpgradeButton2 => _upgradeButton2;

        [Header("Progress")]
        [SerializeField] private Slider _progressSlider;

        #region Public Methods

        /// <summary>
        /// Updates the progress slider value.
        /// </summary>
        public void SetProgress(float progress)
        {
            if (_progressSlider != null)
                _progressSlider.value = progress;
        }

        /// <summary>
        /// Updates the level up button cost text.
        /// </summary>
        public void SetLevelUpButtonText(int cost)
        {
            if (_levelUpButtonText != null)
                _levelUpButtonText.text = $"LVL UP\nPrice: {cost}$";
        }

        /// <summary>
        /// Updates the text for a specific upgrade button.
        /// </summary>
        public void SetUpgradeButtonText(int upgradeIndex, string upgradeName, float incomeMultiplier, int cost, bool isBought)
        {
            TMP_Text targetText = GetUpgradeTextByIndex(upgradeIndex);
            if (targetText == null) return;

            string boughtLabel = isBought ? "\nPurchased" : string.Empty;
            int percentBonus = Mathf.RoundToInt(incomeMultiplier * 100);

            targetText.text = $"{upgradeName}\nIncome: +{percentBonus}%\nPrice: {cost}${boughtLabel}";
        }

        /// <summary>
        /// Updates all main business data text fields.
        /// </summary>
        public void SetData(string name, string level, string income, string upgradeCost)
        {
            SetText(_businessNameText, name);
            SetText(_levelText, level);
            SetText(_incomeText, income);
        }

        #endregion

        #region Private Helpers

        private TMP_Text GetUpgradeTextByIndex(int index)
        {
            switch (index)
            {
                case 1: return _upgradeButton1Text;
                case 2: return _upgradeButton2Text;
                default:
                    Debug.LogWarning($"Invalid upgrade index: {index}");
                    return null;
            }
        }

        private void SetText(TMP_Text target, string value)
        {
            if (target != null)
                target.text = value;
        }

        #endregion
    }
}
