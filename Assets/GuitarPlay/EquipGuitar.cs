using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UniHumanoid;
using System.Linq;
using VRM;
using UnityEngine.Animations;

public class EquipGuitar: MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        var handGuide = FindDescendantsRecursively(transform, "handGuide")?.gameObject;
        var leftHand = FindDescendantsRecursively(transform.parent, "LeftHand_target")?.gameObject;
        Assert.IsNotNull(handGuide, "Hand guide is not found");
        Assert.IsNotNull(leftHand, "Left hand is not found");

        SetupLeftHand(leftHand, handGuide);
    }

    private void SetupLeftHand(GameObject leftHand, GameObject handGuide) {
        var constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = handGuide.transform;
        constraintSource.weight = 1;
        var constraint = leftHand.AddComponent<ParentConstraint>();
        {
            constraint.AddSource(constraintSource);
            constraint.SetTranslationOffset(0, new Vector3(0.0f, 0.03f, 0.0f));
            constraint.SetRotationOffset(0, new Vector3(180, 180, 0));
        }
        constraint.constraintActive = true;
    }

    private Transform FindTarget(BoneLimit[] boneLimits, HumanBodyBones bone) {
        var boneName = boneLimits.First((_bone) => {
            return _bone.humanBone == bone;
        }).boneName;

        return FindDescendantsRecursively(transform, boneName);
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

    // Update is called once per frame
    void Update() {

    }
}
