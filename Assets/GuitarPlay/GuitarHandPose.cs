using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;
using UnityEngine.Assertions;
using System.Linq;
using UniHumanoid;

public class GuitarHandPose: MonoBehaviour {
    [SerializeField]
    private HandForm handForm = HandForm.CLike;

    private Transform leftIndex1;
    private Transform leftIndex2;
    private Transform leftIndex3;
    private Transform leftMiddle1;
    private Transform leftMiddle2;
    private Transform leftMiddle3;
    private Transform leftRing1;
    private Transform leftRing2;
    private Transform leftRing3;
    private Transform leftLittle1;
    private Transform leftLittle2;
    private Transform leftLittle3;
    private Transform leftThumb1;
    private Transform leftThumb2;
    private Transform leftThumb3;

    private Transform rightIndex1;
    private Transform rightIndex2;
    private Transform rightIndex3;
    private Transform rightMiddle1;
    private Transform rightMiddle2;
    private Transform rightMiddle3;
    private Transform rightRing1;
    private Transform rightRing2;
    private Transform rightRing3;
    private Transform rightLittle1;
    private Transform rightLittle2;
    private Transform rightLittle3;
    private Transform rightThumb1;
    private Transform rightThumb2;
    private Transform rightThumb3;

    // Start is called before the first frame update
    void Start() {
        var humanoidDesc = gameObject.GetComponent<VRMHumanoidDescription>();
        Assert.IsNotNull(humanoidDesc, "Humanoid Description is not set.");

        PrepareHandJoints(humanoidDesc);

        SetHandForm();
    }

    private void PrepareHandJoints(VRMHumanoidDescription humanoidDesc) {
        leftIndex1 = FindTarget(humanoidDesc, HumanBodyBones.LeftIndexProximal);
        leftIndex2 = FindTarget(humanoidDesc, HumanBodyBones.LeftIndexIntermediate);
        leftIndex3 = FindTarget(humanoidDesc, HumanBodyBones.LeftIndexDistal);

        leftMiddle1 = FindTarget(humanoidDesc, HumanBodyBones.LeftMiddleProximal);
        leftMiddle2 = FindTarget(humanoidDesc, HumanBodyBones.LeftMiddleIntermediate);
        leftMiddle3 = FindTarget(humanoidDesc, HumanBodyBones.LeftMiddleDistal);

        leftRing1 = FindTarget(humanoidDesc, HumanBodyBones.LeftRingProximal);
        leftRing2 = FindTarget(humanoidDesc, HumanBodyBones.LeftRingIntermediate);
        leftRing3 = FindTarget(humanoidDesc, HumanBodyBones.LeftRingDistal);

        leftLittle1 = FindTarget(humanoidDesc, HumanBodyBones.LeftLittleProximal);
        leftLittle2 = FindTarget(humanoidDesc, HumanBodyBones.LeftLittleIntermediate);
        leftLittle3 = FindTarget(humanoidDesc, HumanBodyBones.LeftLittleDistal);

        leftThumb1 = FindTarget(humanoidDesc, HumanBodyBones.LeftThumbProximal);
        leftThumb2 = FindTarget(humanoidDesc, HumanBodyBones.LeftThumbIntermediate);
        leftThumb3 = FindTarget(humanoidDesc, HumanBodyBones.LeftThumbDistal);

        rightIndex1 = FindTarget(humanoidDesc, HumanBodyBones.RightIndexProximal);
        rightIndex2 = FindTarget(humanoidDesc, HumanBodyBones.RightIndexIntermediate);
        rightIndex3 = FindTarget(humanoidDesc, HumanBodyBones.RightIndexDistal);

        rightMiddle1 = FindTarget(humanoidDesc, HumanBodyBones.RightMiddleProximal);
        rightMiddle2 = FindTarget(humanoidDesc, HumanBodyBones.RightMiddleIntermediate);
        rightMiddle3 = FindTarget(humanoidDesc, HumanBodyBones.RightMiddleDistal);

        rightRing1 = FindTarget(humanoidDesc, HumanBodyBones.RightRingProximal);
        rightRing2 = FindTarget(humanoidDesc, HumanBodyBones.RightRingIntermediate);
        rightRing3 = FindTarget(humanoidDesc, HumanBodyBones.RightRingDistal);

        rightLittle1 = FindTarget(humanoidDesc, HumanBodyBones.RightLittleProximal);
        rightLittle2 = FindTarget(humanoidDesc, HumanBodyBones.RightLittleIntermediate);
        rightLittle3 = FindTarget(humanoidDesc, HumanBodyBones.RightLittleDistal);

        rightThumb1 = FindTarget(humanoidDesc, HumanBodyBones.RightThumbProximal);
        rightThumb2 = FindTarget(humanoidDesc, HumanBodyBones.RightThumbIntermediate);
        rightThumb3 = FindTarget(humanoidDesc, HumanBodyBones.RightThumbDistal);
    }

