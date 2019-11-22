using UnityEngine;


public class MyUnitySingleton : MonoBehaviour
{
    private static MyUnitySingleton instance = null;

    public static MyUnitySingleton Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        transform.SetParent(null);
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
// any other methods you need