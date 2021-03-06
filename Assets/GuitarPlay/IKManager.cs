﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using VRM;
using UniHumanoid;
using System.Linq;
using RootMotion.FinalIK;

public class IKManager: MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        var humanoidDesc = gameObject.GetComponent<VRMHumanoidDescription>();
        Assert.IsNotNull(humanoidDesc);
        var vrik = gameObject.AddComponent<VRIK>();
        SetupVRIK(vrik);

        SetupTargets(humanoidDesc, vrik);
        Debug.Log("VRIK is ready.");
    }

    void SetupVRIK(VRIK vrik) {
        vrik.AutoDetectReferences();
        
        vrik.solver.leftLeg.positionWeight = 0.99f;
        vrik.solver.leftLeg.rotationWeight = 0.99f;
        vrik.solver.rightLeg.positionWeight = 0.99f;
        vrik.solver.rightLeg.rotationWeight = 0.99f;

        vrik.solver.leftLeg.swivelOffset = -35;
        vrik.solver.rightLeg.swivelOffset = 35;

        vrik.solver.leftArm.swivelOffset = 23;
        vrik.solver.rightArm.swivelOffset = -23;

        vrik.solver.spine.pelvisPositionWeight = 0.99f;
        vrik.solver.spine.pelvisRotationWeight = 0.99f;

        vrik.solver.locomotion.weight = 0;
    }

    void SetupTargets(VRMHumanoidDescription humanoidDesc, VRIK vrik) {
        var boneLimits = humanoidDesc.Description.human;
        var solver = vrik.solver;

        {
            var bodyPart = HumanBodyBones.Head;
            var head = FindTarget(boneLimits, bodyPart);
            var leftEye = FindTarget(boneLimits, HumanBodyBones.LeftEye);
            if(head != null) {
                var cube = AddTargetCube(head);
                cube.transform.position = new Vector3(
                    cube.transform.position.x,
                    leftEye.transform.position.y,
                    cube.transform.position.z
                );
                cube.name = string.Format("{0}_target", bodyPart);
                solver.spine.headTarget = cube.transform;
            } else {
                Debug.LogFormat("{0} is not found.", bodyPart);
            }
        }

        {
            var bodyPart = HumanBodyBones.LeftHand;
            var bone = FindTarget(boneLimits, bodyPart);
            if(bone != null) {
                var cube = AddTargetCube(bone);
                cube.name = string.Format("{0}_target", bodyPart);
                solver.leftArm.target = cube.transform;
            } else {
                Debug.LogFormat("{0} is not found.", bodyPart);
            }
        }

        {
            var bodyPart = HumanBodyBones.RightHand;
            var bone = FindTarget(boneLimits, bodyPart);
            if(bone != null) {
                var cube = AddTargetCube(bone);
                cube.name = string.Format("{0}_target", bodyPart);
                solver.rightArm.target = cube.transform;
            } else {
                Debug.LogFormat("{0} is not found.", bodyPart);
            }
        }

        {
            var bodyPart = HumanBodyBones.LeftFoot;
            var foot = FindTarget(boneLimits, bodyPart);
            var toe = FindTarget(boneLimits, HumanBodyBones.LeftToes);
            if(foot != null) {
                var cube = AddTargetCube(foot);
                cube.name = string.Format("{0}_target", bodyPart);
                cube.transform.position = new Vector3(
                    cube.transform.position.x,
                    cube.transform.position.y,
                    toe.transform.position.z
                );
                solver.leftLeg.target = cube.transform;
            } else {
                Debug.LogFormat("{0} is not found.", bodyPart);
            }
        }

        {
            var bodyPart = HumanBodyBones.RightFoot;
            var foot = FindTarget(boneLimits, bodyPart);
            var toe = FindTarget(boneLimits, HumanBodyBones.RightToes);
            if(foot != null) {
                var cube = AddTargetCube(foot);
                cube.name = string.Format("{0}_target", bodyPart);
                cube.transform.position = new Vector3(
                    cube.transform.position.x,
                    cube.transform.position.y,
                    toe.transform.position.z
                );
                solver.rightLeg.target = cube.transform;
            } else {
                Debug.LogFormat("{0} is not found.", bodyPart);
            }
        }

        {
            var bodyPart = HumanBodyBones.Hips;
            var body = FindTarget(boneLimits, bodyPart);
            if (body != null) {
                var cube = AddTargetCube(body);
                cube.name = string.Format("{0}_target", bodyPart);
                solver.spine.pelvisTarget = cube.transform;
            } else {
                Debug.LogFormat("{0} is not found.", bodyPart);
            }
        }
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
        // var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var cube = new GameObject();

        cube.transform.SetParent(transform.parent);
        cube.transform.position = bodyPart.transform.position;
        cube.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);

        return cube;
    }

    // Update is called once per frame
    void Update() {

    }
}
