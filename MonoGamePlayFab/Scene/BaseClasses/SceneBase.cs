﻿using Microsoft.Xna.Framework;
using MonoGamePlayFab.Enums;
using MonoGamePlayFab.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Scene.BaseClasses
{
    public abstract class SceneBase : DrawableGameComponent, IScene
    {
        public ISceneManager sceneManager { get { return Game.Services.GetService<ISceneManager>(); } }
        protected ICoroutineService coroutineService { get { return Game.Services.GetService<ICoroutineService>(); } }
        protected IKeyboardStateManager kbManager { get { return Game.Services.GetService<IInputStateHandler>().KeyboardManager; } }
        protected IMouseStateManager msManager { get { return Game.Services.GetService<IInputStateHandler>().MouseManager; } }

        protected IAudioManager audioManager { get { return Game.Services.GetService<IAudioManager>(); } }

        public string Name { get; set; }
        public IScene LastScene { get; set; }

        SceneStateEnum _state;
        public SceneStateEnum State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (_state == SceneStateEnum.Unloaded)
                {
                    Components.Clear();
                    Game.Components.Remove(this);
                }
            }
        }

        public List<IGameComponent> Components { get; set; }

        public SceneBase(Game game, string name) : base(game)
        {
            Name = name;
            Components = new List<IGameComponent>();
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (IGameComponent component in Components)
                component.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (State == SceneStateEnum.Unloaded)
                return;

            base.Update(gameTime);

            foreach (IGameComponent component in Components)
            {
                if (component is IUpdateable && ((IUpdateable)component).Enabled)
                    ((IUpdateable)component).Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (State == SceneStateEnum.Unloaded)
                return;

            foreach (IGameComponent component in Components)
            {
                if (component is IDrawable && ((IDrawable)component).Visible)
                    ((IDrawable)component).Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public virtual void LoadScene()
        {
            // Load our shit up!
            Game.Components.Add(this);
        }

        public virtual void UnloadScene()
        {
            // Unload our shit!
            UnloadContent();

        }
    }
}
