using System;
using System.Collections.Generic;
using System.Text;

namespace PlayFabMonoGame.Interfaces
{
    public interface IAuthenticationContext
    {
        string ClientSessionTicket { get; set; }
        string PlayFabId { get; set; }
        string EntityToken { get; set; }
        string EntityId { get; set; }
        string EntityType { get; set; }
    }
}
