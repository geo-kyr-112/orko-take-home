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
            public Dictionary<string, string> RowKeyToRowMap = new();
        }

        static void Main(string[] args)
        {
            string csvFilePath = args[0];
            string columnName = args[1];

            CsvObject sortedCsvObject = ParseCsvObject(csvFilePath, columnName);
            WriteCsvObject(sortedCsvObject);
        }

        private static CsvObject ParseCsvObject(string csvFilePath, string columnName)
        {
            if (!File.Exists(csvFilePath))
            {
                throw new Exception("File does not exist");
            }

            string[] lines = File.ReadAllLines(csvFilePath);
            List<string> headers = lines[0].Split(',').ToList();
            if (!headers.Contains(columnName))
            {
                throw new Exception("Column does not exist in file header");
            }
            HashSet<string> headerSet = new(headers);
            if (headers.Count != headerSet.Count)
            {
                throw new Exception("Header contains duplicate values");
            }
            int keyColumnIndex = headers.IndexOf(columnName);

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
            if (errorLines.Count > 0)
            {
                throw new Exception($"Lines {string.Join(", ", errorLines)} do not have correct column count");
            }
            csvMap = csvMap.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            return new CsvObject
            {
                HeaderRow = lines[0],
                RowKeyToRowMap = csvMap
            };
        }

        private static void WriteCsvObject(CsvObject csvObject)
        {
            Console.WriteLine(csvObject.HeaderRow);
            foreach (var line in csvObject.RowKeyToRowMap)
            {
                Console.WriteLine(line.Value);
            }
        }
    }
}