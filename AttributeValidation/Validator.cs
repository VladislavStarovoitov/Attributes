using Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttributeValidation
{
    public static class Validator
    {
        public static void ValidateUser(User user)
        {
            List<MemberInfo> members = new List<MemberInfo>(user.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public));
            members.AddRange(user.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public));
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

        private static void ValidateStringFieldsAndProperties(User user, dynamic member, StringValidatorAttribute validator)
        {
            string value = member.GetValue(user);
            if (value.Length > validator.MaxLength)
            {
                throw new ValidationException(member.ToString());
            }
        }

        private static void ValidateIntFieldsAndProperties(User user, dynamic member, IntValidatorAttribute validator)
        {
            int value = member.GetValue(user);
            if (value > validator.Max && value < validator.Min)
            {
                throw new ValidationException(member.ToString());
            }
        }
    }
}
