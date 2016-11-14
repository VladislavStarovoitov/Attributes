using Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
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

        static List<User> CreateUsers()
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
            User user = new User(userAttribute.Id)
            {
                FirstName = userAttribute.FirstName,
                LastName = userAttribute.LastName
            };
            ValidateUser(user);
            return user;
        }

        static void ValidateUser(User user)
        {

        }
    }
}
