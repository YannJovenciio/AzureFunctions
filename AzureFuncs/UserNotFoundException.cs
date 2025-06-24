using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureFunctions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() { }

        public UserNotFoundException(string message) : base(message) { }

        public UserNotFoundException(string message, Exception inner) : base(message, inner) { }

        public override string Message
        {
            get
            {
                return "Usuário não encontrado. Por favor, verifique o ID do usuário e tente novamente.";
            }
        }
        
    }
}