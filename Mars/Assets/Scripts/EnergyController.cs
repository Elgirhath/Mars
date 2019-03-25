using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public float maxEnergyLimit;
    public float initEnergy;
    public float regenerationSpeed;
    public float sprintDropSpeed;
    public float jumpDropValue;
    public float secondsToUnlock;
    public float energyLossInterval;
    public float maxEnergyLossMultiplier;

    private PlayerController playerController;

    private bool lockedTimer;
    private float _energy;
    private float _maxEnergy;
    private bool _lockState;
    public float energy {
        get => _energy;
        set => _energy = value;
    }

    public float maxEnergy
    {
        get => _maxEnergy;
        set => _maxEnergy = value;
    }

    public bool LockState
    {
        get => _lockState;
        set => _lockState = value;
    }

    public static EnergyController instance;
    private EnergyBar energyBar;

    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start() {
        _energy = initEnergy;
        _maxEnergy = maxEnergyLimit;
        energyBar = EnergyBar.instance;
        _lockState = false;
        lockedTimer = false;
        
        playerController = PlayerController.instance;
        
        InvokeRepeating(nameof(EnergyLoss),energyLossInterval, energyLossInterval);
    }

    private void ChangeEnergy(float diff)
    {
        if (_energy + diff <= 0)
        {
            _lockState = true;
        }
        else if (_energy + diff > maxEnergy)
        {
            _energy = maxEnergy;
        }
        else
        {
            energy += diff;
        }
        energyBar.ChangeEnergyBar(energy/maxEnergy);
        if(diff < 0)
            ChangeMaxEnergy(diff*maxEnergyLossMultiplier);
    }

    private void ChangeMaxEnergy(float diff)
    {
        if (maxEnergy + diff < 0)
        {
            maxEnergy = 0;
        }
        else if (maxEnergy + diff > maxEnergyLimit)
        {
            maxEnergy = maxEnergyLimit;
        }
        else
        {
            maxEnergy += diff;
        }
        energyBar.ChangeMaxEnergyBar(maxEnergy/maxEnergyLimit);
        
        if (maxEnergy < energy)
        {
            energy = maxEnergy;
            energyBar.ChangeEnergyBar(energy/maxEnergy);
        }
    }

    private void EnergyLoss()
    {
        ChangeMaxEnergy(-0.01f);
        Debug.Log(maxEnergy);
    }

    private void Update()
    {
        if (LockState)
        {
            if (!lockedTimer)
                StartCoroutine(LockRegeneration());
        }
        else
        {
            if (playerController.jumped)
            {
                ChangeEnergy(-jumpDropValue);
            }
            else if (!playerController.IsRunning())
            {
                if(playerController.IsGrounded())
                    ChangeEnergy(regenerationSpeed);
            }
            else
                ChangeEnergy(-sprintDropSpeed);
        }
        //Debug.Log(LockState);
    }

    private IEnumerator LockRegeneration()
    {
        lockedTimer = true;
        yield return new WaitForSeconds(secondsToUnlock);
        LockState = false;
    }
}
