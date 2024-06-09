using System.Collections.Generic;
using System.Linq;
using cc.dingemans.bigibas123.MaterialDedup.Editor.Model;
using cc.dingemans.bigibas123.MaterialDedup.Runtime;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC;
using VRC.SDK3.Avatars.Components;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor
{
	[CustomEditor(typeof(MaterialDeduplicatorBehavior))]
	public class MaterialDeduplicatorBehaviorEditor : UnityEditor.Editor
	{
		SerializedProperty _replaceEvenIfOnlyOneProp;

		[CanBeNull] private GroupBox _box;

		void OnEnable()
		{
			_replaceEvenIfOnlyOneProp =
				serializedObject.FindProperty(nameof(MaterialDeduplicatorBehavior.replaceEvenIfOnlyOne));
		}

		private void ReDrawMaterials(MaterialDeduplicatorBehavior behavior)
		{
			_box.contentContainer.Clear();
			var list = behavior
				.AsMaterialRefs(behavior.gameObject.GetComponentInParent<VRCAvatarDescriptor>())
				.AsDedupList();
			var matBlock = GetMaterialFoldouts(list, behavior.replaceEvenIfOnlyOne);
			foreach (var foldout in matBlock)
			{
				_box.contentContainer.Add(foldout);
			}
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement inspector = new VisualElement();
			var behavior = (MaterialDeduplicatorBehavior)target;

			var checkBox = new PropertyField(_replaceEvenIfOnlyOneProp, "Always replace materials");
			checkBox.RegisterValueChangeCallback(evt => { ReDrawMaterials(behavior); });

			inspector.Add(checkBox);

			var list = behavior
				.AsMaterialRefs(behavior.gameObject.GetComponentInParent<VRCAvatarDescriptor>())
				.AsDedupList();

			var matBlock = GetMaterialFoldouts(list, behavior.replaceEvenIfOnlyOne);
			_box ??= new GroupBox();
			foreach (var foldout in matBlock)
			{
				_box.contentContainer.Add(foldout);
			}
			inspector.Add(_box);

			return inspector;
		}

		public IEnumerable<Foldout> GetMaterialFoldouts(IEnumerable<DeduplicatedMaterial> list,
			bool replaceEvenIfOnlyOne)
		{
			return list
				.Where(dedup => replaceEvenIfOnlyOne || dedup.DestinationCount > 1)
				.ToList()
				.Select(dedup =>
				{
					var rendererFold = new Foldout
					{
						text = dedup.Name,
					};
					var gb = new GroupBox() { focusable = false };
					gb.SetEnabled(false);
					var of = new ObjectField() { focusable = false, objectType = typeof(Material) };
					of.value = dedup.Material;
					gb.contentContainer.Add(of);
					foreach (var dest in dedup.Destinations)
					{
						var rendObjField = new ObjectField
						{
							focusable = false,
							objectType = dest.Target.GetType(),
							label = dest.Name,
							value = dest.Target
						};
						gb.contentContainer.Add(rendObjField);
					}

					rendererFold.contentContainer.Add(gb);
					return rendererFold;
				});
		}
	}
}