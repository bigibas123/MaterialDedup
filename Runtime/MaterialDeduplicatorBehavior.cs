using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace cc.dingemans.bigibas123.MaterialDedup.Runtime
{
	[AddComponentMenu("MaterialDeduplicator")]
	public class MaterialDeduplicatorBehavior : MonoBehaviour, IEditorOnly
	{
		public List<Renderer> Renderers => GetRenderers(gameObject);
		
		private static List<Renderer> GetRenderers(GameObject root)
		{
			var materials = new List<Renderer>();
			materials.AddRange(root.GetComponentsInChildren<SkinnedMeshRenderer>());
			materials.AddRange(root.GetComponentsInChildren<MeshRenderer>());
			return materials;
		}
	}
}