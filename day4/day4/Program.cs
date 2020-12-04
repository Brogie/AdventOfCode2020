using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("../../../input.txt");
            string[] passports = sr.ReadToEnd().Split("\r\n\r\n");

            Console.WriteLine(Part1(passports));
            Console.WriteLine(Part2(passports));
        }

        static int Part1(string[] passports)
        {
            int validPassports = 0;

            foreach (var passport in passports)
            {
                if(IsPassportHeaderValid(passport)) { validPassports++; }
            }

            return validPassports;
        }

        static int Part2(string[] passports)
        {
            int validPassports = 0;

            foreach (var passport in passports)
            {
                if (IsPassportDataValid(passport)) { validPassports++; }
            }

            return validPassports;
        }

        private static bool IsPassportHeaderValid(string passport)
        {
            Regex rx = new Regex("(...):");

            MatchCollection matches = rx.Matches(passport);
            List<string> keysRequired = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            List<string> keysFound = new List<string>();

            foreach (Match match in matches)
            {
                keysFound.Add(match.Groups[1].Value);
            }

            foreach (var key in keysRequired)
            {
                if(!keysFound.Contains(key)) { return false; }
            }

            return true;
        }

        private static bool IsPassportDataValid(string passport)
        {
            if(!IsPassportHeaderValid(passport))
            {
                return false;
            }

            Regex rx = new Regex("(...):([a-z,0-9,\\#]*)");
            Dictionary<string, string> passportItems = new Dictionary<string, string>();

            foreach (Match match in rx.Matches(passport))
            {
                passportItems.Add(match.Groups[1].Value, match.Groups[2].Value);
            }

            //filter years
            if (!rangeValid(passportItems["byr"], 1920,2002) ||
                !rangeValid(passportItems["iyr"], 2010, 2020) ||
                !rangeValid(passportItems["eyr"], 2020, 2030)) {
                return false;
            }

            //filter hight
            if (passportItems["hgt"].EndsWith("cm") && !rangeValid(passportItems["hgt"].Replace("cm", string.Empty), 150, 193))
            { 
                return false; 
            }
            else if (passportItems["hgt"].EndsWith("in") && !rangeValid(passportItems["hgt"].Replace("in", string.Empty), 59, 76))
            {
                return false;
            }
            else if (!passportItems["hgt"].EndsWith("cm") && !passportItems["hgt"].EndsWith("in"))
            {
                return false;
            }

            //filter hair
            Regex hexColour = new Regex("#[0-9,a-f]{6}");
            if(!hexColour.IsMatch(passportItems["hcl"])) { return false; }

            //filter eyes
            List<string> ValidEyeColours = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            if(!ValidEyeColours.Contains(passportItems["ecl"])) { return false; }

            //filter passport id
            Regex passId = new Regex("[0-9]{9}");
            if (passportItems["pid"].Length != 9 || !passId.IsMatch(passportItems["pid"])) { return false; }

            Console.WriteLine(passportItems["pid"]);

            return true;
        }

        private static bool rangeValid(string year, int start, int end)
        {
            return int.Parse(year) >= start && int.Parse(year) <= end;
        }
    }
}
