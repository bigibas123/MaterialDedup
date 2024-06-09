using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace cc.dingemans.bigibas123.MaterialDedup.Runtime
{
	[AddComponentMenu("MaterialDeduplicator")]
	public class MaterialDeduplicatorBehavior : MonoBehaviour, IEditorOnly
	{
		public bool replaceEvenIfOnlyOne;
		public List<Renderer> Renderers => GetRenderers(gameObject);

		private static List<Renderer> GetRenderers(GameObject root)
		{
			var renderers = new List<Renderer>();
			renderers.AddRange(root.GetComponentsInChildren<SkinnedMeshRenderer>());
			renderers.AddRange(root.GetComponentsInChildren<MeshRenderer>());
			return renderers;
		}
	}
}