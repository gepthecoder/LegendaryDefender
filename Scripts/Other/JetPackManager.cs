using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackManager : MonoBehaviour
{

    private static JetPackManager instance;
    public static JetPackManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(JetPackManager)) as JetPackManager;
            }
            return instance;

        }
        set { instance = value; }
    }


    public enum JETPACKS
    {
        blueThunder = 0,
                greenMamba,
                        goldenJuice,
                                conservative,
                                        doubleFurious,
                                                superTurbo,
    }

    public int currentJetpack;

    public GameObject JetpackBlueThunder;
    public GameObject JetpackGreenMamba;
    public GameObject JetpackGoldenJuice;
    public GameObject JetpackConservative;
    public GameObject JetpackDoubleFurious;
    public GameObject JetpackSuperTurbo;

    public float currentGas;

    public float blueThunderGas = 5f;
    public float greenMambaGas = 8f;
    public float goldenJuiceGas = 12f;
    public float conservativeGas = 20f;
    public float doubleFuriousGas = 35f;
    public float superTurboGas = 60f;


    public void Awaik()
    {
        Debug.Log("start AWAIK");

        instance = this;

        if (PlayerPrefs.HasKey("jetpack"))
        {
            // we had a previous session
            currentJetpack = PlayerPrefs.GetInt("jetpack", 0);
            Debug.Log("AWAIK current jetpack: " + currentJetpack);
        }
        else
        {
            Debug.Log("AWAIK save :O ");
            SavePrefs();
        }

        Debug.Log("end AWAIK");
    }


    void Start()
    {
        START_SET_JETPACK(PlayerPrefs.GetInt("jetpack", 0));
        Debug.Log("current jetpack: " + currentJetpack);
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("jetpack", currentJetpack);
        PlayerPrefs.Save();
    }

    public void START_SET_JETPACK(int jetpack)
    {
        switch (jetpack)
        {
            case (int)JetPackManager.JETPACKS.blueThunder:
                currentGas = blueThunderGas;
                Debug.Log("BLUE THUNDER!!");
                JetpackBlueThunder.SetActive(true);
                JetpackGreenMamba.SetActive(false);
                JetpackGoldenJuice.SetActive(false);
                JetpackConservative.SetActive(false);
                JetpackDoubleFurious.SetActive(false);
                JetpackSuperTurbo.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.greenMamba:
                currentGas = greenMambaGas;

                JetpackBlueThunder.SetActive(false);
                JetpackGreenMamba.SetActive(true);
                JetpackGoldenJuice.SetActive(false);
                JetpackConservative.SetActive(false);
                JetpackDoubleFurious.SetActive(false);
                JetpackSuperTurbo.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.goldenJuice:
                currentGas = goldenJuiceGas;

                JetpackBlueThunder.SetActive(false);
                JetpackGreenMamba.SetActive(false);
                JetpackGoldenJuice.SetActive(true);
                JetpackConservative.SetActive(false);
                JetpackDoubleFurious.SetActive(false);
                JetpackSuperTurbo.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.conservative:
                currentGas = conservativeGas;

                JetpackBlueThunder.SetActive(false);
                JetpackGreenMamba.SetActive(false);
                JetpackGoldenJuice.SetActive(false);
                JetpackConservative.SetActive(true);
                JetpackDoubleFurious.SetActive(false);
                JetpackSuperTurbo.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.doubleFurious:
                currentGas = doubleFuriousGas;

                JetpackBlueThunder.SetActive(false);
                JetpackGreenMamba.SetActive(false);
                JetpackGoldenJuice.SetActive(false);
                JetpackConservative.SetActive(false);
                JetpackDoubleFurious.SetActive(true);
                JetpackSuperTurbo.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.superTurbo:
                currentGas = superTurboGas;

                JetpackBlueThunder.SetActive(false);
                JetpackGreenMamba.SetActive(false);
                JetpackGoldenJuice.SetActive(false);
                JetpackConservative.SetActive(false);
                JetpackDoubleFurious.SetActive(false);
                JetpackSuperTurbo.SetActive(true);
                break;
        }

        Controller.instance.SET_MAX_FUEL(currentGas);

    }

    public void SET_JETPACK(int jetpack)
    {
        switch (jetpack)
        {
            case (int)JetPackManager.JETPACKS.blueThunder:
                currentGas = blueThunderGas;
                int a = currentJetpack;
                currentJetpack = (int)JetPackManager.JETPACKS.blueThunder;
                SWITCH_JETPACK(a, currentJetpack);
                SavePrefs();
                break;
            case (int)JetPackManager.JETPACKS.greenMamba:
                currentGas = greenMambaGas;
                int b = currentJetpack;
                currentJetpack = (int)JetPackManager.JETPACKS.greenMamba;
                SWITCH_JETPACK(b, currentJetpack);
                SavePrefs();
                break;
            case (int)JetPackManager.JETPACKS.goldenJuice:
                currentGas = goldenJuiceGas;
                int c = currentJetpack;
                currentJetpack = (int)JetPackManager.JETPACKS.goldenJuice;
                SWITCH_JETPACK(c, currentJetpack);
                SavePrefs();
                break;
            case (int)JetPackManager.JETPACKS.conservative:
                currentGas = conservativeGas;
                int d = currentJetpack;
                currentJetpack = (int)JetPackManager.JETPACKS.conservative;
                SWITCH_JETPACK(d, currentJetpack);
                SavePrefs();
                break;
            case (int)JetPackManager.JETPACKS.doubleFurious:
                currentGas = doubleFuriousGas;
                int e = currentJetpack;
                currentJetpack = (int)JetPackManager.JETPACKS.doubleFurious;
                SWITCH_JETPACK(e, currentJetpack);
                SavePrefs();
                break;
            case (int)JetPackManager.JETPACKS.superTurbo:
                currentGas = superTurboGas;
                int f = currentJetpack;
                currentJetpack = (int)JetPackManager.JETPACKS.superTurbo;
                SWITCH_JETPACK(f, currentJetpack);
                SavePrefs();
                break;
        }

        Debug.Log("Current gas: " + currentGas);
        Controller.instance.SET_MAX_FUEL(currentGas);
    }
    private void SWITCH_JETPACK(int previousJ, int currentJ)
    {
        // hide previous jetpack
        switch (previousJ)
        {
            case (int)JetPackManager.JETPACKS.blueThunder:
                JetpackBlueThunder.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.greenMamba:
                JetpackGreenMamba.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.goldenJuice:
                JetpackGoldenJuice.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.conservative:
                JetpackConservative.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.doubleFurious:
                JetpackDoubleFurious.SetActive(false);
                break;
            case (int)JetPackManager.JETPACKS.superTurbo:
                JetpackSuperTurbo.SetActive(false);
                break;
        }

        // show current jetpack

        switch (currentJ)
        {
            case (int)JetPackManager.JETPACKS.blueThunder:
                JetpackBlueThunder.SetActive(true);
                break;
            case (int)JetPackManager.JETPACKS.greenMamba:
                JetpackGreenMamba.SetActive(true);
                break;
            case (int)JetPackManager.JETPACKS.goldenJuice:
                JetpackGoldenJuice.SetActive(true);
                break;
            case (int)JetPackManager.JETPACKS.conservative:
                JetpackConservative.SetActive(true);
                break;
            case (int)JetPackManager.JETPACKS.doubleFurious:
                JetpackDoubleFurious.SetActive(true);
                break;
            case (int)JetPackManager.JETPACKS.superTurbo:
                JetpackSuperTurbo.SetActive(true);
                break;
        }

    }
}
