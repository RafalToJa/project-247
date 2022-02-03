using UnityEngine;
using UnityEngine.UI;

public class AmmoCountUI : MonoBehaviour
{
    public Text _ammoCount;

    private void Update()
    {
        if (Cannon.Instance.unlimAmmoTime <= 0)
        {
            _ammoCount.text = Cannon.Instance.ammunitionCount.ToString();
        }
        else
        {
            _ammoCount.text = "Inf";
        }
    }


}
