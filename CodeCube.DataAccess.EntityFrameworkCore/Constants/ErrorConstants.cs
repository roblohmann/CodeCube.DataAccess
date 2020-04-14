namespace CodeCube.DataAccess.EntityFrameworkCore.Constants
{
    public static class ErrorConstants
    {
        public const string MissingConnectionstring = "No connectionstring found!. Please add a connectionstring with the name 'DatabaseConnection' to your AppSettings.json or implement the method 'GetConnectionstring'";
        public const string ConnectionstringParameterRequired = "The parameter 'connectionString' is required!";
        public const string ModelbuilderRequired = "ModelBuilder is required!";
        public const string AppSettingsBaseDirectoryNotFound = "Basedirectory for 'appsettings.json' could not be determined!";
        public const string AppSettingsBaseDirectoryNotFoundRecursively = "Basedirectory for 'appsettings.json' could not be determined! Paths have been recusively checked!";
    }
}
