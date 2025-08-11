using UnityEngine;

namespace Code.Config.Business
{
    [System.Serializable]
    public class BusinessUpgrade
    {
        [SerializeField, Tooltip("Название улучшения")]
        private string nameUpgrade;
        
        [SerializeField, Tooltip("Цена улучшения в игровых деньгах")]
        private int price;

        [SerializeField, Tooltip("Множитель дохода: 0.5 = +50%, 1.0 = +100%")]
        private float incomeMultiplier;

        public int Price => price;
        public float IncomeMultiplier => incomeMultiplier;
        
        public string Name => nameUpgrade;
    }

    [CreateAssetMenu(fileName = "BusinessConfig", menuName = "Configs/Business Config")]
    public class BusinessConfig : ScriptableObject
    {
        [Header("Основные параметры")]
      
        [SerializeField, Tooltip("Название бизнеса")]
        private string businessName;
        
        [SerializeField, Tooltip("Стартовый уровень бизнеса")]
        private int startLevel = 1;

        public int StartLevel => startLevel;

        [SerializeField, Tooltip("Задержка получения дохода в секундах")]
        private float incomeDelay;

        [SerializeField, Tooltip("Базовая стоимость бизнеса")]
        private int baseCost;

        [SerializeField, Tooltip("Базовый доход за цикл")]
        private int baseIncome;

        [Header("Улучшение 1")]
        [SerializeField] private BusinessUpgrade upgrade1;

        [Header("Улучшение 2")]
        [SerializeField] private BusinessUpgrade upgrade2;

        public string BusinessName => businessName;
        public float IncomeDelay => incomeDelay;
        public int BaseCost => baseCost;
        public int BaseIncome => baseIncome;
        public BusinessUpgrade Upgrade1 => upgrade1;
        public BusinessUpgrade Upgrade2 => upgrade2;
    }
}