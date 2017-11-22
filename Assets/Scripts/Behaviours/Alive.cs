using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Antique))]
public class Alive : MonoBehaviour
{
    Antique antique;
    // Use this for initialization
    void Start()
    {
        antique = GetComponent<Antique>();
    }

    Vector3 localPosition;
    public Vector2 speed;
    bool alive = true;
    float timeDead = 0;
    float DECOMPOSITION_TIME = 3600 * 24 * 30;
    // Update is called once per frame
    void Update()
    {
        if (antique.currentAge < antique.maxAge)
        {
            if (Random.value < 0.1f * speed.x)
            {
                float movement = Random.Range(-1.0f, 1.0f);
                localPosition = transform.localPosition;
                localPosition.x += movement * speed.x;
                GetComponent<SpriteRenderer>().flipX = movement > 0;
            }
            if (Random.value < 0.1f)
            {
                localPosition += new Vector3(0, Random.Range(-0.5f, 5) * speed.y, 0);
            }
            if (localPosition != Vector3.zero)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, localPosition, Time.deltaTime);

            }
        }
        else if (alive)
        {
            alive = false;
            GetComponent<SpriteRenderer>().flipY = true;
        }
        if (!alive)
        {
            timeDead += TimeHandler.DeltaTime;
            if(timeDead> DECOMPOSITION_TIME)
            {
                GameObject.DestroyImmediate(this.gameObject);
            }
            else
            {
                Vector3 scale = this.transform.localScale;
                scale.y = Mathf.Lerp(scale.y, 0, timeDead / DECOMPOSITION_TIME);
                this.transform.localScale = scale;
            }
        }
    }
}
