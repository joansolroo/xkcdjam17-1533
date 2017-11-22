using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    int LayerIgnoreRaycast = 2;
    int LayerAntique = 8;
    int LayerMarket = 9;
    int layerDialog = 10;
    int layerZone = 11;
    int zoneAntiquarian = 13;


    public GameObject antiquarian;
    // Use this for initialization
    void Start()
    {

    }

    Transform child = null;
    Transform childsParent = null;
    int childsLayer = 0;
    Vector3 childsPos;
    Quaternion childsRot;

    public GameObject sellNote;
    // Update is called once per frame
    void Update()
    {

        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            child = null;
            childsParent = null;
            childsLayer = 0;
            childsPos = Vector3.zero;
            childsRot = Quaternion.identity;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerAntique && Antiquarian.instance.CanWork())
                {
                    Debug.Log("Antique: " + hit.collider.gameObject.name);
                    child = hit.transform;
                    childsPos = child.position;
                    childsRot = child.rotation;
                    childsParent = hit.transform.parent;

                    child.parent = this.transform;
                    child.GetComponent<Rigidbody2D>().isKinematic = true;
                    childsLayer = hit.collider.gameObject.layer;
                    hit.collider.gameObject.layer = LayerIgnoreRaycast;
                    sellNote.SetActive(true);
                }
                else if (hit.collider.gameObject.layer == zoneAntiquarian)
                {
                    if(!Antiquarian.instance.here)
                    {
                        Antiquarian.ComeBack();
                    }
                    Debug.Log("CLICK ANTIQUARIAN: " + hit.collider.gameObject.name);
                }
                else
                {
                    Debug.Log("CLICK : " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("CLICK : null");
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {

            if (child != null)
            {
                bool undo = true;
                bool drop = false;
                int layerMask = LayerAntique | LayerMarket | layerZone;

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer == LayerAntique || hit.collider.gameObject.layer == layerZone)
                    {
                        if (childsLayer == LayerAntique)
                        {
                            Debug.Log("Dropping: " + hit.collider.gameObject.name);
                            child.gameObject.layer = childsLayer;
                        }

                        else if (childsLayer == LayerMarket)
                        {
                            Debug.Log("buy: " + hit.collider.gameObject.name);
                        }
                        drop = true;
                        undo = false;
                    }
                    if (hit.collider.gameObject.layer == LayerMarket)
                    {
                        Debug.Log("sell: " + hit.collider.gameObject.name);
                        Wallet.instance.money += child.gameObject.GetComponent<Antique>().resellValue;
                        GameObject.DestroyImmediate(child.gameObject);
                        undo = false;
                    }

                    else
                    {
                        Debug.Log("error: " + hit.collider.gameObject.name);
                    }
                }
                if (drop)
                {
                    child.parent = childsParent.transform;
                    child.GetComponent<Rigidbody2D>().isKinematic = false;

                }
                if (undo)
                {
                    child.parent = childsParent.transform;
                    child.position = childsPos;
                    child.rotation = childsRot;
                    child.GetComponent<Rigidbody2D>().isKinematic = false;
                    child.gameObject.layer = childsLayer;
                    Debug.Log("Undoing: " + child.gameObject.name + ":::" + hit!=null&&hit.collider != null ? hit.collider.name : "");
                }
                sellNote.SetActive(false);
            }
        }
    }
}
