using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol : MonoBehaviour
{
    public int ammoCount;
    public int maxAmmoCount;
    public List<Image> bulletImages;
    [SerializeField] Transform bullets;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] CombatButton combatButton;

    private void Start()
    {
        ammoCount = maxAmmoCount;
        for (int i = 0; i < maxAmmoCount; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, bullets);
            bulletImages.Add(newBullet.GetComponent<Image>());
        }

        PlayerController.instance.onActionReady.AddListener(ConsumeAmmo);
    }

    public void ConsumeAmmo()
    {
        if (!PlayerController.instance.currentAction.useAmmo) { return; }
        if (ammoCount == 0) { return; }

        ammoCount = Mathf.Clamp(ammoCount - 1, 0, maxAmmoCount);
        int ammoIndex = ammoCount;

        bulletImages[ammoIndex].color = new Color32(100, 100, 100, 255);

        if (ammoCount == 0)
        {
            DisableActionButton();
        }
    }

    public void DisableActionButton()
    {
        combatButton.DisableButton();
    }
}
