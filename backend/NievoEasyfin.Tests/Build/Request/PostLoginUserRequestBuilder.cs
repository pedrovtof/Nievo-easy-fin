using System;
using System.Text;
using Bogus;
using NievoEasyfin.Application.Interfaces.Request;

namespace NievoEasyfin.Tests.Build.Request
{
    public class PostLoginUserRequestBuilder : PostLoginUserRequest
    {
        private readonly Faker _faker = new Faker("pt_BR");

        /// <summary>
        /// Default setter to build
        /// </summary>
        /// <param name="email">str</param>
        internal void Default()
        {
            Email = _faker.Person.Email;
            Password = BuildRamdomPassword();
        }

        /// <summary>
        /// Method to create a total ramdom password
        /// </summary>
        /// <returns>string</returns>
        internal string BuildRamdomPassword()
        {
            List<string> symbols = new List<string> {
                "!", "@","#","$","%",
                "^","&","*","(",")",
                "+","=","/","[","]",
                "{","}","\\","`","~",
                "<",">",",","."
            };

            StringBuilder str = new StringBuilder();

            str.Append($"{_faker.Hacker.Random.AlphaNumeric(3).ToUpper()}");
            str.Append($"{_faker.Random.AlphaNumeric(3)}");
            str.Append($"{_faker.Random.Number(3).ToString()}");
            str.Append($"{string.Join("", _faker.PickRandom(symbols, _faker.Random.Number(1, 3)))}");

            return str.ToString();
        }

        /// <summary>
        /// Setter to build
        /// </summary>
        /// <param name="email">str</param>
        internal void WithEmail(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Setter to build
        /// </summary>
        /// <param name="email">str</param>
        internal void WithPassword(string password)
        {
            Password = password;
        }
    }
}
