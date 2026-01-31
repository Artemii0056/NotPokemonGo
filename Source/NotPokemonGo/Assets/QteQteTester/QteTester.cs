using UnityEngine;

public class QteTester : MonoBehaviour
{
    public QteSwipeHandler qte;

    void Start()
    {
        qte.OnSwipeResult = success =>
        {
            Debug.Log("QTE Result: " + (success ? "SUCCESS" : "FAIL"));
        };
    }
}