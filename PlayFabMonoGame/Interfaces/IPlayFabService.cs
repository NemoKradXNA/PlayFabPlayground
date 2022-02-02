using PlayFab;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabMonoGame.Interfaces
{
    public interface IPlayFabService
    {
        string PlayFabTitleId { get; }

        Task<IAuthenticationContext> Register(IRegistrationModel model);
        Task<IAuthenticationContext> Login(IRegistrationModel model, IAuthenticationContext pac = null);
    }
}
