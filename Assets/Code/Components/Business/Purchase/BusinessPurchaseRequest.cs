namespace Code.Components.Business
{
    /// <summary>
    /// Request component to initiate a business purchase or level-up.
    /// </summary>
    struct BusinessPurchaseRequest
    {
        /// <summary>
        /// The name identifier of the business being purchased or leveled up.
        /// </summary>
        public string BusinessName;
    }
}