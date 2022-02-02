## Getting Started
So, you have a PlayFab account (free like me maybe) and you have created a Title tile on your Studio Dashboard, and like me, you want to integrate PlayFab into your MonoGame project.

First thing I did, once I set up my project was to have a browse of the NuGet packages for PlayFab, you should find one called PlayFabAllSDK
![image](https://user-images.githubusercontent.com/2579338/152223873-58f50b45-213b-4acb-8c1a-325a5355046b.png)

Once installed I started to look through the documentation 
(you can find there is a link at the top of your dashboard to get you to the docs [here](https://docs.microsoft.com/en-us/gaming/playfab/))

![image](https://user-images.githubusercontent.com/2579338/152224222-bf20b314-a200-4469-9f4f-0bd76070f95f.png) 

and from there you can get to the quck start "Making your first API calls" [here](https://docs.microsoft.com/en-us/gaming/playfab/personas/developer)

![image](https://user-images.githubusercontent.com/2579338/152224609-6274ef74-3cd1-48ab-9ac2-1edceb84b3f7.png)

This opening page is great and tells you where to find your Title's ID, you will need this when calling the PlayFab API's

### Login & Authentication
I was hoping to see some guides, and there are, there are lots of ways you can log into and authenticate with PlayFab, taking a look at their 
[best practices page](https://docs.microsoft.com/en-us/gaming/playfab/features/authentication/login/login-basics-best-practices) 
you get an idea of all the options that are available. I decided to go with the WithPlayFab approach for now.

### Code
I decided to put my PlayFab API code into it's own project, so I created a MonoGame .Net Standard project called [PlayFabMonoGame](https://github.com/NemoKradXNA/PlayFabPlayground/tree/main/PlayFabMonoGame)
I have done this so it can be easily transported from one project to the next if I want it too, and, if this turns out to be quite useful, I will publish it as a NuGet
package for others to be able to use too.

### PlayFab as a Service
I know, I know, it's already a service, but I intent to put it in as a MonoGame service, this will mean that my components in my game project will be able to access my PlayFab
service at any time.

To do this I have set up a number of folders in my project, I dare say these will grow with time, I have only got as far as registering a player and then logging them in.

![image](https://user-images.githubusercontent.com/2579338/152227383-1591c511-c3b9-45cd-823f-ea5686f7914e.png)

### Interfaces
I love using interfaces in my projects, both t work and in my hobby, they make life so much easier when you want code that is easily extendable. 
For example, the current PlayFabservice I am writing it using the LogIn with Playfab approach, by using interfaces later I can swap out this service for
one that uses a different approach, maybe for IOs or Android, all Ill have to do in my game code, is keep the interface but use the new service that 
implements my service interface.

![image](https://user-images.githubusercontent.com/2579338/152227992-cfbfe094-99d6-41f7-a016-3a2782000633.png)

##### IPlayFabService
As you can see, I only have 3 interfaces at the time of writing, let's have a look at the IPlayFabService.

````cs
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
````
So, our limited service will allow us to have a read only PlayFabTitleId property and two methods, one to register a player and another to log them in. Again parameters for these methods are driven by interfaces.

#### IRegistrationModel
Objects that implement this interface will hold information needed to register a player.

````cs
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
````
#### IAuthenticationContext
Once logged in, objects implementing this interface will hold the data we can use to make other API calls into the PlayFab API for the player.

````cs
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
````
Some of you might ask, why have you created an interface for a class that already exists in the PlayFab API, well, I am trying to keep all the PlayFab objects in this project, so the calling project does not have to directly reference PlayFab, it may be in vein, but that's what I am trying to do with this.

### Models
So we have some interfaces, we need to also provide some models that implement them so we can start passing data to and from the calling application. Keep in mind, the calling application could create their own objects that implement these interfaces, or derive from the ones I provide.

#### RegistrationModel
````cs
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
````
#### AuthenticationContext
````cs
using PlayFab;
using PlayFabMonoGame.Interfaces;

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
````

### PlayFabService
Finally, we come to the service its self. Naturally this will grow over time, but here it is in the whole as at the time of writing

````cs
using Microsoft.Xna.Framework;
using PlayFab;
using PlayFab.ClientModels;
using PlayFabMonoGame.Interfaces;
using PlayFabMonoGame.Models;
using System;
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
````
As we can see, we are implementing the IPlayFabService interface, our constructor takes an additional parameter of playFabTitleId which will be your title Id, it then uses that to set the PlayFabSettings.staticSettings.TitleId, the documentation I saw to set this actually used PlayFabSettings.TitleId, but that is deprecated now, so I went with the suggested method. 

We can then implement the two methods that the interface requires, Register and LogIn, both on success return a AuthenticationContext, the calling project can then use those to pass to future service methods to make calls for a given player. I could store this within the service I guess, but I am thinking if you are running more than ne player off a single session (thinking maybe a beat-em up or a split screen racer?) you may want to make calls per player in the game rather than just manage one player.

### Service In Action
We now have a mechanism to register and log in a player, let's have a look at how this might work in action, in my test project, I have no UI at the moment, so I am just creating a mock user and passing it to the service when I hit the F1 key.

I have a property in my Game1 class that reads my game ID from a configuration file.
````cs
        public string PlayFabTitleId
        {
            get
            {
                return ConfigurationManager.AppSettings["PlayFabTitleId"];
            }
        }
````
I can then pass this onto the constructor of my service.
````cs
           playFabService = new PlayFabService(this, PlayFabTitleId);
`````
I then have in my Update method check to see if the F1 key is pressed (not down, pressed)
````cs
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (F1Down && Keyboard.GetState().IsKeyUp(Keys.F1))
                Register("TestDude1", "tentsareAc3!").ConfigureAwait(false);

            F1Down = Keyboard.GetState().IsKeyDown(Keys.F1);

            base.Update(gameTime);
        }
````
which calls a Register method, which in turn checks if we have a players details recorder (I am storing them in encrypted json files), if we do, we log them in, if we don't we register them, then log them in.
````cs
        protected async Task Register(string userName, string pwd)
        {
            if (File.Exists($"{userName}.json"))
            {
                // Log in
                await LogIn(userName,pwd);
            }
            else
            {
                await RegisterUser(userName, pwd);
                await LogIn(userName, pwd);
            }
        }
````
I will cover the encryption another time, it's not complicated, and I am sure you have your own methods. It is using the C# intrinsic Aes cryptography.

The RegisterUser method looks like this:
````cs
        protected async Task RegisterUser(string userName, string pwd)
        {
            try
            {
                IAuthenticationContext atuth = await playFabService.Register(new RegistrationModel() { UserName = userName, Password = pwd });

                if (atuth != null)
                    SaveObjectToJSOFile($"{userName}.json", atuth);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
````
and the LogIn method looks like this:
````cs
        protected async Task LogIn(string userName, string pwd)
        {
            try
            {
                IAuthenticationContext auth = LoadObjectFromJSONFile<AuthenticationContext>($"{userName}.json");
                auth = await playFabService.Login(new RegistrationModel() { UserName = userName, Password = pwd }, auth);
                SaveObjectToJSOFile($"{userName}.json", auth);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
````

Now, this is my first stab at this, so I am guessing some of you might well be jumping up and down shouting "NO!! NO! DONT DO IT LIKE THAT!", if that is the case, then let me know :D
