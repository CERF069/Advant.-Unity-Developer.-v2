using Code.Config.Business;

namespace Code.Components.Business
{
    public struct BusinessComponent
    {
        public BusinessConfig Config;

        public int Level;
        
        public bool HasUpgrade1;
        public bool HasUpgrade2;
        
        public float ProgressTimer;
        
        public bool IsDirty;
        
        public float ProgressNormalized => Config.IncomeDelay > 0 ? ProgressTimer / Config.IncomeDelay : 1f;
        public float CalculateIncome()
        {
            float multiplier = 1f;
            if (HasUpgrade1)
                multiplier += Config.Upgrade1.IncomeMultiplier;
            if (HasUpgrade2)
                multiplier += Config.Upgrade2.IncomeMultiplier;
            return Level * Config.BaseIncome * multiplier;
        }

        public int CalculateNextLevelCost()
        {
            return (Level + 1) * Config.BaseCost;
        }

        public string Name => Config.BusinessName;
        public float Delay => Config.IncomeDelay;
    }
}