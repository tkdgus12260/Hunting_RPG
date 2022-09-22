public delegate void HealthDelegate();
public delegate void LevelDelegate();
public delegate void ExpDelegate();

public interface IStatus
{
    float HP { get; }
    float MaxHP { get; }

    float Level { get; }

    float EXP { get; }
    float MaxEXP { get; }

    HealthDelegate OnHealthChange { get; set; }
    LevelDelegate OnLevelChange { get; set; }
    ExpDelegate OnExpChange { get; set; }
}