    private Transform FindTarget(VRMHumanoidDescription humanoidDesc, HumanBodyBones bone) {
        var boneName = humanoidDesc.Description.human.First((_bone) => {
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

    // Update is called once per frame
    void Update() {
        if(Input.GetKey(KeyCode.Space)) {
            (string boneName, Transform bone)[] targets = {
                ("rightIndex1", rightIndex1),
                ("rightIndex2", rightIndex2),
                ("rightIndex3", rightIndex3),
                ("rightMiddle1", rightMiddle1),
                ("rightMiddle2", rightMiddle2),
                ("rightMiddle3", rightMiddle3),
                ("rightRing1", rightRing1),
                ("rightRing2", rightRing2),
                ("rightRing3", rightRing3),
                ("rightLittle1", rightLittle1),
                ("rightLittle2", rightLittle2),
                ("rightLittle3", rightLittle3),
                ("rightThumb1", rightThumb1),
                ("rightThumb2", rightThumb2),
                ("rightThumb3", rightThumb3),
            };

            string log = "";
            foreach((var boneName, var bone) in targets) {
                var local = bone.localRotation;
                log += string.Format(
                    "{0}.localRotation = new Quaternion({1}f, {2}f, {3}f, {4}f);\n",
                    boneName,
                    (float)((int)(local.x * 1000)) / 1000,
                    (float)((int)(local.y * 1000)) / 1000,
                    (float)((int)(local.z * 1000)) / 1000,
                    (float)((int)(local.w * 1000)) / 1000
                    );
            }
            Debug.Log(log);
        }
    }

    void OnValidate() {
        SetHandForm();
    }

    private void SetHandForm() {
        if (leftIndex3 == null) {
            return;
        }
        switch (handForm) {
            case HandForm.CLike:
                leftIndex1.localRotation = new Quaternion(0.086f, 0.062f, 0.58f, 0.807f);
                leftIndex2.localRotation = new Quaternion(0f, 0f, 0.33f, 0.943f);
                leftIndex3.localRotation = new Quaternion(0f, 0f, 0.683f, 0.729f);
                leftMiddle1.localRotation = new Quaternion(-0.039f, -0.049f, 0.735f, 0.674f);
                leftMiddle2.localRotation = new Quaternion(0f, 0f, 0.202f, 0.979f);
                leftMiddle3.localRotation = new Quaternion(0f, 0f, 0.551f, 0.834f);
                leftRing1.localRotation = new Quaternion(-0.067f, 0.044f, 0.832f, 0.548f);
                leftRing2.localRotation = new Quaternion(0f, 0f, 0.018f, 0.999f);
                leftRing3.localRotation = new Quaternion(0f, 0f, 0.359f, 0.933f);
                leftLittle1.localRotation = new Quaternion(-0.254f, -0.258f, 0.819f, 0.442f);
                leftLittle2.localRotation = new Quaternion(0f, 0f, -0.068f, 0.997f);
                leftLittle3.localRotation = new Quaternion(0f, 0f, 0.488f, 0.872f);
                leftThumb1.localRotation = new Quaternion(0f, 0.134f, 0f, 0.99f);
                leftThumb2.localRotation = new Quaternion(0f, 0f, 0f, 1f);
                leftThumb3.localRotation = new Quaternion(0f, 0f, 0f, 1f);
                break;
            case HandForm.DLike:
                leftIndex1.localRotation = new Quaternion(0.022f, 0.013f, 0.609f, 0.792f);
                leftIndex2.localRotation = new Quaternion(0f, 0f, 0.432f, 0.901f);
                leftIndex3.localRotation = new Quaternion(0f, 0f, 0.584f, 0.811f);
                leftMiddle1.localRotation = new Quaternion(-0.094f, -0.021f, 0.453f, 0.886f);
                leftMiddle2.localRotation = new Quaternion(0f, 0f, 0.556f, 0.83f);
                leftMiddle3.localRotation = new Quaternion(0f, 0f, 0.575f, 0.818f);
                leftRing1.localRotation = new Quaternion(-0.156f, 0.01f, 0.761f, 0.628f);
                leftRing2.localRotation = new Quaternion(0f, 0f, 0.15f, 0.988f);
                leftRing3.localRotation = new Quaternion(-0.025f, -0.017f, 0.643f, 0.765f);
                leftLittle1.localRotation = new Quaternion(-0.213f, -0.051f, 0.744f, 0.629f);
                leftLittle2.localRotation = new Quaternion(0f, 0f, 0.093f, 0.995f);
                leftLittle3.localRotation = new Quaternion(0f, 0f, 0.488f, 0.872f);
                leftThumb1.localRotation = new Quaternion(0f, 0.246f, 0f, 0.969f);
                leftThumb2.localRotation = new Quaternion(0.23f, 0f, 0f, 0.973f);
                leftThumb3.localRotation = new Quaternion(0f, -0.196f, 0f, 0.98f);
                break;
            case HandForm.ELike:
                leftIndex1.localRotation = new Quaternion(0.086f, 0.062f, 0.58f, 0.807f);
                leftIndex2.localRotation = new Quaternion(0f, 0f, 0.33f, 0.943f);
                leftIndex3.localRotation = new Quaternion(0f, 0f, 0.683f, 0.729f);
                leftMiddle1.localRotation = new Quaternion(-0.094f, -0.024f, 0.778f, 0.62f);
                leftMiddle2.localRotation = new Quaternion(0f, 0f, 0.271f, 0.962f);
                leftMiddle3.localRotation = new Quaternion(0f, 0f, 0.445f, 0.895f);
                leftRing1.localRotation = new Quaternion(-0.108f, 0.064f, 0.831f, 0.54f);
                leftRing2.localRotation = new Quaternion(0f, 0f, 0.202f, 0.979f);
                leftRing3.localRotation = new Quaternion(-0.027f, 0.078f, 0.324f, 0.942f);
                leftLittle1.localRotation = new Quaternion(-0.125f, -0.079f, 0.769f, 0.621f);
                leftLittle2.localRotation = new Quaternion(0f, 0f, 0.218f, 0.975f);
                leftLittle3.localRotation = new Quaternion(0.049f, 0.029f, 0.765f, 0.64f);
                leftThumb1.localRotation = new Quaternion(-0.074f, 0.088f, -0.01f, 0.993f);
                leftThumb2.localRotation = new Quaternion(0.016f, -0.011f, 0.032f, 0.999f);
                leftThumb3.localRotation = new Quaternion(0.08f, -0.055f, 0.156f, 0.982f);
                break;
            case HandForm.FLike:
                leftIndex1.localRotation = new Quaternion(-0.045f, 0.099f, 0.632f, 0.766f);
                leftIndex2.localRotation = new Quaternion(0f, 0f, 0.473f, 0.881f);
                leftIndex3.localRotation = new Quaternion(0f, 0f, 0.36f, 0.932f);
                leftMiddle1.localRotation = new Quaternion(-0.092f, 0f, 0.724f, 0.683f);
                leftMiddle2.localRotation = new Quaternion(0f, 0f, 0.132f, 0.991f);
                leftMiddle3.localRotation = new Quaternion(0f, 0f, 0.7f, 0.713f);
                leftRing1.localRotation = new Quaternion(-0.104f, 0.058f, 0.786f, 0.606f);
                leftRing2.localRotation = new Quaternion(0f, 0f, 0.134f, 0.99f);
                leftRing3.localRotation = new Quaternion(-0.044f, 0.085f, 0.642f, 0.76f);
                leftLittle1.localRotation = new Quaternion(-0.214f, -0.139f, 0.885f, 0.387f);
                leftLittle2.localRotation = new Quaternion(0f, 0f, -0.161f, 0.986f);
                leftLittle3.localRotation = new Quaternion(0f, 0f, 0.414f, 0.91f);
                leftThumb1.localRotation = new Quaternion(0f, 0.134f, 0f, 0.99f);
                leftThumb2.localRotation = new Quaternion(0f, 0f, 0f, 1f);
                leftThumb3.localRotation = new Quaternion(0f, 0f, 0f, 1f);
                break;
        }

        rightIndex1.localRotation = new Quaternion(0f, 0f, -0.222f, 0.974f);
        rightIndex2.localRotation = new Quaternion(0f, 0f, -0.472f, 0.881f);
        rightIndex3.localRotation = new Quaternion(0f, 0f, -0.398f, 0.917f);
        rightMiddle1.localRotation = new Quaternion(0f, 0f, -0.074f, 0.997f);
        rightMiddle2.localRotation = new Quaternion(0f, 0f, -0.12f, 0.992f);
        rightMiddle3.localRotation = new Quaternion(0f, 0f, -0.192f, 0.981f);
        rightRing1.localRotation = new Quaternion(-0.01f, -0.043f, -0.237f, 0.97f);
        rightRing2.localRotation = new Quaternion(0f, 0f, 0.015f, 0.999f);
        rightRing3.localRotation = new Quaternion(0f, 0f, -0.126f, 0.991f);
        rightLittle1.localRotation = new Quaternion(0.14f, -0.099f, -0.278f, 0.944f);
        rightLittle2.localRotation = new Quaternion(0f, 0f, -0.046f, 0.998f);
        rightLittle3.localRotation = new Quaternion(0f, 0f, -0.039f, 0.999f);
        rightThumb1.localRotation = new Quaternion(0.378f, 0.102f, 0.045f, 0.918f);
        rightThumb2.localRotation = new Quaternion(0.165f, -0.039f, -0.024f, 0.985f);
        rightThumb3.localRotation = new Quaternion(0.445f, 0.224f, -0.053f, 0.865f);
    }
}

public enum HandForm {
    CLike,
    DLike,
    ELike,
    FLike,
}