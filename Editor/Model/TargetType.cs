using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Model
{
	public enum TargetType
	{
		Renderer,
		AnimationClip,
	}

	public static class TargetTypeExtensions
	{
		public static TargetType GetTargetTypeFrom(this Type ob)
		{
			if (typeof(Renderer).IsAssignableFrom(ob))
			{
				return TargetType.Renderer;
			}
			if (typeof(AnimationClip).IsAssignableFrom(ob))
			{
				return TargetType.AnimationClip;
			}

			throw new ArgumentOutOfRangeException(nameof(ob), ob, "Enum not implemented");
		}
		
		public static bool IsTargetTypeSupported(this Type ob)
		{
			try
			{
				ob.GetTargetTypeFrom();
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				return false;
			}
		}

		public static bool ObjectIsType(this TargetType tt, Object ob)
		{
			return tt switch
			{
				TargetType.Renderer => ob is Renderer,
				TargetType.AnimationClip => ob is AnimationClip,
				_ => throw new ArgumentOutOfRangeException(nameof(tt), tt, "Enum not implemented")
			};
		}
	}
}