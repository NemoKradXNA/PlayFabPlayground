using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGamePlayFab.Scene.BaseClasses;
using System;
using System.Collections;

namespace MonoGamePlayFab.Scene
{
    public class SplashScene : SceneWithFadeBase
    {
        public TimeSpan SplashDuration = new TimeSpan(0, 0, 0, 3, 0);
        public string NextScene = "mainMenuScene";
        public SplashScene(Game game, string name) : base(game, name) { }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/playfablogo"), new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - 150, 0, 300, 300), Color.White);
            _spriteBatch.Draw(Game.Content.Load<Texture2D>("Textures/monoGameLog"), new Rectangle(Game.GraphicsDevice.Viewport.Width / 2 - 100, 228, 200, 200), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void ClearEvents()
        {

        }

        protected override void ActionAfterFadeIn()
        {
            coroutineService.StartCoroutine(SplashTimer());
        }
        protected override void ActionAfterFadeOut()
        {
            
        }


        protected virtual IEnumerator SplashTimer()
        {
            TimeSpan start = DateTime.Now.TimeOfDay;
           

            while (kbManager.KeysPressed().Length == 0 && !msManager.LeftButtonDown)
            {
                yield return new WaitForEndOfFrame(Game);

                if (DateTime.Now.TimeOfDay - start > SplashDuration)
                    break;
            }

            sceneManager.LoadScene(NextScene);
        }
    }
}
