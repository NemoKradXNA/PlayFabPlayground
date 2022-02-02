using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabMonoGame.Interfaces;
using PlayFabMonoGame.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlayFabMonoGame.Services
{
    public class PlayFabService : GameComponent, IPlayFabService
    {
        string _playFabTitleId;
        public string PlayFabTitleId { get { return _playFabTitleId; } }

        public PlayFabService(Game game, string playFabTitleId) : base(game)
        {
            _playFabTitleId = playFabTitleId;

            PlayFabSettings.staticSettings.TitleId = _playFabTitleId;

            Game.Components.Add(this);
            Game.Services.AddService<IPlayFabService>(this);
        }

        public async Task<IAuthenticationContext> Register(IRegistrationModel model)
        {
            RegisterPlayFabUserRequest rpur = new RegisterPlayFabUserRequest()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Password = model.Password,
                TitleId = PlayFabTitleId,
                Username = model.UserName,
                RequireBothUsernameAndEmail = model.RequireBothUsernameAndEmail
            };

            PlayFabResult<RegisterPlayFabUserResult> registerResult = await PlayFabClientAPI.RegisterPlayFabUserAsync(rpur);

            if (registerResult.Error != null)
                throw new Exception(registerResult.Error.ErrorMessage);

            IAuthenticationContext pac = new AuthenticationContext(registerResult.Result.AuthenticationContext);

            return pac;
        }

        public async Task<IAuthenticationContext> Login(IRegistrationModel model, IAuthenticationContext auth = null)
        {
            PlayFabAuthenticationContext pac = null;

            if (auth != null)
            {
                pac = new PlayFabAuthenticationContext()
                {
                    ClientSessionTicket = auth.ClientSessionTicket,
                    EntityId = auth.EntityId,
                    EntityToken = auth.EntityToken,
                    EntityType = auth.EntityType,
                    PlayFabId = auth.PlayFabId
                };
            }

            PlayFabResult<LoginResult> loginResult = await PlayFabClientAPI.LoginWithPlayFabAsync(new LoginWithPlayFabRequest()
            {
                Username = model.UserName,
                Password = model.Password,
                TitleId = PlayFabTitleId,
                AuthenticationContext = pac != null ? pac : null,
            });

            if (loginResult.Error != null)
                throw new Exception(loginResult.Error.ErrorMessage);

            auth = new AuthenticationContext(loginResult.Result.AuthenticationContext);

            return auth;
        }

    }
}
