using System.Text.RegularExpressions;

namespace Test
{
    public class UserFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userList">List of users in which will be username finding</param>
        /// <param name="completeUsername">User name with or without domain example: (domain\username or username)</param>
        /// <returns>Id of user if is find in DB or null if not</returns>
        public int? GetUserId(IEnumerable<User> userList, string completeUsername)
        {
            var splittedUserName = completeUsername.Split('\\');
            var userNameWithoutDomain = splittedUserName.Last();

            string regexPattern = @$"^(.*\\)({userNameWithoutDomain})$|^{userNameWithoutDomain}$";
            var regex = new Regex(regexPattern);

            var filteredUserNames = userList.Where(x => regex.IsMatch(x.Username)).ToList();

            if (!filteredUserNames.Any())
            {
                return null;
            }

            var containsDomain = splittedUserName.Length > 1;

            var idUser = containsDomain
                            ? FilterByDomain(filteredUserNames, completeUsername)
                            : FilterWithoutDomain(filteredUserNames);
            return idUser;
        }

        private int? FilterWithoutDomain(List<User> userList)
        {
            var idUser = userList.Count > 1
                            ? default(int?)
                            : userList[0].Id;
            return idUser;
        }

        private int? FilterByDomain(List<User> userList, string completeUsername)
        {
            var userOfSpecifiedDomain = userList.Where(x => x.Username == completeUsername)
                                                .SingleOrDefault();

            if (userOfSpecifiedDomain != null)
            {
                return userOfSpecifiedDomain.Id;
            }

            var userWithoutDomain = userList.Where(x => x.Username.Split('\\').Length == 1)
                                            .Single();

            if (userWithoutDomain != null)
            {
                var idUser = userWithoutDomain.Id;
                return idUser;
            }

            return null;
        }
    }
}
