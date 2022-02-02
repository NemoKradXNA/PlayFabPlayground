using System;
using System.Collections.Generic;
using System.Text;

namespace PlayFabMonoGame.Interfaces
{
    public interface IRegistrationModel
    {
        string UserName { get; set; }
        string DisplayName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool RequireBothUsernameAndEmail { get; set; }
    }
}
