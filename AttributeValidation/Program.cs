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
    class Program
    {
        static void Main(string[] args)
        {
            var users = CreateUsers();
        }

        static List<User> CreateUsers()//принимать тип атрибута
        {
            List<User> users = new List<User>();
            InstantiateUserAttribute[] attributes =
                (InstantiateUserAttribute[])Attribute.GetCustomAttributes(typeof(User), typeof(InstantiateUserAttribute));
            foreach (var item in attributes)
            {
                users.Add(CreateUser(item));
            }

            return users;
        }

        static User CreateUser(InstantiateUserAttribute userAttribute)
        {
            User user = new User(userAttribute.Id ?? MatchPataremeter(typeof(User)))//предусмотреть наследников
            {
                FirstName = userAttribute.FirstName,
                LastName = userAttribute.LastName
            };
            ValidateUser(user);
            return user;
        }

        static void ValidateUser(User user)
        {
            ValidateStringFields(user);
        }

        static dynamic MatchPataremeter(Type type)
        {
            var matchAttributes =
                (MatchParameterWithPropertyAttribute[])Attribute.GetCustomAttributes(type.GetConstructors()[0], typeof(MatchParameterWithPropertyAttribute));
            PropertyInfo property = type.GetProperty(matchAttributes[0].Property);
            DefaultValueAttribute defaultAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
            return defaultAttribute.Value;
        }

        static void ValidateStringFields(User user)
        {
            
        }

        static void ValidateIntFields(User user)
        {

        }
    }
}
