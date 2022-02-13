using Microsoft.Xna.Framework;
using MonoGamePlayFab.Enums;
using MonoGamePlayFab.Interfaces;
using MonoGamePlayFab.Models.Coroutine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGamePlayFab.Services
{
    public class SceneManagerService : ISceneManager
    {
        ICoroutineService coroutineService { get { return Game.Services.GetService<ICoroutineService>(); } }

        public IScene CurrentScene { get; set; }
        public SceneStateEnum CurrentSceneState
        {
            get
            {
                if (CurrentScene != null)
                    return CurrentScene.State;

                return SceneStateEnum.Unknown;
            }
        }


        public Dictionary<string, IScene> Scenes { get; set; }

        protected Game Game { get; set; }

        public SceneManagerService(Game game)
        {
            Game = game;
            Scenes = new Dictionary<string, IScene>();

            Game.Services.AddService(typeof(ISceneManager), this);
        }

        public void AddScene(IScene scene)
        {
            Scenes.Add(scene.Name, scene);
        }

        public void LoadScene(string name)
        {
            if (Scenes.ContainsKey(name))
                coroutineService.StartCoroutine(LoadScene(Scenes[name]));
        }


        protected IEnumerator LoadScene(IScene scene)
        {
            if (CurrentScene != null)
            {
                CurrentScene.State = SceneStateEnum.Unloading;
                CurrentScene.UnloadScene();

                while (CurrentScene.State != SceneStateEnum.Unloaded)
                    yield return new WaitForEndOfFrame(Game);
            }

            CurrentScene = scene;
            CurrentScene.State = SceneStateEnum.Loading;
            scene.LoadScene();
        }
    }
}
