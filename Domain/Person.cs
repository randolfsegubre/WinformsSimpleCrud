using System;

namespace Domain
{
    public class Person
    {
        #region Properties

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var difference = DateTime.Now - DateOfBirth;
                var age = (int)difference.TotalDays / 365;
                return age;
            }
        }

        public string TypeOfAge => AgeType.Seniority(Age);

        public string Privilege => Voting.Rights(Age);

        #endregion Properties

        #region Contructor

        /// <inheritdoc />
        public Person()
        {
            Id = int.MinValue;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            DateOfBirth = DateTime.MinValue;
        }

        #endregion Contructor
    }
}