using UnityEngine;
using System.Collections;

public class ObeyMovement : MonoBehaviour {

    public enum ObeyMovementType
    {
        Center,
        Mouse,
        Circle,
        Random
    }

    public ObeyMovementType movement;
    public float smoothing = .1f;
    Vector3 target;
    float phase = 0;

	// Use this for initialization
	void Start ()
    {
        Messenger.AddListener("b7", () => { movement = ObeyMovementType.Center; });
        Messenger.AddListener("b15", () => { movement = ObeyMovementType.Circle; });
        Messenger.AddListener("b23", () => { movement = ObeyMovementType.Mouse; });
        Messenger.AddListener("b22", () => { movement = ObeyMovementType.Random; });
        Messenger.AddListener("r6", () => {
            if (movement == ObeyMovementType.Random) {
                Vector3 v = target;
                while (Vector3.Distance(v, target) < 1)
                    v = new Vector3(Random.value * 6 - 3, Random.value * 4 - 2, 0);
                target = v;
            }
            else if (movement == ObeyMovementType.Circle)
                phase += 1;
        });
    }
	
	// Update is called once per frame
	void Update () {
        phase += Time.deltaTime;
        if (movement == ObeyMovementType.Center)
            target = Vector3.zero;
        else if (movement == ObeyMovementType.Circle)
            target = new Vector3(Mathf.Cos(phase), Mathf.Sin(phase), 0)*2;
        else if (movement == ObeyMovementType.Mouse)
            target = new Vector3(Input.mousePosition.x/Screen.width*6-3, Input.mousePosition.y / Screen.height * 4-2, 0);
        
        transform.position = Vector3.Lerp(transform.position, target, smoothing);
	}
}
