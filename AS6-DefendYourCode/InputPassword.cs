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
        internal List<string> Errors { get; private set; }
		private byte[] _salt;
		private byte[] _hash;
		private Dictionary<byte[], byte[]> _accounts = new Dictionary<byte[], byte[]>();
		

        private static string ReadLine()
        {
            // TODO: Test to see if this works?
            Console.SetIn(new StreamReader(Console.OpenStandardInput(READLINE_BUFFER_SIZE)));
            return Console.ReadLine();
        }

   //     internal void Prompt()
   //     {
   //         try
   //         {
			//	if (Errors == null)
			//		Errors = new List<string>();
			//	Console.Write("\nEnter A Password (must be between 6-12 characters long): ");
			//	SecurePassword(ReadLine().Trim());
			//	System.Threading.Thread.Sleep(700);
			//	StorePassword();
			//	Console.WriteLine("PASSWORD HASHED and STORED");
			//	int tries = 3;
			//	do
			//	{
   //                 Console.Write("Re-enter Password ({0} tries remaining): ", tries);
   //                 SecurePassword(ReadLine().Trim());
			//		Console.WriteLine(@"VALIDATING PASSWORD \|/__");
			//		System.Threading.Thread.Sleep(1500);
			//		if (ValidatePassword())
			//		{
			//			Console.WriteLine("\nPASS\n");
			//			break;
			//		}
			//		else
			//		{
			//			Console.WriteLine("\nFAIL\n");
			//			tries--;
			//			if (tries <= 0)
			//			{
			//				Console.WriteLine("****[Attempts expired, exiting application for security]****");
			//				System.Environment.Exit(1);
			//			}
			//		}
			//	} while (tries > 0);
			//}
   //         catch (Exception e)
   //         {
   //             Console.WriteLine("There was an error in your password input: ");
   //             Console.WriteLine(e);
   //             Errors.Add("InputPassword - Prompt() " + e.ToString());
   //             Prompt();
   //         }
   //     }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        internal void HungPrompt()
        {
            try
            {
                if (Errors == null)
                    Errors = new List<string>();
                Console.Write("\nEnter A Password (must be between 6-12 characters long): ");
                string originalPassword = Console.ReadLine().Trim();
                if (!HungValidatePasswordInput(originalPassword)) throw new Exception();
                originalPassword = HungSecurePassword(originalPassword);
                Console.WriteLine("PASSWORD is now ENCRPYTED");
                int tries = 3;
                while (tries > 0)
                {
                    Console.Write("Re-enter Password ({0} tries remaining): ", tries);
                    string newPassword = Console.ReadLine().Trim();
                    if (HungValidatePasswordInput(newPassword))
                    {
                        newPassword = HungSecurePassword(newPassword);
                        tries = originalPassword.Equals(newPassword) ? 0 : tries -= 1;
                        //Console.WriteLine(originalPassword.ToString());
                        //Console.WriteLine(newPassword.ToString());
                    }
                    else
                    {
                        Console.WriteLine("2nd Password did not follow the password format!! Try again!");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error in your password input: ");
                Console.WriteLine(e);
                Errors.Add("InputPassword - Prompt() " + e.ToString());
                HungPrompt();
            }
        }

        private string HungSecurePassword(string s)
        {
            var bytes = new UTF8Encoding().GetBytes(s);
            byte[] hashBytes;
            using (var algorithm = new SHA512Managed())
            {
                hashBytes = algorithm.ComputeHash(bytes);
            }
            return Convert.ToBase64String(hashBytes);
        }

        private bool HungValidatePasswordInput(string s)
        {
            return Regex.IsMatch(s, @"^[\w\!\@\#\$\%\^\&\*\)\(\-\+\=\]\[\}\{\]\|\>\<\?\~\`]{6,13}$");
        }

  //      private void SecurePassword(string input)
  //      {
  //          if (new Regex(@"^[\w\!\@\#\$\%\*\)\(\-\+\=_\]\[\}\{\|\?\~\`]{6,12}$").IsMatch(input))
  //          {
  //              byte[] password = Encoding.UTF8.GetBytes(input);
  //              if (_salt == null) _salt = GenerateSalt();
  //              _hash = GenerateSaltedHash(password, _salt);
		//		input = null;
		//		password = null;
  //          }
  //          else
  //          {
		//		// TODO: print to error log and handle better...
  //              throw new Exception("Secure Password issues.");
  //          }
  //      }

		//private void StorePassword()
		//{
		//	var name = new InputName();
		//	var file = new FileStream("..\\..\\..\\" + "account.txt", FileMode.Create, FileAccess.Write);
		//	using (var stream = new StreamWriter(file))
		//	{
		//		try
		//		{
		//			if (!_accounts.ContainsKey(_salt))
		//				_accounts.Add(_salt, _hash);
		//			else
		//				_accounts[_salt] = _hash;
		//			stream.Write("{0}\n{1}\n", Convert.ToBase64String(_salt), Convert.ToBase64String(_hash));
		//		}
		//		catch (Exception e)
		//		{
		//			// TODO: print ot error log and handle better...like specifiy all possible exception types i.e, IOException...
		//			throw new Exception("There was an issue in storing the password in this file.");
		//		}
		//		finally
		//		{
		//			stream.Close();
		//			file.Close();
		//		}
		//	}
		//}

		//private bool ValidatePassword()
		//{
		//	string readSalt = null;
		//	string readHash = null;
		//	var list = new List<string>();
		//	var lines = File.ReadAllLines("..\\..\\..\\" + "account.txt", Encoding.UTF8);
		//	try
		//	{
		//		readSalt = lines[0];
		//		readHash = lines[1];
		//		return Convert.ToBase64String(_salt).Equals(readSalt) && Convert.ToBase64String(_hash).Equals(readHash) ? true : false;
		//	}
		//	catch (Exception e)
		//	{
		//		throw new Exception("Error reading from file.");
		//	}
		//	finally
		//	{

		//	}
		//}

  //      // TODO: test to see if this is being generated by rnadomness or by username?
  //      private byte[] GenerateSalt()
  //      {
		//	using (var hash = new RNGCryptoServiceProvider())
		//	{
		//		byte[] salt = new byte[10];
		//		hash.GetBytes(salt);
		//		return salt;
		//	}
  //      }

  //      private byte[] GenerateSaltedHash(byte[] password, byte[] salt)
  //      {
  //          using (var hmac = new HMACSHA256(salt))
  //          {
  //              return hmac.ComputeHash(password);
  //          }
  //      }

  //      public bool TestPrompt(string s)
  //      {
  //          return TestSecurePassword(s);
  //      }

  //      private bool TestSecurePassword(string input)
  //      {
  //          if (new Regex(@"^[\w\!\@\#\$\%\^\&\*\)\(\-\+\=\]\[\}\{\]\|\>\<\?\~\`]{6,13}$").IsMatch(input))
  //          {
  //              byte[] password = Encoding.UTF8.GetBytes(input);
  //              byte[] salt = GenerateSalt();
  //              byte[] hash = GenerateSaltedHash(password, salt);
  //              return true;
  //          }
  //          else
  //          {
  //              return false;
  //          }
  //      }

    }
}
