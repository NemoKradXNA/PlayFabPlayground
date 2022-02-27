using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Audio;
using MonoGame.Randomchaos.Services.Coroutine;
using MonoGame.Randomchaos.Services.Encryption;
using MonoGame.Randomchaos.Services.Input;
using MonoGame.Randomchaos.Services.Input.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGamePlayFab.Interfaces;
using MonoGamePlayFab.Scene;
using MonoGamePlayFab.Services;
using Newtonsoft.Json;
using PlayFabMonoGame.Interfaces;
using PlayFabMonoGame.Models;
using PlayFabMonoGame.Services;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamePlayFab
{
    

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        protected ICoroutineService coroutineService;
        protected IInputStateService inputHandlerService;
        protected IKeyboardStateManager kbManager;
        protected IMouseStateManager msManager;
        protected IAudioService audioManager;
        protected ISceneService sceneManager;

        public string PlayFabTitleId
        {
            get
            {
                return ConfigurationManager.AppSettings["PlayFabTitleId"];
            }
        }



        IEncryptionService encryptionService;

        IPlayFabService playFabService;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            encryptionService = new EncryptionService(this, Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["key"]), Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["iv"]));
            playFabService = new PlayFabService(this, PlayFabTitleId);
        }

       

        protected override void Initialize()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");

            coroutineService = new CoroutineService(this);
            audioManager = new AudioService(this);
            kbManager = new KeyboardStateManager(this);
            msManager = new MouseStateManager(this);
            inputHandlerService = new InputHandlerService(this, kbManager, msManager);

            sceneManager = new SceneManagerService(this);

            sceneManager.AddScene(new SplashScene(this, "splashScene"));
            sceneManager.AddScene(new MainMenuScene(this, "mainMenuScene"));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            sceneManager.LoadScene("splashScene");
           
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            //// TODO: Add your update logic here
            //if (F1Down && Keyboard.GetState().IsKeyUp(Keys.F1))
            //    Register("TestDude1", "tentsareAc3!").ConfigureAwait(false);

            //F1Down = Keyboard.GetState().IsKeyDown(Keys.F1);

            inputHandlerService.PreUpdate(gameTime);
            base.Update(gameTime);
        }

        bool F1Down;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            base.EndDraw();

            coroutineService.UpdateEndFrame(null);
        }

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

        protected T LoadObjectFromJSONFile<T>(string file)
        {
            if (File.Exists(file))
            {
                string json = encryptionService.Decrypt(File.ReadAllBytes(file));
                //json = File.ReadAllText(file);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default(T);
        }

        protected void SaveObjectToJSOFile<T>(string file, T obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            File.WriteAllBytes(file, encryptionService.Encrypt(json));
        }

    }
}
