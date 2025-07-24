//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;

//public static class SoftbodyMeshGenerator
//{
//    private const float PositionTolerance = 0.001f;
//    private const string OutputFolder = "Assets/GeneratedMeshes";

//    [MenuItem("Tools/Generate Softbody Mesh From Selected")]
//    public static void GenerateSoftbodyMesh()
//    {
//        var selected = Selection.activeGameObject;
//        if (selected == null)
//        {
//            Debug.LogError("No GameObject selected. Please select a GameObject with SkinnedMeshRenderer.");
//            return;
//        }

//        var smr = selected.GetComponent<SkinnedMeshRenderer>();
//        if (smr == null || smr.sharedMesh == null)
//        {
//            Debug.LogError("Selected GameObject does not have a SkinnedMeshRenderer with a valid mesh.");
//            return;
//        }

//        var originalMesh = smr.sharedMesh;

//        // Ensure output folder exists
//        if (!AssetDatabase.IsValidFolder(OutputFolder))
//        {
//            AssetDatabase.CreateFolder("Assets", "GeneratedMeshes");
//        }

//        // Duplicate mesh asset
//        var meshCopy = Object.Instantiate(originalMesh);
//        var assetPath = Path.Combine(OutputFolder, originalMesh.name + "_Softbody.asset");
//        AssetDatabase.CreateAsset(meshCopy, assetPath);

//        // Compute unique vertex positions
//        var originalVertices = meshCopy.vertices;
//        var uniquePositions = new List<Vector3>();
//        var vertexToBone = new int[originalVertices.Length];

//        for (int i = 0; i < originalVertices.Length; i++)
//        {
//            var v = originalVertices[i];
//            int boneIndex = -1;
//            for (int j = 0; j < uniquePositions.Count; j++)
//            {
//                if (Vector3.Distance(uniquePositions[j], v) <= PositionTolerance)
//                {
//                    boneIndex = j;
//                    break;
//                }
//            }

//            if (boneIndex < 0)
//            {
//                boneIndex = uniquePositions.Count;
//                uniquePositions.Add(v);
//            }

//            vertexToBone[i] = boneIndex;
//        }

//        // Create bones hierarchy under selected
//        var root = new GameObject(originalMesh.name + "_Softbody_Root");
//        root.transform.SetParent(selected.transform, false);
//        var boneTransforms = new Transform[uniquePositions.Count];

//        for (int b = 0; b < uniquePositions.Count; b++)
//        {
//            var boneGO = new GameObject("Bone_" + b);
//            boneGO.transform.SetParent(root.transform, false);
//            // Position bone at cluster center
//            boneGO.transform.localPosition = uniquePositions[b];
//            boneTransforms[b] = boneGO.transform;
//        }

//        // Assign weights
//        var weights = new BoneWeight[originalVertices.Length];
//        for (int i = 0; i < weights.Length; i++)
//        {
//            var w = new BoneWeight
//            {
//                boneIndex0 = vertexToBone[i],
//                weight0 = 1f
//            };
//            weights[i] = w;
//        }
//        meshCopy.boneWeights = weights;

//        // Setup bind poses (mesh local space to bone space)
//        var bindPoses = new Matrix4x4[uniquePositions.Count];
//        var meshTransform = selected.transform;
//        for (int b = 0; b < boneTransforms.Length; b++)
//        {
//            var bone = boneTransforms[b];
//            // bindPose = inverse(boneMatrix) * meshTransformMatrix
//            bindPoses[b] = bone.worldToLocalMatrix * meshTransform.localToWorldMatrix;
//        }
//        meshCopy.bindposes = bindPoses;

//        // Assign new mesh and bones to SkinnedMeshRenderer
//        smr.sharedMesh = meshCopy;
//        smr.bones = boneTransforms;
//        smr.rootBone = root.transform;

//        AssetDatabase.SaveAssets();
//        Debug.Log($"Softbody mesh generated and saved to '{assetPath}'. Bones created: {uniquePositions.Count}.");
//    }
//}
