using Zenject;

namespace Secs
{
	public sealed partial class EcsSystems
	{
		/// <summary>
		/// Injects ECS types into all systems that were added
		/// </summary>
		public void Inject()
		{
			foreach(var ecsSystem in _allSystems) 
				_world.Inject(ecsSystem);
		}
		
		public void Inject(DiContainer diContainer)
		{
			foreach(var ecsSystem in _allSystems)
			{
				_world.Inject(ecsSystem);
				diContainer.Inject(ecsSystem);
			}
		}
	}
}