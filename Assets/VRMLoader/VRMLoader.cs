using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using VRM;

public class VRMLoader: MonoBehaviour {
    [SerializeField] public string vrmPath = "";

    // Start is called before the first frame update
    async void Start() {
        StartCoroutine(DebugClock());
        {
            var vrmTransform = await LoadVRM();
            vrmTransform.SetParent(transform, false);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public async Task<Transform> LoadVRM() {
        var bytes = File.ReadAllBytes(vrmPath);
        var context = new VRMImporterContext();
        context.ParseGlb(bytes);

        var metadata = context.ReadMeta(false);
        Debug.LogFormat("meta: title: {0}", metadata.Title);

        await context.LoadAsyncTask();

        context.ShowMeshes();

        return context.Root.transform;
    }

    /********************************************/
    // Debug
    /********************************************/
    private int clockCount = 0;

    private IEnumerator DebugClock() {
        while(true) {
            clockCount++;
            Debug.Log(clockCount.ToString());
            yield return new WaitForSeconds(1f);
        }
    }
    /********************************************/
}
