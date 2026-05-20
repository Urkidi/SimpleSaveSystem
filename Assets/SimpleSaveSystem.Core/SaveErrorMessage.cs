namespace SimpleSaveSystem.Core
{
    public static class SaveErrorMessage
    {
        public static readonly string HashDoesntMatch = "Save Error: Hash doesn't match with stored slot hash";
        public static readonly string UnableToReadPath = "Save Error: Unable to read save on path {0}";
        public static readonly string UnableToFindSaveWithId = "Save Error: Unable to save. Couldn't find save with id {0}";
        public static readonly string SaveIdAlreadyExists = "Save Error: Unable to create new save. Save with id {0} already exists";
        public static readonly string IncrementalIdNamingIssue = "Save Error: Unable to create save. Save with id {0} already exists. Created Id was incremental, yet the Id already exists";
        public static readonly string LoadCreateCannotLoad = "Save Error: Id: {0} exists, and was unable to Load";
        public static readonly string GenericCannotLoadSave = "Save Error: Unable to load savegame of id: {0}";
    }
}