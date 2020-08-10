using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniHumanoid;
using System.Linq;
using VRM;
using UnityEngine.Animations;
using RootMotion.FinalIK;

public class EquipGuitar: MonoBehaviour {

    [SerializeField] GameObject avatar;

    // Start is called before the first frame update
    void Start() {
        Assert.IsNotNull(avatar, "Avatar is not set.");
        var spineBone = FindTarget(avatar, HumanBodyBones.Spine);
        Assert.IsNotNull(spineBone, "Spine is not found.");

        var vrik = avatar.GetComponent<VRIK>();
        Assert.IsNotNull(vrik, "VRIK is not set.");
        var leftHand = vrik.solver.leftArm.target;
        Assert.IsNotNull(leftHand, "Left hand is not found.");

        SetupConstraints(spineBone, leftHand);
    }

    private void SetupConstraints(Transform spineBone, Transform leftHandBone) {
        {
            var constraint = gameObject.AddComponent<AimConstraint>();
            var constraintSource = new ConstraintSource();
            constraintSource.sourceTransform = leftHandBone.transform;
            constraintSource.weight = 1;
            constraint.AddSource(constraintSource);
            constraint.constraintActive = true;
            
            constraint.aimVector = new Vector3(0, 0, 1);
            constraint.upVector = new Vector3(0, 1, 0);
            constraint.worldUpType = AimConstraint.WorldUpType.SceneUp;
            constraint.locked = true;
            constraint.rotationAtRest = new Vector3(0, 0, 0);
            constraint.rotationOffset = new Vector3(-0.264f, 86.67f, -4.629f);
            constraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;
        }

        {
            var constraint = gameObject.AddComponent<ParentConstraint>();
            var constraintSource = new ConstraintSource();
            constraintSource.sourceTransform = spineBone.transform;
            constraintSource.weight = 1;
            constraint.AddSource(constraintSource);
            constraint.constraintActive = true;
            constraint.rotationAxis = Axis.None;
            constraint.translationAtRest = new Vector3(0.0909f, 0.821f, 0.129f);

            constraint.locked = false;
            constraint.translationAtRest = new Vector3(0.091f, 0.821f, 0.129f);
            constraint.rotationAtRest = new Vector3(0, 0, 0);
            constraint.SetTranslationOffset(0, new Vector3(0.0907f, 0.03f, 0.04f));
            constraint.SetRotationOffset(0, new Vector3(0, 0, 0));
            constraint.translationAxis = Axis.X | Axis.Y | Axis.Z;
            constraint.rotationAxis = Axis.None;
        }

        {
            var constraint = leftHandBone.gameObject.AddComponent<RotationConstraint>();
            var constraintSource = new ConstraintSource();
            constraintSource.sourceTransform = transform;
            constraintSource.weight = 1;
            constraint.AddSource(constraintSource);
            constraint.constraintActive = true;

            constraint.rotationOffset = new Vector3(200, 90, -75);
        }
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
