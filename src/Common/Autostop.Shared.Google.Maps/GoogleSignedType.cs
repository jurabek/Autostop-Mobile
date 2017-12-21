namespace Google.Maps
{
    /// <summary>
    ///     Describes the way in which a Uri will be signed
    /// </summary>
    public enum GoogleSignedType
    {
        /// <summary>
        ///     Indicates that the Uri will be signed using an API Key which would allow per key quotas.
        /// </summary>
        ApiKey,

        /// <summary>
        ///     Indicates that the Uri will be signed using the business client id and allows the use of the business services.
        /// </summary>
        Business
    }
}