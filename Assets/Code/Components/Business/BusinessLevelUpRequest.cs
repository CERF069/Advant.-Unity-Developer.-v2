namespace Code.Components.Business
{
    /// <summary>
    /// Represents a request to level up a specific business identified by its name.
    /// </summary>
    public struct BusinessLevelUpRequest
    {
        /// <summary>
        /// The unique name identifier of the business to level up.
        /// </summary>
        public string BusinessName;
    }
}