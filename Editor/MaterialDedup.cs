using cc.dingemans.bigibas123.materialdedup.Editor;
using cc.dingemans.bigibas123.materialdedup.Runtime;
using nadena.dev.ndmf;
using nadena.dev.ndmf.vrchat;

[assembly: ExportsPlugin(typeof(MaterialDedup))]

namespace cc.dingemans.bigibas123.materialdedup.Editor
{
	public class MaterialDedup : Plugin<MaterialDedup>
	{
		public override string QualifiedName => "cc.dingemans.bigibas123.MaterialDedup.MaterialDedup";
		public override string DisplayName => "Material Deduplicator";

		public static readonly string TAG = "[MaterialDedup]";

		protected override void Configure()
		{
			InPhase(BuildPhase.Optimizing)
				.BeforePlugin("com.anatawa12.avatar-optimizer")
				.Run("Deduplicate materials", ctx =>
				{
					var roots = ctx.AvatarRootTransform.GetComponentsInChildren<MaterialDeduplicatorBehavior>(true);
					foreach (var root in roots)
					{
						root.AsMaterialRefs(ctx.VRChatAvatarDescriptor()).AsDedupList()
							.ForEach(d => d.ApplyToDests(root.replaceEvenIfOnlyOne));
						UnityEngine.Object.DestroyImmediate(root);
					}
				});
		}
	}
}