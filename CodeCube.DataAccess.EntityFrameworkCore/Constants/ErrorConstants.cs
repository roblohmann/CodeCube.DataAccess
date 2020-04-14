namespace CodeCube.DataAccess.EntityFrameworkCore.Constants
{
    public static class ErrorConstants
    {
        public const string MissingConnectionstring = "There is not connectionstring configured. Please add a connectionstring with the name 'DatabaseConnection'.";
        public const string ModelbuilderRequired = "ModelBuilder is required!";
        public const string AppSettingsBaseDirectoryNotFound = "Basedirectory for 'appsettings.json' could not be determined!";
        public const string AppSettingsBaseDirectoryNotFoundRecursively = "Basedirectory for 'appsettings.json' could not be determined! Paths have been recusively checked!";
    }
}
