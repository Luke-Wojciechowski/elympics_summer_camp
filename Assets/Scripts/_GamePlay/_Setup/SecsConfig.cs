using System;
using Secs;
using UnityEngine;

namespace Ingame.Gameplay
{
	[CreateAssetMenu(fileName = "SecsConfig", menuName = "Configs/SecsConfig")]

	public sealed class SecsConfig : ScriptableObject
	{
		[SerializeField] private EcsConfig ecsConfig;

		public EcsConfig EcsConfig => ecsConfig;
	}
}