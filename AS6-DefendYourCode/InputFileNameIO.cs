﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AS6_DefendYourCode
{
    internal class InputFileNameIO
    {

        internal string InputFileName { get; private set; }
        internal string OutputFileName { get; private set; }
        private List<string> _originalInput = new List<string>();
        internal List<string> errors { get; private set; }

        internal void Prompt()
        {
            // TODO: Limit file extension type and directory location!
            try
            {
                if (errors == null)
                    errors = new List<string>();
                Console.Write("\nEnter Input File Name: ");
                InputFileName = Console.ReadLine().Trim();
                if (!TestPrompt(InputFileName)) throw new Exception("Input file incorrect format.");

                Console.Write("\nEnter Output File Name: ");
                OutputFileName = Console.ReadLine().Trim();
                if (!TestPrompt(OutputFileName)) throw new Exception("Output file incorrect format.");
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an error in your file input: ");
                Console.WriteLine(e);
                Console.WriteLine("Only .txt files in this directory are accepted, example input: abc.txt");
                errors.Add("InputFileNameIO - Prompt() " + e.ToString());
                Prompt();
            }
        }

        // TODO: Read input file and then go thru it and print it on the output file
        internal void WriteTo(InputName name, InputInteger integer)
        {
            SaveOriginalInputFileContent();
            using (var stream = new StreamWriter("..\\..\\..\\" + OutputFileName))
            {
                try
                {
                    stream.WriteLine("{0}, {1}\nSum: {2}\nProduct: {3}", new object[] {
                        name.LastName, name.FirstName, integer.Sum(), integer.Multiply()
                    });
                    _originalInput.ForEach(s => stream.WriteLine(s));
                }
                catch (Exception e)
                {
                    Console.WriteLine("There was an issue writing Files. Lets pick 2 new files: ");
                    errors.Add("InputFileNameIO - WriteTo(InputName name, InputInteger integer) " + e.ToString());
                    Prompt();
                    throw new IOException(); // TODO: fix? #HALF DONE????
                }
            }
        }

        private void SaveOriginalInputFileContent() 
        {
            using (var reader = new StreamReader("..\\..\\..\\" + InputFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _originalInput.Add(line);
                }
            }
        }
        
        // TODO: this right her ewill check if its a txt, and location. (like how big integer class was "supposed" to be) #DONE
        internal bool TestPrompt(string s)
        {
            return Regex.IsMatch(s, "^([a-z]+.txt)$");
        }
    }
}
