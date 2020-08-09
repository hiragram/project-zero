using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniHumanoid;
using System.Linq;
using VRM;
using UnityEngine.Animations;

public class EquipGuitar: MonoBehaviour {

    [SerializeField] GameObject avatar;

    // Start is called before the first frame update
    void Start() {
        Assert.IsNotNull(avatar, "Avatar is not set.");
        var spineBone = FindTarget(avatar, HumanBodyBones.Spine);
        // var leftHandBone = FindTarget(avatar, HumanBodyBones.LeftThumbProximal);
        var leftHandBone = FindTarget(avatar, HumanBodyBones.LeftHand);
        Assert.IsNotNull(spineBone, "Spine is not found.");
        Assert.IsNotNull(leftHandBone, "Left hand is not found.");

        SetupConstraints(spineBone, leftHandBone);
    }

    private void SetupConstraints(Transform spineBone, Transform leftHandBone) {
        // {
        //     var constraint = gameObject.AddComponent<AimConstraint>();
        //     var constraintSource = new ConstraintSource();
        //     constraintSource.sourceTransform = leftHandBone.transform;
        //     constraintSource.weight = 1;
        //     constraint.AddSource(constraintSource);
        //     constraint.constraintActive = true;
        // }

        // {
        //     var constraint = gameObject.AddComponent<ParentConstraint>();
        //     var constraintSource = new ConstraintSource();
        //     constraintSource.sourceTransform = spineBone.transform;
        //     constraintSource.weight = 1;
        //     constraint.AddSource(constraintSource);
        //     constraint.constraintActive = true;
        //     constraint.rotationAxis = Axis.None;
        //     constraint.translationAtRest = new Vector3(0.0909f, 0.821f, 0.129f);
        // }
    }

    private Transform FindTarget(GameObject avatar, HumanBodyBones bone) {
        var humanoidDesc = avatar.GetComponent<VRMHumanoidDescription>();
        Assert.IsNotNull(humanoidDesc, "VRMHumanoidDescription is not set.");
        
        var boneName = humanoidDesc.Description.human.First((_bone) => {
            return _bone.humanBone == bone;
        }).boneName;

        return FindDescendantsRecursively(avatar.transform, boneName);
    }

    private Transform FindDescendantsRecursively(Transform root, string boneName) {
        var found = root.Find(boneName);

        if(found != null) {
            return found;
        } else if(root.childCount == 0) {
            return null;
        } else {
            foreach(var child in root.GetChildren()) {
                var foundInChild = FindDescendantsRecursively(child, boneName);
                if(foundInChild != null) {
                    return foundInChild;
                } else {
                    continue;
                }
            }

            return null;
        }
    }

    private GameObject AddTargetCube(Transform bodyPart) {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.SetParent(bodyPart);
        cube.transform.position = bodyPart.transform.position;
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        return cube;
    }
}
