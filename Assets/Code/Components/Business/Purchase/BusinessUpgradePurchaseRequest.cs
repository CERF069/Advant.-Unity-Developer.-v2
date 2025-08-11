namespace Code.Components.Business
{
    /// <summary>
    /// Request component to purchase a specific upgrade for a business.
    /// </summary>
    public struct BusinessUpgradePurchaseRequest
    {
        /// <summary>
        /// The name of the business for which the upgrade is requested.
        /// </summary>
        public string BusinessName;

        /// <summary>
        /// Index of the upgrade to purchase.
        /// 1 or 2, corresponding to Upgrade1 or Upgrade2 respectively.
        /// </summary>
        public int UpgradeIndex;
    }
}