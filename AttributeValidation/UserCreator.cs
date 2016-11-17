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
            var attributes = typeof(AdvancedUser).Assembly.GetCustomAttributes<InstantiateAdvancedUserAttribute>();

            return CreateUsers<InstantiateAdvancedUserAttribute, AdvancedUser>(attributes);
        }

        private static List<U> CreateUsers<T, U>(IEnumerable<T> attributes) where T: InstantiateUserAttribute
                                                                            where U: User
        {
            List<U> users = new List<U>();
            foreach (var item in attributes)
            {
                users.Add((U)CreateUser(item, typeof(U)));
            }

            return users;
        }

        private static User CreateUser(InstantiateUserAttribute userAttribute, Type userType)
        {
            User user;
            if (userType == typeof(User))
            {
                user = new User(userAttribute.Id ?? MatchPataremeter("id", typeof(InstantiateUserAttribute)));
            }
            else
            {
                var advancedUserAttribute = userAttribute as InstantiateAdvancedUserAttribute;
                user =
                    new AdvancedUser(advancedUserAttribute.Id ?? MatchPataremeter("id", typeof(InstantiateAdvancedUserAttribute)),
                                     advancedUserAttribute.ExternalId ?? MatchPataremeter("externalId", typeof(InstantiateAdvancedUserAttribute)));
            }
            return user;
        }

        private static dynamic MatchPataremeter(string paramName, Type type)
        {
            DefaultValueAttribute defaultAttribute = default(DefaultValueAttribute);
            var matchAttributes =
                (MatchParameterWithPropertyAttribute[])Attribute.GetCustomAttributes(type.GetConstructors()[0], typeof(MatchParameterWithPropertyAttribute));
            foreach (var item in matchAttributes)
            {
                if (paramName.Equals(item.Parameter, StringComparison.InvariantCultureIgnoreCase))
                {
                    PropertyInfo property = type.GetProperty(item.Property);
                    defaultAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
                }
            }     
            if (ReferenceEquals(defaultAttribute, null))
            {
                throw new MatchException();
            }       
            return defaultAttribute.Value;
        }
    }
}
