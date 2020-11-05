using System.Linq;
using FileStorage.Models;


namespace FileStorage
{
    static class ConsoleCommandParser
    {
        public static bool IsInitialParametersCorrect(string[] args)
        {
            if (args.Count() == 4 && (args[0] == "--l" || args[2] == "--l") && (args[0] == "--p" || args[2] == "--p"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Credentials GetCredentialsFromInitialParameters(string[] args)
        {
           Credentials credentials = new Credentials();
            
           if (args[0] == "--l" && args[2] == "--p")
            {
                credentials.Login = args[1];
                credentials.Password = args[3];
            }
            else
            {
                credentials.Login = args[3];
                credentials.Password = args[1];
            }
            return credentials;
        }
    }
}
