using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class AbilityController : MonoBehaviour
{
    private class Slot
    {
        public AbilityData data;
        public float cooldownLeft;
        public float activeLeft;
    }

    // Phím theo thứ tự slot. Sau này có thể chuyển sang InputAction nếu cần rebind
    private static readonly Key[] SlotKeys = { Key.Digit1, Key.Digit2, Key.Digit3 };

    [SerializeField] private AbilityData[] abilities;

    private Slot[] slots;
    private AbilityContext context;

    // Audio và hiệu ứng sẽ nghe các event này
    public event Action<int> OnAbilityActivated;
    public event Action<int> OnAbilityReady;

    public int SlotCount => slots.Length;

    private void Awake()
    {
        context = new AbilityContext
        {
            Stats = GetComponent<PlayerStats>(),
            Health = GetComponent<Health>()
        };

        var unlocked = new List<AbilityData>();

        foreach (var a in abilities)
            if (PlayerProfile.IsOwnedId(a.UnlockId) && unlocked.Count < SlotKeys.Length)
                unlocked.Add(a);

        slots = new Slot[unlocked.Count];
        for (int i = 0; i < slots.Length; i++)
            slots[i] = new Slot { data = unlocked[i] };
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;   // đang tạm dừng (chọn nâng cấp) thì không nhận phím, không đếm giờ

        float dt = Time.deltaTime;
        var keyboard = Keyboard.current;

        for (int i = 0; i < slots.Length; i++)
        {
            Slot s = slots[i];

            if (s.activeLeft > 0f)
            {
                s.activeLeft -= dt;
                if (s.activeLeft <= 0f) s.data.Deactivate(context);
            }

            if (s.cooldownLeft > 0f)
            {
                s.cooldownLeft -= dt;
                if (s.cooldownLeft <= 0f)
                {
                    s.cooldownLeft = 0f;
                    OnAbilityReady?.Invoke(i);
                }
            }

            if (keyboard != null && keyboard[SlotKeys[i]].wasPressedThisFrame)
                TryActivate(i);
        }
    }

    private void TryActivate(int index)
    {
        Slot s = slots[index];
        if (s.cooldownLeft > 0f) return;   // còn cooldown (kể cả đang hiệu lực) thì bỏ qua

        s.data.Activate(context);
        s.activeLeft = s.data.Duration;
        s.cooldownLeft = s.data.Cooldown;
        OnAbilityActivated?.Invoke(index);
    }

    // HUD đọc các hàm này
    public AbilityData GetData(int index) => slots[index].data;
    public float GetCooldownRemaining(int index) => slots[index].cooldownLeft;
}