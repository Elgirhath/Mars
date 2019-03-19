using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour {
    public int maxEnergy;
    public int initEnergy;
    public float regenerationSpeed;
    public float sprintDropSpeed;
    public float jumpDropValue;
    public float secondsToUnlock;

    private PlayerController playerController;

    private bool lockedTimer;
    private float _energy;
    private bool _lockState;
    public float energy {
        get => _energy;
        set => _energy = value;
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
        energyBar = EnergyBar.instance;
        _lockState = false;
        lockedTimer = false;
        
        playerController = PlayerController.instance;
    }

    public void ChangeEnergy(float diff)
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
        
        energyBar.ChangeEnergyBar();
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
    }

    private IEnumerator LockRegeneration()
    {
        lockedTimer = true;
        yield return new WaitForSeconds(secondsToUnlock);
        LockState = false;
    }
}
