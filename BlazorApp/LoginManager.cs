using System.Security.Cryptography;
using System.Text.RegularExpressions;

class HashedPassword()
{
    public required byte[] hash { set; get; }
    public required byte[] salt { set; get; }
    public int iterations { set; get; }
}

class LoginManager()
{
    /*
        Checks length, numbers, etc.
    */

    public bool checkPasswordStrength(string password)
    {
        // Length Check
        if (password.Length < 8) return false;

        // Includes Digit
        Regex r = new Regex(".*[0-9].*");
        if (!r.IsMatch(password)) return false;

        // Valid Password
        return true;
    }

    public bool checkValidEmailFormat(string email)
    {
        Regex r = new Regex(@"^((\w+([-+.]?\w+)*@\w+([-.]?\w+)*\.\w+([-.]?\w+)*)\s*[;]{0,1}\s*)+$");
        if (!r.IsMatch(email)) return false;
        return true;
    }


    public HashedPassword hashAndSalt(string password)
    {
        Random random = new Random();
        HashedPassword pass = new HashedPassword
        {
            hash = string.Empty,
            salt = RandomNumberGenerator.GetBytes(16),
            iterations = 5
        };
        byte[] hash;
        using (var hashGenerator = new Rfc2898DeriveBytes(password, pass.salt, pass.iterations, HashAlgorithmName.SHA256))
        {
            hash = hashGenerator.GetBytes(64);
        }

        pass.hash = hash;
        Console.Write(pass.hash);
    }

    public bool checkPasswordWithHash(string password, string username)
    {
        HashedPassword attemptHash = hashAndSalt(password);

        // Check password linked to the username in the DB, compare hashes.
        // if username not in DB, return false;
        // if Hash doesnt match, return false;
        return true;
    }

}