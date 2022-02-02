using PlayFab;
using PlayFabMonoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayFabMonoGame.Models
{
    public class AuthenticationContext : IAuthenticationContext
    {
        public string ClientSessionTicket { get; set; }
        public string PlayFabId { get; set; }
        public string EntityToken { get; set; }
        public string EntityId { get; set; }
        public string EntityType { get; set; }

        public AuthenticationContext() { }

        public AuthenticationContext(PlayFabAuthenticationContext pac)
        {
            ClientSessionTicket = pac.ClientSessionTicket;
            PlayFabId = pac.PlayFabId;
            EntityToken = pac.EntityToken;
            EntityId = pac.EntityId;
            EntityType = pac.EntityType;
        }
    }
}
