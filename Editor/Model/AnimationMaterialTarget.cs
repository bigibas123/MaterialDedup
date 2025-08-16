using UnityEditor;
using UnityEngine;

namespace cc.dingemans.bigibas123.materialdedup.Editor.Model
{
	public class AnimationMaterialTarget : MaterialTarget
	{
		private readonly AnimationClip _clip;
		private readonly EditorCurveBinding _binding;
		private readonly int _keyFrameId;

		public AnimationMaterialTarget(AnimationClip clip, EditorCurveBinding binding, int keyFrame)
		{
			_clip = clip;
			_binding = binding;
			_keyFrameId = keyFrame;
		}

		private ObjectReferenceKeyframe Keyframe =>
			AnimationUtility.GetObjectReferenceCurve(_clip, _binding)[_keyFrameId];

		public override Material Material => Keyframe.value as Material;
		public override string Name => $"{_clip.name}:{_binding.propertyName}[{_keyFrameId}]";

		public override TargetType Type => TargetType.AnimationClip;
		public override Object Target => _clip;

		public override void SetNewMat(Material mat)
		{
			//TODO check if this is destructive
			var curve = AnimationUtility.GetObjectReferenceCurve(_clip, _binding);
			curve[_keyFrameId].value = mat;
			AnimationUtility.SetObjectReferenceCurve(_clip, _binding, curve);
		}
	}
}