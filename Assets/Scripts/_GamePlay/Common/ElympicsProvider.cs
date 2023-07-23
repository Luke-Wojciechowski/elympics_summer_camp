using System;
using System.Collections;
using System.Collections.Generic;
using Elympics;
using Inagme;
using Ingame.DiInstallers;
using NaughtyAttributes;
using Secs;
using UnityEngine;
using Zenject;
using IInitializable = Elympics.IInitializable;

namespace Ingame.Player
{
    public sealed class ElympicsProvider : ElympicsMonoBehaviour, IUpdatable
    {
        public Action TickableAction;
        public event Action OnGameDone;

        [SerializeReference, Required] private ElympicsPlayerProvider player1;
        [SerializeReference, Required] private ElympicsPlayerProvider player2;
        
        private EcsWorld _ecsWorld;
        private ISceneService _sceneService;
        private readonly int _winsToWin = 3;
        
        #region ElympicsVar
        
        [HideInInspector] public ElympicsInt playerAWins = new ElympicsInt(0);
        [HideInInspector] public ElympicsInt playerBWins = new ElympicsInt(0);
        public ElympicsBool blockInput = new ElympicsBool(true);
        
        public ElympicsArray<ElympicsBool> ready = new ElympicsArray<ElympicsBool>(2, () => new ElympicsBool(false));
        #endregion
        
        public int WinsToWin => _winsToWin;
        
        public ElympicsPlayerProvider Player1 => player1;

        public ElympicsPlayerProvider Player2 => player2;
        
        [Inject] 
        private void Construct(GameplaySceneInstaller.EcsWorldProvider ecsWorldProvider, ISceneService sceneService)
        {
            _ecsWorld = ecsWorldProvider.ecsWorld;
            _sceneService = sceneService;
        }
        
        public void Start()
        {
            StartCoroutine(StartFightRoutine());
        }
        private void Awake()
        {
            playerAWins.ValueChanged += OnWinsChanged;
            playerBWins.ValueChanged += OnWinsChanged;
        }
        private void OnDestroy()
        {
            playerAWins.ValueChanged -= OnWinsChanged;
            playerBWins.ValueChanged -= OnWinsChanged;
        }
        
        public void ElympicsUpdate()
        {
            TickableAction?.Invoke();
        }
        

        private void OnWinsChanged(int oldValue, int newValue)
        {
           if(newValue != _winsToWin)
               return;
           
           OnGameDone?.Invoke();
        }
        
        private IEnumerator StartFightRoutine()
        {
            yield return new WaitUntil(()=> ready.Values[0].Value && ready.Values[1].Value);
            blockInput.Value = false;
        }
        
        public int? GetWinnerId()
        {
            if (playerAWins.Value >= _winsToWin) 
                return 1;
            
            if (playerBWins.Value >= _winsToWin) 
                return 2;
            
            return null;
        }

    }
}