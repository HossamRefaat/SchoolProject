namespace SchoolProject.Data.AppMetaData
{
    public static class Router
    {
        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";
        public const string SingleRoute = "/{id}";

        public static class StudentRouting
        {
            public const string Prefix = Rule + "Student";
            public const string GetStudentList = Prefix + "/List";
            public const string GetStudentPaginated = Prefix + "/Paginated";
            public const string GetStudentById = Prefix + SingleRoute;
            public const string Create = Prefix + "/Create";
            public const string Edit = Prefix + "/Edit";
            public const string Delete = Prefix + SingleRoute;
        }

        public static class DepartmentRouting
        {
            public const string Prefix = Rule + "Department";
            public const string GetById = Prefix + "/Id";
        }
    }
}
