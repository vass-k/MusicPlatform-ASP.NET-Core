namespace MusicPlatform.GCommon
{
    public static class ExceptionMessages
    {
        public const string SoftDeleteOnNonSoftDeletableEntity =
            "Soft Delete can't be performed on an Entity which does not support it!";

        public const string InterfaceNotFoundMessage =
            "The {0} could not be added to the Service Collection, because no interface matching the convention could be found! Convention for Interface naming: I{0}.";
    }
}
