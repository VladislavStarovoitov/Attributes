using Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributeValidation
{
    public static class UserCreator
    {
        public static List<User> CreateUsers()
        {
            InstantiateUserAttribute[] attributes =
                (InstantiateUserAttribute[])Attribute.GetCustomAttributes(typeof(User), typeof(InstantiateUserAttribute));

            return CreateUsers<InstantiateUserAttribute, User>(attributes);
        }

        public static List<AdvancedUser> CreateAdvancedUsers()
        {
            List<AdvancedUser> users = new List<AdvancedUser>();
            var attributes = typeof(AdvancedUser).Assembly.GetCustomAttributes<InstantiateAdvancedUserAttribute>();

            return CreateUsers<InstantiateAdvancedUserAttribute, AdvancedUser>(attributes);
        }

        private static List<K> CreateUsers<T, K>(IEnumerable<T> attributes) where T: InstantiateUserAttribute
                                                                            where K: User
        {
            List<K> users = new List<K>();
            foreach (var item in attributes)
            {
                users.Add((K)CreateUser(item, null));
            }

            return null;
        }

        private static User CreateUser(InstantiateUserAttribute userAttribute, ParameterInfo[] parameters)
        {
            User user = new User(userAttribute.Id ?? MatchPataremeter(typeof(User)))//предусмотреть наследников
            {
                FirstName = userAttribute.FirstName,
                LastName = userAttribute.LastName
            };
            return user;
        }

        private static dynamic MatchPataremeter(Type type)
        {
            var matchAttributes =
                (MatchParameterWithPropertyAttribute[])Attribute.GetCustomAttributes(type.GetConstructors()[0], typeof(MatchParameterWithPropertyAttribute));
            PropertyInfo property = type.GetProperty(matchAttributes[0].Property);
            DefaultValueAttribute defaultAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
            return defaultAttribute.Value;
        }
    }
}
