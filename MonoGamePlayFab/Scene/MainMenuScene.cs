using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGamePlayFab.Models.Coroutine;
using MonoGamePlayFab.Scene.BaseClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Scene
{
    public class MainMenuScene : SceneWithFadeBase
    {
        protected SpriteFont versionFont;
        protected const string version = "Version: [1.0.0.0]";
        Vector2 versionPosition = Vector2.Zero;

        Texture2D bgImage;
        Random rnd;

        public MainMenuScene(Game game, string name) : base(game, name) { }

        public override void Initialize()
        {
            versionFont = Game.Content.Load<SpriteFont>("Fonts/versionFont");
            Vector2 vm = versionFont.MeasureString(version);
            versionPosition = new Vector2(Game.GraphicsDevice.Viewport.Width - (vm.X + 8), Game.GraphicsDevice.Viewport.Height - (vm.Y + 4));

            rnd = new Random(1971);

            bgImage = Game.Content.Load<Texture2D>("Textures/BG1");
            //coroutineService.StartCoroutine(AnimateBG());
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        IEnumerator AnimateBG()
        {
            while (State != Enums.SceneStateEnum.Unloaded)
            {
                yield return new WaitForSeconds(Game,.125f);

                float ratio = (float)Game.GraphicsDevice.Viewport.Width / Game.GraphicsDevice.Viewport.Height;

                int h = 24;
                int w = (int)(h * ratio);

                Color[] c = new Color[w * h];
                Color tint = new Color(1f,.33f,0,1f);
                Vector4 tintC = tint.ToVector4();

                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        float r = (float)Math.Min(1, rnd.NextDouble() + 0);
                        c[x + y * w] = new Color(r * tintC.X, r * tintC.Y, r * tintC.Z, 1 * tintC.W);
                    }
                }

                bgImage = new Texture2D(Game.GraphicsDevice, w, h);
                bgImage.SetData(c);
            }
        }


        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (bgImage != null)
                _spriteBatch.Draw(bgImage, new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.DrawString(versionFont, version, versionPosition, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);


        }

        protected override void ActionAfterFadeIn()
        {
            
        }

        protected override void ActionAfterFadeOut()
        {
            
        }

        protected override void ClearEvents()
        {
            
        }
    }
}
