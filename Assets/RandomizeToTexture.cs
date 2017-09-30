using UnityEngine;
using System.Collections;

public class RandomizeToTexture : MonoBehaviour {

    public new Transform transform;
    public Texture2D targetTexture;
    public Camera renderCamera;
    public float maxPositionJump;
    public float maxScaleJump;
    public float maxRotationJump;
    public int maxBranchSteps;
    public int randsteps = 0;
    public float deflation;
    Vector3[] scales;
    Vector3[] rotations;
    Vector3[] positions;

    public RenderTexture rend;
    
    float distance = int.MaxValue;
    int success = 0;

	// Use this for initialization
	void Start ()
    {
        positions = new Vector3[transform.childCount];
        scales = new Vector3[transform.childCount];
        rotations = new Vector3[transform.childCount];
        StoreVars();
    }

    void Awake()
    {
        Start();
        new GameObject().AddComponent<Camera>().CopyFrom(renderCamera);

    }

    void OnPostRender()
    {
            if (positions.Length != transform.childCount)
                Start();
            for (int i = 0; i < randsteps; i++)
            {
                Randomize();
            }
            Capture();
            Compare();
    }
    int ggg = 0;
	void Randomize () {
        //for(int i = 0; i<transform.childCount; i++)
        ggg++;
        int i = ggg% transform.childCount; ;// Mathf.FloorToInt(Random.value * transform.childCount);
        {
            Transform t = transform.GetChild(i);
            t.localPosition += new Vector3(maxPositionJump*Random.value- maxPositionJump*.5f, maxPositionJump * Random.value- maxPositionJump*.5f, 0);
            t.localEulerAngles += Random.onUnitSphere *maxRotationJump;
            t.localScale += (Random.value *  maxScaleJump-maxScaleJump*.5f)*Vector3.one;


            Vector3 v = t.position;
            Vector3 distance = new Vector3(5,5,1);
            for (int j = 0; j < 3; j++)
            {
                while (v[j] < -distance[j] * .5f)
                    v[j] += distance[j] ;
                while (v[j] > distance[j] * .5f)
                    v[j] -= distance[j] ;
            }
            t.position = v;
            v = t.localScale;
            if (v.magnitude > 4) v = Vector3.one;
            t.localScale = v;
        }
	}
    Texture2D tex;
    void Capture()
    {
        if (rend == null || rend.width != targetTexture.width || rend.height != targetTexture.height) {
            if (rend != null)
                Destroy(rend);
            if (tex != null)
                Destroy(tex);
            rend = new RenderTexture(targetTexture.width, targetTexture.height, 16);
            Debug.Log(56789);
            renderCamera.targetTexture = rend;
            tex = new Texture2D(rend.width, rend.height, TextureFormat.RGB24, true);
        }
        RenderTexture.active = rend;
        tex.ReadPixels(new Rect(0,0,tex.width, tex.height), 0, 0, true);
        tex.Apply();
        
        //Debug.Log(renderCamera.pixelWidth + " - " + renderCamera.rect.width +" - "+ targetTexture.width + " - "+rend.width);
    }
    int c = 0;
    void Compare()
    {
        float d = 0;
        Color c1, c2;
        tex.Apply();
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                c1 = tex.GetPixel(i, j);
                c2 = targetTexture.GetPixel(i, j);
                tex.SetPixel(i, j, new Color( Mathf.Abs(c1.r - c2.r),0,0));
                
                d += Mathf.Abs(c1.r - c2.r);
            }
        }
        tex.Apply();
        Graphics.Blit(tex, rend);
        Debug.Log(success +"   "+ggg+"  "+(distance -d));
        distance += deflation;
        if (d <= distance)
        {
            distance = d;
            StoreVars();
            success++;
            c = 0;
        }
        else
        {
            c++;
            if (c > maxBranchSteps)
            {
                c = 0;
                RecoverVars();
            }
            //RecoverVars();
        }
    }

    void StoreVars()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            positions[i]= t.localPosition ;
            rotations[i] = t.localEulerAngles;
            scales[i] = t.localScale;
        }
    }

    void RecoverVars()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.localPosition = positions[i];
            t.localEulerAngles = rotations[i];
            t.localScale = scales[i];
        }
    }
}
