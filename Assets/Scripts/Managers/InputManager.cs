using UnityEngine;

public static class InputManager
{
    private static Controls controls;

    public static void Init(Player player)
    {
        // Hide and confine the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        controls = new Controls();

        // Subscribe to movement event
        controls.Game.Movement.performed += ctx =>
        {
            player.SetMovementDirection(ctx.ReadValue<Vector3>());
        };

        // Subscribe to jump event
        controls.Game.Jump.started += ctx =>
        {
            player.Jump();
        };

        // Subscribe to look event
        controls.Game.Look.performed += ctx =>
        {
            player.SetLookRotation(ctx.ReadValue<Vector2>());
        };

        // Subscribe to shoot event
        controls.Game.Shoot.started += ctx =>
        {
            player.Shoot();
        };

        // Subscribe to reload event
        controls.Game.Reload.performed += ctx =>
        {
            player.Reload();
        };
        controls.Game.SwitchWeapon.performed += ctx =>
        {
            player.SwitchWeapon();
        };

        // Enable controls
        controls.Permanent.Enable();
    }

    public static void GameMode()
    {
        // Enable game controls and disable UI controls
        controls.Game.Enable();
        controls.UI.Disable();
    }

    public static void UIMode()
    {
        // Disable game controls and enable UI controls
        controls.Game.Disable();
        controls.UI.Enable();
    }
}