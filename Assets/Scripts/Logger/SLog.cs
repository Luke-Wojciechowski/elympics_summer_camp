using UnityEngine;

namespace Ingame
{
	public static class SLog
	{
		public static bool enabled = true;
		
		public static void Msg(object message, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.Log(message, context);
		}

		public static void Msg(object message, in object sender, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.Log($"[{sender.GetType().Name}] {message}", context);
		}

		public static void Msg(object message, in object sender, Color senderHighlightColor, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(senderHighlightColor)}>[{sender.GetType().Name}]</color> {message}", context);
		}
		
		public static void Msg(object message, in string sender, Color senderHighlightColor, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(senderHighlightColor)}>[{sender}]</color> {message}", context);
		}

		public static void Wrn(object message, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogWarning(message, context);
		}

		public static void Wrn(object message, in object sender, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogWarning($"[{sender.GetType().Name}] {message}", context);
		}
		
		public static void Wrn(object message, in object sender, Color senderHighlightColor, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGB(senderHighlightColor)}>[{sender.GetType().Name}]</color> {message}", context);
		}
		
		public static void Wrn(object message, in string sender, Color senderHighlightColor, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogWarning($"<color=#{ColorUtility.ToHtmlStringRGB(senderHighlightColor)}>[{sender}]</color> {message}", context);
		}
		
		public static void Err(object message, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogError(message, context);
		}

		public static void Err(object message, in object sender, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogError($"[{sender.GetType().Name}] {message}", context);
		}

		public static void Err(object message, in object sender, Color senderHighlightColor, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGB(senderHighlightColor)}>[{sender.GetType().Name}]</color> {message}", context);
		}
		
		public static void Err(object message, in string sender, Color senderHighlightColor, Object context = null)
		{
			if(!enabled)
				return;
			
			Debug.LogError($"<color=#{ColorUtility.ToHtmlStringRGB(senderHighlightColor)}>[{sender}]</color> {message}", context);
		}
	}
}