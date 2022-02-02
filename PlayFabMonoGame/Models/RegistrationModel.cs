using PlayFabMonoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayFabMonoGame.Models
{
    public class RegistrationModel : IRegistrationModel
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool RequireBothUsernameAndEmail { get; set; }
    }
}
