using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Model
{
	public enum TargetType
	{
		Static,
		Skinned,
		AnimationClip,
		Trail
	}

	public static class TargetTypeExtensions
	{
		public static TargetType GetTargetTypeFrom(this Type ob)
		{
			if (ob  == typeof(SkinnedMeshRenderer))
			{
				return TargetType.Skinned;
			}

			if (ob == typeof(MeshRenderer))
			{
				return TargetType.Static;
			}

			if (ob == typeof(AnimationClip))
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
				TargetType.Static => ob is MeshRenderer,
				TargetType.Skinned => ob is SkinnedMeshRenderer,
				TargetType.AnimationClip => ob is AnimationClip,
				_ => throw new ArgumentOutOfRangeException(nameof(tt), tt, "Enum not implemented")
			};
		}
	}
}