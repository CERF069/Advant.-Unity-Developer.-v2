namespace Code.Components.Business
{
    /// <summary>
    /// Holds progress data for a business, including the elapsed timer and normalized progress value.
    /// </summary>
    public struct BusinessProgressComponent
    {
        /// <summary>
        /// Elapsed time since the current business cycle started.
        /// </summary>
        public float Timer;

        /// <summary>
        /// Normalized progress value (0 to 1) representing the completion of the current business cycle.
        /// </summary>
        public float Progress;
    }
}