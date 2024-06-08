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
			foreach (var renderer in ((MaterialDeduplicatorBehavior)target).Renderers)
			{
				var rendererFold = new Foldout
				{
					text = renderer.name,
				};
				var container = rendererFold.contentContainer;

				var list = renderer.AsMaterialRefs().ToList();
				var listView = new ListView(list,
					makeItem: () => new GroupBox { focusable = false},
					bindItem: (elem, i) =>
					{
						var matRef = list[i];
						var gb = elem as GroupBox; 
						var of = new ObjectField(){focusable = false, objectType = typeof(Material)};
						of.label = matRef.Slot.ToString();
						of.value = matRef.Material;
						of.SetEnabled(false);
						gb.contentContainer.Add(of);
					}
				);
				container.Add(listView);


				inspector.Add(rendererFold);
			}

			return inspector;
		}
	}
}