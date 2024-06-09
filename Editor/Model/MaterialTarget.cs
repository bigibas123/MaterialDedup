using UnityEngine;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor.Model
{
	public abstract class MaterialTarget : MaterialContainer
	{
		public abstract TargetType Type { get; }
		public abstract Object Target { get; }
		public abstract void SetNewMat(Material mat);
	}
}