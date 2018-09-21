using System.Linq;

namespace Domain
{
    public class AgeType
    {
        #region Methods

        public static string Seniority(int age)
        {
            //int[] max = new int[int.MaxValue];
            var minor = Enumerable.Range(0, 18);

            var adult = Enumerable.Range(18, 70);

            var senior = Enumerable.Range(70, 200);

            if (senior.Contains(age))
            {
                return "Senior";
            } 
            else if (adult.Contains(age))
            {
                return "Adult";
            }
            else if (minor.Contains(age))
            {
                return "Minor";
            }
            else
                return string.Empty;
        }

        #endregion Methods
    }
}