namespace AppleStore.DataQuery
{
    public class QueryData
    {
        public bool loginAdmin(string email, string password)
        {
            if (email.Equals("admin@a.com") && password.Equals("applestore@123")) return true;
            return false;
        }

        public bool loginAdmin(string password)
        {
            if (password.Equals("applestore@123")) return true;
            return false;
        }
    }
}
