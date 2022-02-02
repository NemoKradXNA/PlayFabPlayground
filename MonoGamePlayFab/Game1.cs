using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGamePlayFab.Interfaces;
using MonoGamePlayFab.Services;
using Newtonsoft.Json;
using PlayFabMonoGame.Interfaces;
using PlayFabMonoGame.Models;
using PlayFabMonoGame.Services;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamePlayFab
{
    

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public string PlayFabTitleId
        {
            get
            {
                return ConfigurationManager.AppSettings["PlayFabTitleId"];
            }
        }

        public object CryptographicBuffer { get; private set; }
        byte[] _key { get { return Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["key"]); } }
        byte[] _iv { get { return Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["iv"]); } }


        IEncryptionService encryptionService;

        IPlayFabService playFabService;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            encryptionService = new EncryptionService(this, _key, _iv);
            playFabService = new PlayFabService(this, PlayFabTitleId);
        }

       

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

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

        bool F1Down;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            

            base.Draw(gameTime);
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
