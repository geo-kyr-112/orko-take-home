using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CsvParser
{
    internal class Program
    {
        private class CsvObject
        {
            public string HeaderRow = string.Empty;
            public List<string> Rows = new();
        }

        static void Main(string[] args)
        {
            string csvFilePath = args[0];
            string columnName = args[1];

            CsvObject sortedCsvObject = ParseCsvObject(csvFilePath, columnName);
            WriteCsvObject(sortedCsvObject);
        }

        /// <summary>
        /// Reads a csv file and parses it to an object containing a header and list of ordered rows
        /// </summary>
        /// <param name="csvFilePath">The file path</param>
        /// <param name="columnName">The name of the column to sort by</param>
        /// <returns>CsvObject.  Contains header row as a string and a list of ordered rows</returns>
        private static CsvObject ParseCsvObject(string csvFilePath, string columnName)
        {
            // Validate that file exists
            if (!File.Exists(csvFilePath))
            {
                throw new Exception("File does not exist");
            }

            string[] lines = File.ReadAllLines(csvFilePath);
            List<string> headers = lines[0].Split(',').ToList();
            // Validate that ordering column exists in the header
            if (!headers.Contains(columnName))
            {
                throw new Exception("Column does not exist in file header");
            }
            // Validate that the header value is unique by comparing the length of the headers as a total to a hash set of the headers
            HashSet<string> headerSet = new(headers);
            if (headers.Count != headerSet.Count)
            {
                throw new Exception("Header contains duplicate values");
            }
            int keyColumnIndex = headers.IndexOf(columnName);

            // Creates a dictionary where the key field is the one to order by and value is the entire row
            // This allows sorting based on the key while preserving the entire row
            Dictionary<string, string> csvMap = new();
            List<int> errorLines = new();
            for (int i = 1; i < lines.Length; i++)
            {
                List<string> line = lines[i].Split(',').ToList();
                if (line.Count != headers.Count)
                {
                    errorLines.Add(i);
                }
                else
                {
                    csvMap.Add(line[keyColumnIndex], lines[i]);
                }
            }
            // Validate that there are no lines with an incorrect number of columns
            if (errorLines.Count > 0)
            {
                throw new Exception($"Lines {string.Join(", ", errorLines)} do not have correct column count");
            }
            // Sorts the dictionary in descending order of the key column
            csvMap = csvMap.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            return new CsvObject
            {
                HeaderRow = lines[0],
                Rows = csvMap.Values.ToList()
            };
        }

        /// <summary>
        /// Prints to console a CsvObject
        /// </summary>
        /// <param name="csvObject">Object containing a header and list of ordered rows</param>
        private static void WriteCsvObject(CsvObject csvObject)
        {
            Console.WriteLine(csvObject.HeaderRow);
            foreach (var line in csvObject.Rows)
            {
                Console.WriteLine(line);
            }
        }
    }
}