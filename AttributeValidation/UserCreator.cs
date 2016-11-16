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
                users.Add((U)CreateUser(item, typeof(U), null));
            }

            return users;
        }

        private static User CreateUser(InstantiateUserAttribute userAttribute, Type userType, ParameterInfo[] parameters)
        {
            User user = (User)Activator.CreateInstance(userType);
            //User user = new User(userAttribute.Id ?? MatchPataremeter(typeof(User)))//предусмотреть наследников
            //{
            //    FirstName = userAttribute.FirstName,
            //    LastName = userAttribute.LastName
            //};
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
