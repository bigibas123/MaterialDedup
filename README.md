# MaterialDedup
Scans the avatar for duplicate materials and replaces them with references to the same one, useful if you're using [VRCFury](https://vrcfury.com/) together with [AvatarOptimizer's Merge Skinned Mesh](https://vpm.anatawa12.com/avatar-optimizer/en/docs/reference/merge-skinned-mesh/).
Makes use of [ndmf](https://github.com/bdunderscore/ndmf), so all modifications are non-destructive. But still: Remember to take regular backups of your project.

# Usage
Add the MaterialDeduplicator component to the root and it will run when you build your avi.

The component also displays a prediction for how it will try to deduplicate the materials
This gets recalculated at build time due to other plugins making changes beforehand.

[VCC](https://bigibas123.github.io/VCC/)

## Background
This [ndmf](https://github.com/bdunderscore/ndmf) plugin is written as a replacement replicating the same functionality as [NdmfVRCFReorder](https://github.com/bigibas123/NdmfVRCFReorder)
in a way that isn't dependent on a bunch of reflection.

This will help it break less often and doesn't change the behavior of other plugins.

NdmfVRCFReorder was orignally written because VRCFury creates a seperate material for each skinned mesh,
which makes [AvatarOptimizers](https://github.com/anatawa12/AvatarOptimizer/)'s mesh join tool unable to merge the materials.
