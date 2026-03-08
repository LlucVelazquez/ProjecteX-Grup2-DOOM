public interface IRefillable
{
    public WeaponType Weapon { get; }
    public int CurrentAmmunition { get; }

    public void AddAmmunition(int ammunition);
}

public enum WeaponType { Shotgun = 0 }