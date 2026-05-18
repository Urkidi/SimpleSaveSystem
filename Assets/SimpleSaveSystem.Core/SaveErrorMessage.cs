namespace SimpleSaveSystem.Core
{
    public static class SaveErrorMessage
    {
        public static readonly string HashDoesntMatch = "Save Error: Hash doesn't match with stored slot hash";
        public static readonly string UnableToReadPath = "Save Error: Unable to read save on path {0}";
        public static readonly string UnableToFindSaveWithId = "Save Error: Unable so save. Couldn't find save with id {0}";
    }
}