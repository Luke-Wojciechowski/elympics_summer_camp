﻿using System;

namespace Secs
{
	[Serializable]
	public struct EcsConfig
	{
		public static EcsConfig Default => new EcsConfig
		{
			world = new WorldConfig
			{
				defaultWorldId = "unspecified_id",
				initialAllocatedEntities = 64,
				initialAllocatedEntityUpdateOperations = 32,
				initialAllocatedPools = 16,
				initialAllocatedFilters = 16
			},
			pool = new PoolConfig
			{
				initialAllocatedComponents = 16
			},
			filter = new FilterConfig
			{
				initialAllocatedEntities = 16
			}
		};

		public WorldConfig world;
		public PoolConfig pool;
		public FilterConfig filter;
		
		[Serializable]
		public struct WorldConfig
		{
			public string defaultWorldId;
			public int initialAllocatedEntities;
			public int initialAllocatedEntityUpdateOperations;
			public int initialAllocatedPools;
			public int initialAllocatedFilters;
		}
		
		[Serializable]
		public struct PoolConfig
		{
			public int initialAllocatedComponents;
		}
		
		[Serializable]
		public struct FilterConfig
		{
			public int initialAllocatedEntities;
		}
	}
}