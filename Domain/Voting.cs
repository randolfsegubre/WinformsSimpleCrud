using System.Linq;
using System.Text;
using System.Xml;

namespace Domain
{
    public class Voting 
    {
        public static string Rights(int age)
        {
            
            var canVote = Enumerable.Range(AgeRange.MinVoteAgeRange, AgeRange.MaxAgeRange);

            var isInfant = Enumerable.Range(AgeRange.StartingAgeRage, AgeRange.InfantMaxAgeRange);

            var canRetire = Enumerable.Range(AgeRange.MinRetireAgeRange, AgeRange.MaxAgeRange);

            var privileges = new StringBuilder();

            if (canVote.Contains(age))
                privileges.Append(Constants.CanVoteConst) ;
            if (canRetire.Contains(age))
                privileges.Append(Constants.CanRetireConst);
            if (isInfant.Contains(age) || age < 4)
                privileges.Append(Constants.IsInfantConst);

            return privileges.ToString();
        }
    }
}