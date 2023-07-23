using System;
using System.Collections;
using System.Collections.Generic;
using Ingame.DiInstallers;
using Ingame.Player;
using NaughtyAttributes;
using Secs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Ingame.UI
{
    public sealed class UiWonRoundsController : MonoBehaviour
    {

        [SerializeReference, Required] private UiPlayerRoundsController player1Rounds;
        [SerializeReference, Required] private UiPlayerRoundsController player2Rounds;
        [SerializeReference, Required] private UiTextGameStatusController uiTextGameStatusController;
        
        private ElympicsProvider _provider;
        private EcsWorld _ecsWorld;

        [Inject]
        private void Construct(ElympicsProvider provider, GameplaySceneInstaller.EcsWorldProvider ecsWorldProvider)
        {
            _provider = provider;
            _ecsWorld = ecsWorldProvider.ecsWorld;
        }
        
        private void Awake()
        {
            _provider.playerAWins.ValueChanged += OnPlayerOneWin;
            _provider.playerBWins.ValueChanged += OnPlayerTwoWin;
            _provider.OnGameDone += OnGameDone;
 
        }
        
        private void OnDestroy()
        {
            _provider.playerAWins.ValueChanged -= OnPlayerOneWin;
            _provider.playerBWins.ValueChanged -= OnPlayerTwoWin; 
            _provider.OnGameDone -= OnGameDone;
        }
        
        private void OnPlayerOneWin(int a, int b)
        {
            ModifyView(player1Rounds, b,1);
        }
        private void OnPlayerTwoWin(int a, int b)
        {
            ModifyView(player2Rounds, b,2);
        }

        private void OnGameDone()
        {
            StartCoroutine(FinishGameRoutine());
        }

        private IEnumerator FinishGameRoutine()
        {
            var player = _provider.GetWinnerId();
            
            if(!player.HasValue)
                yield break;
            
            uiTextGameStatusController.SetText($"Player {player.Value} has won !!!");
            
            yield return !uiTextGameStatusController.IsRunning;
            yield return new WaitForSecondsRealtime(3.5f);
            
            _ecsWorld.NewEntityWithCmp<FinishGameEvt>(out _);
        }
        
        private void ModifyView(UiPlayerRoundsController controller, int value, int player)
        {
            StartCoroutine(FinishRoundRoutine(controller,value,player));
        }
        
        private IEnumerator FinishRoundRoutine(UiPlayerRoundsController controller, int value, int player)
        {
            controller.AddPoint(value);
            
            if(value >= _provider.WinsToWin)
                yield break;
            
            uiTextGameStatusController.SetText($"Player {player} has won the round !!!");
            
            yield return new WaitForSecondsRealtime(2.5f);
            
            uiTextGameStatusController.SetText($"Fight");
        }
        
    }
}