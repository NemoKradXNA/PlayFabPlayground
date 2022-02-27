using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System;
using System.Collections;

namespace MonoGamePlayFab.Scene.BaseClasses
{
    public abstract class SceneWithFadeBase : SceneBase
    {
        protected SpriteBatch _spriteBatch;

        protected Texture2D fader;
        protected Color fadeColor = Color.Black;

        public SceneWithFadeBase(Game game, string name) : base(game, name) { }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);



            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (State != SceneStateEnum.Loaded)
                _spriteBatch.Draw(fader, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), fadeColor);

            _spriteBatch.End();
        }

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            fader = new Texture2D(Game.GraphicsDevice, 1, 1);
            fader.SetData(new Color[] { Color.White });

            base.Initialize();
        }

        public override void LoadScene()
        {
            base.LoadScene();
            coroutineService.StartCoroutine(FadeIn());
        }

        public override void UnloadScene()
        {
            base.UnloadScene();

            ClearEvents();

            coroutineService.StartCoroutine(FadeOut());
        }

        protected abstract void ClearEvents();
        protected abstract void ActionAfterFadeIn();
        protected abstract void ActionAfterFadeOut();

        protected virtual IEnumerator FadeIn()
        {
            byte a = 255;
            byte fadeSpeed = 4;
            fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

            while (a > 0)
            {
                yield return new WaitForEndOfFrame(Game);
                a = (byte)Math.Max(0, a - fadeSpeed);
                fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

                
                audioManager.MusicVolume = 1f - (a / 255f);
            }

            State = SceneStateEnum.Loaded;

            ActionAfterFadeIn();
        }

        protected virtual IEnumerator FadeOut()
        {
            byte a = 0;
            byte fadeSpeed = 4;
            fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

            while (a < 255)
            {
                yield return new WaitForEndOfFrame(Game);
                a = (byte)Math.Min(255, a + fadeSpeed);
                fadeColor = new Color(fadeColor.R, fadeColor.G, fadeColor.B, a);

                audioManager.MusicVolume = 1f - (a / 255f);
            }

            State = SceneStateEnum.Unloaded;

            ActionAfterFadeOut();
        }
    }
}
