using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGamePlayFab.Scene.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Scene
{
    public class MainMenuScene : SceneWithFadeBase
    {
        protected SpriteFont versionFont;
        protected const string version = "Version: [1.0.0.0]";
        Vector2 versionPosition = Vector2.Zero;

        public MainMenuScene(Game game, string name) : base(game, name) { }

        public override void Initialize()
        {
            versionFont = Game.Content.Load<SpriteFont>("Fonts/versionFont");
            Vector2 vm = versionFont.MeasureString(version);
            versionPosition = new Vector2(Game.GraphicsDevice.Viewport.Width - (vm.X + 8), Game.GraphicsDevice.Viewport.Height - (vm.Y + 4));
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

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
