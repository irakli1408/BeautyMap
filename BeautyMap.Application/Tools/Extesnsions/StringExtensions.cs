using Microsoft.AspNetCore.Identity;

namespace BeautyMap.Application.Tools.Extesnsions
{
    public static class StringExtensions
    {
        public static string GenerateRandomPassword()
        {
            PasswordOptions opts = new()
            {
                RequiredLength = 8,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };
            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",
            "abcdefghijkmnopqrstuvwxyz",
            "0123456789",
            "!@$?_-"
            };
            Random rand = new(Environment.TickCount);
            List<char> chars = new();
            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);
            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);
            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);
            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);
            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }
            return new string(chars.ToArray());
        }

        public static string GenerateAccessCode()
        {
            const int codeLength = 6;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
            Random random = new();
            char[] code = new char[codeLength];
            for (int i = 0; i < codeLength; i++)
            {
                code[i] = chars[random.Next(chars.Length)];
            }
            return new string(code);
        }

        public static bool IsPathContainsOrderedFolders(this string path, string[] requestedFolders)
        {
            var pathFolders = path.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            if (pathFolders.Length < requestedFolders.Length)
                return false;

            for (int i = 0; i < requestedFolders.Length; i++)
            {
                if (pathFolders[i] != requestedFolders[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
