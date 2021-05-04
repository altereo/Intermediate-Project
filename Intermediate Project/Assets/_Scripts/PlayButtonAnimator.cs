using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonAnimator : MonoBehaviour
{
    public bool ShouldAnimateDown = false;
    public bool HasAnimatedDown = false;
    public float m_AnimationSpeed = 10; // How fast we should animate down.

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 500, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // If/when this flag is changed by another script, begin animating this down.
        // we're doing it this way because for some reason Unity doesn't support
        // animating UI elements.
        if (ShouldAnimateDown) {
            if (transform.localPosition.y > 0f && !HasAnimatedDown) {
                var nPos = transform.position;
                nPos.z = nPos.x = 0;
                nPos.y = m_AnimationSpeed * Time.deltaTime;
                transform.position -= nPos;
            } else if (transform.localPosition.y < 0f) {
                HasAnimatedDown = true;
                transform.position += Vector3.up;
            } else if (transform.localPosition.y > 0f && HasAnimatedDown) {
                transform.position += Vector3.down;
            } 
            if (Mathf.RoundToInt(transform.localPosition.y) == 0) {
                ShouldAnimateDown = false;
            }
        }
    }
}
