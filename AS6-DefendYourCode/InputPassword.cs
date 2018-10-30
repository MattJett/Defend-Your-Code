﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AS6_DefendYourCode
{
    internal class InputPassword
    {
        private const int READLINE_BUFFER_SIZE = 12; // TODO: see if we need 13 or 12?
    
        internal string HashedPassword { get; private set; }

        private static string ReadLine()
        {
            // TODO: Test to see if this works?
            Console.SetIn(new StreamReader(Console.OpenStandardInput(READLINE_BUFFER_SIZE)));
            return Console.ReadLine();
        }

        internal void Prompt()
        {
            try
            {
                Console.Write("\nEnter A Password (must be between 6-12 characters long): ");
                HashedPassword = SecurePassword(ReadLine().Trim());
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error in your password input: ");
                Console.WriteLine(e);
                Prompt();
            }
        }

        private string SecurePassword(string input)
        {
            if (new Regex(@"^[\w\!\@\#\$\%\^\&\*\)\(\-\+\=\]\[\}\{\]\|\>\<\?\~\`]{6,12}$").IsMatch(input))
            {
                byte[] password = Encoding.UTF8.GetBytes(input);
                byte[] salt = GenerateSalt();
                byte[] hash = GenerateSaltedHash(password, salt);
                return Convert.ToBase64String(hash);
            }
            else
            {
                throw new Exception(); // TODO: do more here?
            }
        }

        // TODO: test to see if this is being generated by rnadomness or by username?
        private byte[] GenerateSalt()
        {
            using (var hash = new RNGCryptoServiceProvider())
            {
                var salt = new byte[10];
                hash.GetBytes(salt);
                return salt;
            }
        }

        private byte[] GenerateSaltedHash(byte[] password, byte[] salt)
        {
            using (var hmac = new HMACSHA256(salt))
            {
                return hmac.ComputeHash(password);
            }
        }

        public bool TestPrompt(string s)
        {
            return TestSecurePassword(s);
        }

        private bool TestSecurePassword(string input)
        {
            if (new Regex(@"^[\w\!\@\#\$\%\^\&\*\)\(\-\+\=\]\[\}\{\]\|\>\<\?\~\`]{6,12}$").IsMatch(input))
            {
                byte[] password = Encoding.UTF8.GetBytes(input);
                byte[] salt = GenerateSalt();
                byte[] hash = GenerateSaltedHash(password, salt);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
