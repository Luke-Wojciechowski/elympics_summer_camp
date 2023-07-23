using System.Collections;
using Elympics;
using Inagme;
using Zenject;
using IInitializable = Elympics.IInitializable;

namespace Ingame.Player
{
    public sealed class ElympicsPlayerProvider : ElympicsMonoBehaviour, IInputHandler, IUpdatable
    {

        private PlayerInput.Movement _movement;
        
        private IInputService _inputService;
        private ElympicsProvider _elympicsProvider;
        
        public PlayerInput.Movement Movement => _movement;
        
        [Inject] 
        private void Construct(IInputService inputService, ElympicsProvider elympicsProvider)
        {
            _inputService = inputService;
            _elympicsProvider = elympicsProvider;
        }

        public void OnInputForClient(IInputWriter inputWriter)
        {
 
            if (Elympics.Player != PredictableFor)
                return;

            var movement = _inputService.CurrentPlayerInput.movement;
            
            inputWriter.Write((int)Elympics.Player);
            inputWriter.Write(movement.forward);
            inputWriter.Write(movement.backward);
            inputWriter.Write(movement.right);
            inputWriter.Write(movement.left);
            inputWriter.Write(movement.jump);
            inputWriter.Write(movement.dash);
        }

        public void OnInputForBot(IInputWriter inputSerializer)
        {
           
        }

        public void ElympicsUpdate()
        { 
            if (!ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
               return;
            
            inputReader.Read(out int player);
            inputReader.Read(out bool forward);
            inputReader.Read(out bool backward);
            inputReader.Read(out bool right);
            inputReader.Read(out bool left);
            inputReader.Read(out bool jump);
            inputReader.Read(out bool dash);

            _elympicsProvider.ready.Values[player].Value = true;
            
            _movement = new PlayerInput.Movement()
            {
                right = right,
                left = left,
                backward = backward,
                forward = forward,
                jump = jump,
                dash = dash
            };
               
        }
    }
}