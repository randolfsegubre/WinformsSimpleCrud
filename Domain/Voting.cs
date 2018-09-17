using System.Linq;

namespace Domain
{
    public class Voting
    {
        public static string Rights(int age)
        {
            const int maxValue = 200;
            var canVote = Enumerable.Range(18, maxValue);

            var isInfant = Enumerable.Range(0, 3);

            var canRetire = Enumerable.Range(65, maxValue);

            string privileges = null;

            if (canVote.Contains(age))
                privileges += " Can Vote ";
            if (canRetire.Contains(age))
                privileges += " Can Retire ";
            if (isInfant.Contains(age) || age < 4)
                privileges += " Is Infant ";

            return privileges;
        }
    }
}