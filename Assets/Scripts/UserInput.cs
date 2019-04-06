using UnityEngine;
using System;
using UniRx;

public interface IUserInput
{
    FloatReactiveProperty CharacterMoving { get; }
    ReactiveCommand<int> CharacterShoot { get; }
}

public interface IUIInput
{
    void SetUIShootProperty(BoolReactiveProperty prop);
    void SetUIMovementProperty(FloatReactiveProperty prop);
}

public class UserInput : MonoBehaviour, IUserInput, IGameState, IUIInput
{
    public FloatReactiveProperty CharacterMoving { get { return m_Moving; } }
    public ReactiveCommand<int> CharacterShoot { get { return m_ShootInput; } }

    private ReactiveCommand<int> m_ShootInput = new ReactiveCommand<int>();
    private BoolReactiveProperty m_CanShoot = new BoolReactiveProperty(true);
    private FloatReactiveProperty m_Moving = new FloatReactiveProperty(0f);

    private BoolReactiveProperty m_UIShoot;
    private FloatReactiveProperty m_UIMove;

    private bool m_Shooting = false, m_Active = false;
    private float m_SAMoving = 0f;
    private TimeSpan m_ShootTimer;


    public void SetUIShootProperty(BoolReactiveProperty prop)
    {
#if !UNITY_EDITOR
        if (m_UIShoot != null) m_UIShoot.Dispose();
#endif
        m_UIShoot = prop;
#if !UNITY_EDITOR
        m_UIShoot.Subscribe(ChangeShootState);
#endif
    }

    public void SetUIMovementProperty(FloatReactiveProperty prop)
    {
#if !UNITY_EDITOR
        if (m_UIMove != null) m_UIMove.Dispose();
#endif
        m_UIMove = prop;
#if !UNITY_EDITOR
        m_UIMove.Subscribe(x => m_Moving.Value = x);
#endif
    }

    public void SetGameState(bool state)
    {
        m_Active = state;
    }

    public void SetNewShootTime(float time)
    {
        m_ShootTimer = TimeSpan.FromSeconds(time);
    }
    
    private void Update()
    {
        if (!m_Active) return;

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            m_SAMoving = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            m_SAMoving = 1f;
        }
        else
        {
            m_SAMoving = 0f;
        }
        
        ChangeShootState(Input.GetKey(KeyCode.Space) || m_UIShoot.Value);

        float move = m_SAMoving;
        if (m_UIMove.Value != 0f) move = m_UIMove.Value;
        m_Moving.Value = move;
#endif
    }

    private void ChangeShootState(bool state)
    {
        if (state != m_Shooting)
        {
            m_Shooting = state;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (m_Shooting && m_CanShoot.Value)
        {
            m_ShootInput.Execute(1);
            m_CanShoot.Value = false;
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SetNewShootTime(0.25f);
        m_CanShoot
            .Where(x => x)
            .Subscribe(_ => Shoot());
        m_CanShoot
            .Where(x => !x)
            .Subscribe(_ =>
            {
                //выглядит так, как будто будет забивать память новыми экземплярами
                Observable.Timer(m_ShootTimer).Subscribe(SetCanShoot);
            });
    }

    private void SetCanShoot(long x)
    {
        m_CanShoot.SetValueAndForceNotify(true);
    }
}
