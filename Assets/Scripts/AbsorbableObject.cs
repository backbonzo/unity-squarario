using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbableObject : MonoBehaviour
{
    [Tooltip("Enables destroy other object if this object bigger")]
    public bool DestroyOtherObject = true;

    [Tooltip("Starting Mass")]
    public float Mass = 1.0f;

    [Tooltip("Efficiency of absorbing other object mass")]
    public float FactorGainMass = 0.75f;

    void FixedUpdate()
    {
        transform.root.localScale = new Vector3(Mass, Mass, Mass);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Absorbable")
            return;

        AbsorbableObject temp = other.gameObject.transform.root.gameObject.GetComponent<AbsorbableObject>();

        if (temp == null)
            return;

        Debug.Log($"{Mass} > {temp.Mass}");
        if (Mass > temp.Mass && DestroyOtherObject)
        {
            Mass += temp.Mass * FactorGainMass;
            Destroy(other.gameObject.transform.root.gameObject);
        }
    }

}
