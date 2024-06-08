using System.Linq;
using cc.dingemans.bigibas123.MaterialDedup.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace cc.dingemans.bigibas123.MaterialDedup.Editor
{
	[CustomEditor(typeof(MaterialDeduplicatorBehavior))]
	public class MaterialDeduplicatorBehaviorEditor : UnityEditor.Editor
	{
		public override VisualElement CreateInspectorGUI()
		{
			VisualElement inspector = new VisualElement();
			var list = ((MaterialDeduplicatorBehavior)target).AsMaterialRefs().AsDedupList().ToList();
			
			list.ForEach(dedup =>
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
					var rendObjField = new ObjectField() { focusable = false, objectType = typeof(Renderer) };
					rendObjField.label = dest.Slot.ToString();
					rendObjField.value = dest.Renderer;
					gb.contentContainer.Add(rendObjField);
				}
				rendererFold.contentContainer.Add(gb);
				inspector.Add(rendererFold);
			});

			
			return inspector;
		}
	}
}