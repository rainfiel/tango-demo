using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tango;
using UnityEngine.UI;


public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("....................................start");
	}
	
	// Update is called once per frame
	void Update ()
    {
        TangoCameraIntrinsics ci = new TangoCameraIntrinsics();
        Tango.VideoOverlayProvider.GetIntrinsics(TangoEnums.TangoCameraId.TANGO_CAMERA_COLOR, ci);

        double w_fov = 2 * Math.Atan2(ci.width / 2, ci.fx);
        double h_fov = 2 * Math.Atan2(ci.height / 2, ci.fy);

        w_fov = w_fov * 180 / Math.PI;
        h_fov = h_fov * 180 / Math.PI;

        gameObject.GetComponent<Text>().text = string.Format(
            "width:{0} height:{1} fx:{2} fy:{3} cx:{4} cy:{5} w_fov:{6} h_fov:{7}",
            ci.width, ci.height, ci.fx, ci.fy, ci.cx, ci.cy, w_fov, h_fov);
    }
}
