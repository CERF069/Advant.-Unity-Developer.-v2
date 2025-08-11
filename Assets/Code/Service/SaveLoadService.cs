using Code.Components.Business;
using Code.Config.Business;
using UnityEngine;

namespace Code.Service
{
    /// <summary>
    /// Interface for saving data on application exit.
    /// </summary>
    public interface ISaveOnExit
    {
        void SaveOnExit();
    }

    /// <summary>
    /// Service responsible for saving and loading player balance and business data.
    /// </summary>
    public class SaveLoadService
    {
        private const string BalanceKey = "player_balance";

        #region Balance
        /// <summary>
        /// Loads the player's balance from PlayerPrefs.
        /// </summary>
        public float LoadBalance()
        {
            return PlayerPrefs.GetFloat(BalanceKey, 0);
        }

        /// <summary>
        /// Saves the player's balance to PlayerPrefs.
        /// </summary>
        public void SaveBalance(float value)
        {
            PlayerPrefs.SetFloat(BalanceKey, value);
            PlayerPrefs.Save();
        }
        #endregion

        #region Business
        private string GetBusinessKey(string businessName) => $"business_{businessName}";

        /// <summary>
        /// Saves business data to PlayerPrefs.
        /// </summary>
        public void SaveBusiness(BusinessComponent business, BusinessProgressComponent progress)
        {
            string key = GetBusinessKey(business.Name);

            // Format: Level|HasUpgrade1|HasUpgrade2|Timer|Progress
            string data = string.Join("|",
                business.Level,
                business.HasUpgrade1 ? 1 : 0,
                business.HasUpgrade2 ? 1 : 0,
                progress.Timer.ToString(System.Globalization.CultureInfo.InvariantCulture),
                progress.Progress.ToString(System.Globalization.CultureInfo.InvariantCulture)
            );

            PlayerPrefs.SetString(key, data);
        }

        /// <summary>
        /// Loads business data from PlayerPrefs.
        /// </summary>
        /// <returns>True if business data exists and was loaded successfully, otherwise false.</returns>
        public bool LoadBusiness(string businessName, out BusinessComponent business, out BusinessProgressComponent progress, BusinessConfig config)
        {
            string key = GetBusinessKey(businessName);

            // Default initialization
            business = new BusinessComponent
            {
                Config = config,
                Level = 0,
                HasUpgrade1 = false,
                HasUpgrade2 = false
            };

            progress = new BusinessProgressComponent
            {
                Timer = 0f,
                Progress = 0f
            };

            if (!PlayerPrefs.HasKey(key))
                return false;

            string data = PlayerPrefs.GetString(key);
            var parts = data.Split('|');
            if (parts.Length != 5)
                return false;

            // Parse saved data
            if (int.TryParse(parts[0], out int level))
                business.Level = level;

            business.HasUpgrade1 = parts[1] == "1";
            business.HasUpgrade2 = parts[2] == "1";

            if (float.TryParse(parts[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float timer))
                progress.Timer = timer;

            if (float.TryParse(parts[4], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float prog))
                progress.Progress = prog;

            Debug.Log($"[SaveLoadService] Business '{businessName}' loaded | Level: {business.Level}, Upgrade1: {business.HasUpgrade1}, Upgrade2: {business.HasUpgrade2}, Timer: {progress.Timer:F2}, Progress: {progress.Progress:F2}");

            return true;
        }
        #endregion
    }
}
