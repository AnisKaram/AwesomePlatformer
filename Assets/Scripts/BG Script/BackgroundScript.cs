using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        // SpriteRenderer to give us the background in unity world space
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3 (1, 1, 1);

        // Give us the width of the backgorund we already assigned
        float width = sr.sprite.bounds.size.x;

        // Give us the height of the backgorund we already assigned
        float height = sr.sprite.bounds.size.y;

        // height is = Orthograpgic size, which is half of the size (size if 10, orthographic size is 5 * 2 = 10)
        // Camera.main : will refer to the main camera that is tagged with "MainCamera"
        float worldScreenHeight = Camera.main.orthographicSize * 2f;

        // Screen height & width is the actual size in game (1280 * 720) and we will divide it by the orthographic size
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 tempScale = transform.localScale;
        tempScale.x = worldScreenWidth / width + 0.3f;
        tempScale.y = worldScreenHeight / height + 0.3f;

        transform.localScale = tempScale;
    }
}
