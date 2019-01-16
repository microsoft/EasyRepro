// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;

namespace Microsoft.Dynamics365.UIAutomation.Sample
{
    public static class TestSettings
    {
        public static string InvalidAccountLogicalName = "accounts";

        public static string LookupField = "primarycontactid";
        public static string LookupName = "Rene Valdes (sample)";
        private static readonly string Type = System.Configuration.ConfigurationManager.AppSettings["BrowserType"].ToString();

        public static BrowserOptions Options = new BrowserOptions
        {
            BrowserType = (BrowserType)Enum.Parse(typeof(BrowserType), Type),
            PrivateMode = true,
            FireEvents = false,
            Headless = false,
            UserAgent = false,
            DefaultThinkTime = 2000
        };

        public static string GetRandomFirstName()
        {
            string[] firstNames = new string[] { "Jacob", "Isabella", "Ethan", "Sophia", "Michael", "Emma", "Jayden", "Olivia", "William", "Ava", "Alexander", "Emily", "Noah", "Abigail", "Daniel", "Madison", "Aiden", "Chloe", "Anthony", "Mia", "Joshua", "Addison", "Mason", "Elizabeth", "Christopher", "Ella", "Andrew", "Natalie", "David", "Samantha", "Matthew", "Alexis", "Logan", "Lily", "Elijah", "Grace", "James", "Hailey", "Joseph", "Alyssa", "Gabriel", "Lillian", "Benjamin", "Hannah", "Ryan", "Avery", "Samuel", "Leah", "Jackson", "Nevaeh", "John", "Sofia", "Nathan", "Ashley", "Jonathan", "Anna", "Christian", "Brianna", "Liam", "Sarah", "Dylan", "Zoe", "Landon", "Victoria", "Caleb", "Gabriella", "Tyler", "Brooklyn", "Lucas", "Kaylee", "Evan", "Taylor", "Gavin", "Layla", "Nicholas", "Allison", "Isaac", "Evelyn", "Brayden", "Riley", "Luke", "Amelia", "Angel", "Khloe", "Brandon", "Makayla", "Jack", "Aubrey", "Isaiah", "Charlotte", "Jordan", "Savannah", "Owen", "Zoey", "Carter", "Bella", "Connor", "Kayla", "Justin", "Alexa", "Jose", "Peyton", "Jeremiah", "Audrey", "Julian", "Claire", "Robert", "Arianna", "Aaron", "Julia", "Adrian", "Aaliyah", "Wyatt", "Kylie", "Kevin", "Lauren", "Hunter", "Sophie", "Cameron", "Sydney", "Zachary", "Camila", "Thomas", "Jasmine", "Charles", "Morgan", "Austin", "Alexandra", "Eli", "Jocelyn", "Chase", "Gianna", "Henry", "Maya", "Sebastian", "Kimberly", "Jason", "Mackenzie", "Levi", "Katherine", "Xavier", "Destiny", "Ian", "Brooke", "Colton", "Trinity", "Dominic", "Faith", "Juan", "Lucy", "Cooper", "Madelyn", "Josiah", "Madeline", "Luis", "Bailey", "Ayden", "Payton", "Carson", "Andrea", "Adam", "Autumn", "Nathaniel", "Melanie", "Brody", "Ariana", "Tristan", "Serenity", "Diego", "Stella", "Parker", "Maria", "Blake", "Molly", "Oliver", "Caroline", "Cole", "Genesis", "Carlos", "Kaitlyn", "Jaden", "Eva", "Jesus", "Jessica", "Alex", "Angelina", "Aidan", "Valeria", "Eric", "Gabrielle", "Hayden", "Naomi", "Bryan", "Mariah", "Max", "Natalia", "Jaxon", "Paige", "Brian", "Rachel" };
            Random rand = new Random();
            string firstName = firstNames[rand.Next(0, firstNames.Length)];

            return firstName;
        }

        public static string GetRandomLastName()
        {
            string[] lastNames = new string[] { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King", "Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker", "Gonzalez", "Nelson", "Carter", "Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Sanchez", "Morris", "Rogers", "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey", "Rivera", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez", "James", "Watson", "Brooks", "Kelly", "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross", "Henderson", "Coleman", "Jenkins", "Perry", "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington", "Butler", "Simmons", "Foster", "Gonzales", "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes" };
            Random rand = new Random();
            string lastName = lastNames[rand.Next(0, lastNames.Length)];

            return lastName;
        }

        public static string GetRandomString(int minLen, int maxLen)
        {
            char[] Alphabet = ("ABCDEFGHIJKLMNOPQRSTUVWXYZabcefghijklmnopqrstuvwxyz0123456789").ToCharArray();
            Random m_randomInstance = new Random();
            Object m_randLock = new object();

            int alphabetLength = Alphabet.Length;
            int stringLength;
            lock (m_randLock) { stringLength = m_randomInstance.Next(minLen, maxLen); }
            char[] str = new char[stringLength];

            // max length of the randomizer array is 5
            int randomizerLength = (stringLength > 5) ? 5 : stringLength;

            int[] rndInts = new int[randomizerLength];
            int[] rndIncrements = new int[randomizerLength];

            // Prepare a "randomizing" array
            for (int i = 0; i < randomizerLength; i++)
            {
                int rnd = m_randomInstance.Next(alphabetLength);
                rndInts[i] = rnd;
                rndIncrements[i] = rnd;
            }

            // Generate "random" string out of the alphabet used
            for (int i = 0; i < stringLength; i++)
            {
                int indexRnd = i % randomizerLength;
                int indexAlphabet = rndInts[indexRnd] % alphabetLength;
                str[i] = Alphabet[indexAlphabet];

                // Each rndInt "cycles" characters from the array, 
                // so we have more or less random string as a result
                rndInts[indexRnd] += rndIncrements[indexRnd];
            }
            return (new string(str));
        }
    }

    public static class UCIAppName
    {
        public static string Sales = "Sales Hub";
        public static string CustomerService = "Customer Service Hub";
        public static string Project = "Project Resource Hub";
        public static string FieldService = "Field Resource Hub";
    }
}
