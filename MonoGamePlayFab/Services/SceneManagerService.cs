using Microsoft.Xna.Framework;
using MonoGame.Randomchaos.Services.Coroutine;
using MonoGame.Randomchaos.Services.Coroutine.Models;
using MonoGame.Randomchaos.Services.Interfaces;
using MonoGame.Randomchaos.Services.Interfaces.Enums;
using System.Collections;
using System.Collections.Generic;

namespace MonoGamePlayFab.Services
{
    public class SceneManagerService : ISceneService
    {
        ICoroutineService coroutineService { get { return Game.Services.GetService<CoroutineService>(); } }

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

            Game.Services.AddService(typeof(ISceneService), this);
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
