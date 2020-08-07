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
        var guitar = gameObject.transform.Find("guitar").gameObject;
        var avatar = gameObject.transform.Find("avatar").gameObject;
        Assert.IsNotNull(guitar, "Guitar is not found");
        Assert.IsNotNull(avatar, "Avatar is not found");

        var humanoidDesc = avatar.GetComponent<VRMHumanoidDescription>();
        Assert.IsNotNull(humanoidDesc, "VRMHumanoidDescription is not found");

        SetupConnectionPoint(humanoidDesc, guitar);
    }

    private void SetupConnectionPoint(VRMHumanoidDescription humanoidDesc, GameObject guitar) {
        GameObject bodySideConnectionPoint;
        {
            var boneLimits = humanoidDesc.Description.human;
            var hip = FindTarget(boneLimits, HumanBodyBones.Hips);
            // var cube = AddTargetCube(hip);
            // cube.name = "ConnectionPointBodyA";
            // cube.transform.position = new Vector3(
            //     cube.transform.position.x,
            //     cube.transform.position.y + 0.04f,
            //     cube.transform.position.z + 0.2f
            // );
            // bodySideConnectionPoint = cube;
            bodySideConnectionPoint = hip.gameObject;
        }
        Assert.IsNotNull(bodySideConnectionPoint);

        GameObject guitarSideConnectionPoint;
        {
            // var guitarBody = FindDescendantsRecursively(guitar.transform, "body");
            // var cube = AddTargetCube(guitarBody);
            // cube.transform.localScale *= 100;
            // cube.name = "ConnectionPointGuitarA";
            // cube.transform.position = new Vector3(
            //     cube.transform.position.x - 0.04f,
            //     cube.transform.position.y,
            //     cube.transform.position.z
            // );
            // guitarSideConnectionPoint = cube;

            guitarSideConnectionPoint = guitar;
        }
        Assert.IsNotNull(guitarSideConnectionPoint);

        var constraintSource = new ConstraintSource();
        constraintSource.sourceTransform = bodySideConnectionPoint.transform;
        constraintSource.weight = 1;
        var positionConstraint = guitarSideConnectionPoint.AddComponent<ParentConstraint>();
        {
            positionConstraint.AddSource(constraintSource);
            positionConstraint.SetRotationOffset(0, new Vector3(-30, -90, 0));
            positionConstraint.SetTranslationOffset(0, new Vector3(-0.1f, 0.1f, 0));
        }
        positionConstraint.constraintActive = true;
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
