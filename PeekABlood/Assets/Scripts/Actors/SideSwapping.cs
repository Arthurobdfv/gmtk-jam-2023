using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SideSwapping : MonoBehaviour
{
    public List<GhostInteraction> Ghosts { get; set; }
    public List<SoldierInteraction> Soldiers { get; set; }
    public List<PlayerMovement> ControllableCharacters { get; set; }

    private long _currentlyControlling;
    private PlayerControls _playerControls;
    private PlayerSide _currentSide;
    [SerializeField] float TimeToSwitch = 15f;
    [SerializeField] private bool _enableTimeSwitching;
    private bool EnableTimeSwitching
    {
        get => _enableTimeSwitching; 
        set
        {
            if (value != _enableTimeSwitching)
            {
                _enableTimeSwitching = value;
                OnTimeSwitchingChange(value);
            }
        }
    }

    private void OnTimeSwitchingChange(bool enable)
    {
        if (enable) StartCoroutine(SwitchSideCoroutine());
        else StopCoroutine(SwitchSideCoroutine());
    }

    public void Awake()
    {
        _playerControls = new PlayerControls();
    }

    public void OnEnable()
    {
        _playerControls.Player.SwitchSide.performed += SwitchSide;
        _playerControls.Enable();
    }

    public void OnDisable()
    {
        _playerControls.Player.SwitchSide.performed -= SwitchSide;
        _playerControls.Disable();
    }

    private void SwitchSide(InputAction.CallbackContext ctx)
    {
        SwitchSide();
    }

    private void SwitchSide()
    {
        if (_currentSide == PlayerSide.Ghost)
            SwapTo(SelectSoldier(), PlayerSide.Soldier);
        else
            SwapTo(SelectGhost(), PlayerSide.Ghost);

    }

    private void Update()
    {
        DisableAllExcept((int)_currentlyControlling);
    }

    IEnumerator SwitchSideCoroutine()
    {
        var time = 0f;
        while (true)
        {
            if (time >= TimeToSwitch)
            {
                SwitchSide();
                time = 0f;
            }
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
    }


    public void Start()
    {
        Ghosts = FindObjectsByType<GhostInteraction>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        Soldiers = FindObjectsByType<SoldierInteraction>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        ControllableCharacters = new List<PlayerMovement>();
        ControllableCharacters.AddRange(Ghosts.Select(x => x.GetComponent<PlayerMovement>()));
        ControllableCharacters.AddRange(Soldiers.Select(x => x.GetComponent<PlayerMovement>()));
        SetupInitialCharacter();
        if(EnableTimeSwitching) StartCoroutine(SwitchSideCoroutine());
    }

    private void SwapTo(int instanceId, PlayerSide newSide)
    {
        _currentSide = newSide;
        _currentlyControlling = instanceId;
        DisableAllExcept(instanceId);
    }

    private void SetupInitialCharacter()
    {
        SwapTo(SelectSoldier(), PlayerSide.Soldier);
    }

    private void DisableAllExcept(int instanceId)
    {
        foreach (var entity in ControllableCharacters)
        {
            if (entity.gameObject.GetInstanceID() == instanceId)
                EnableCharacter(entity);
            else
                DisableCharacter(entity);
        }
    }

    private void EnableCharacter(PlayerMovement plr)
    {
        plr.enabled = true;
        plr.GetComponent<AIBehaviour>().enabled = false;
        plr.GetComponent<PlayerInteraction>().enabled = true;
    }

    private void DisableCharacter(PlayerMovement plr)
    {
        plr.enabled = false;
        plr.GetComponent<AIBehaviour>().enabled = true;
        plr.GetComponent<PlayerInteraction>().enabled = false;
    }

    private int SelectSoldier()
    {
        return Soldiers[UnityEngine.Random.Range(0, Soldiers.Count)].gameObject.GetInstanceID();
    }

    private int SelectGhost()
    {
        return Ghosts[UnityEngine.Random.Range(0, Ghosts.Count)].gameObject.GetInstanceID();
    }
}

internal enum PlayerSide
{
    Ghost,
    Soldier
}
