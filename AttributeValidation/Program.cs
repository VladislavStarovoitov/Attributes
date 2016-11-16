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
            List<MemberInfo> members = new List<MemberInfo>(user.GetType().GetFields());
            members.AddRange(user.GetType().GetProperties());

            foreach (var item in members)
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(item);
                foreach (var attribute in attributes)
                {
                    if (attribute is StringValidatorAttribute)
                    {
                        ValidateStringFieldsAndProperties(user, item, (StringValidatorAttribute)attribute);
                    }
                    else
                    {
                        if (attribute is IntValidatorAttribute)
                        {
                            ValidateIntFieldsAndProperties(user, item, (IntValidatorAttribute)attribute);
                        }
                    }
                }
            }
        }

        static dynamic MatchPataremeter(Type type)
        {
            var matchAttributes =
                (MatchParameterWithPropertyAttribute[])Attribute.GetCustomAttributes(type.GetConstructors()[0], typeof(MatchParameterWithPropertyAttribute));
            PropertyInfo property = type.GetProperty(matchAttributes[0].Property);
            DefaultValueAttribute defaultAttribute = property.GetCustomAttribute<DefaultValueAttribute>();
            return defaultAttribute.Value;
        }

        static void ValidateStringFieldsAndProperties(User user, dynamic member, StringValidatorAttribute validator)
        {
            string value = member.GetValue(user);
            if (value.Length > validator.MaxLength)
            {
                throw new ValidationException(member.ToString());
            }
        }

        static void ValidateIntFieldsAndProperties(User user, dynamic member, IntValidatorAttribute validator)
        {
            int value = member.GetValue(user);
            if (value > validator.Max && value < validator.Min)
            {
                throw new ValidationException(member.ToString());
            }
        }
    }
}
