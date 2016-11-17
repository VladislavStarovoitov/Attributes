using Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static AttributeValidation.UserCreator;

namespace AttributeValidation
{
    class  Program
    {
        static void Main(string[] args)
        {
            var users = CreateUsers();
            var advancedUsers = CreateAdvancedUsers();
            foreach (var item in advancedUsers)
            {
                Validator.ValidateUser(item);
            }
        }
    }
}
