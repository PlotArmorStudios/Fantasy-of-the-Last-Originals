using UnityEngine;

public class ToggleStance : MonoBehaviour
{
    private Animator _animator;
    public PlayerStance Stance { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ChangeStance();
    }

    void ChangeStance()
    {
        switch (Stance)
        {
            case PlayerStance.Stance1:
                Stance1();
                break;
            case PlayerStance.Stance2:
                Stance2();
                break;
            case PlayerStance.Stance3:
                Stance3();
                break;
            case PlayerStance.Stance4:
                Stance4();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Stance = PlayerStance.Stance1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Stance = PlayerStance.Stance2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Stance = PlayerStance.Stance3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Stance = PlayerStance.Stance4;
    }


    void Stance1()
    {
        _animator.SetBool("Stance1", true);
        _animator.SetBool("Stance2", false);
        _animator.SetBool("Stance3", false);
        _animator.SetBool("Stance4", false);
    }

    void Stance2()
    {
        _animator.SetBool("Stance1", false);
        _animator.SetBool("Stance2", true);
        _animator.SetBool("Stance3", false);
        _animator.SetBool("Stance4", false);
    }

    void Stance3()
    {
        _animator.SetBool("Stance1", false);
        _animator.SetBool("Stance2", false);
        _animator.SetBool("Stance3", true);
        _animator.SetBool("Stance4", false);
    }

    void Stance4()
    {
        _animator.SetBool("Stance1", false);
        _animator.SetBool("Stance2", false);
        _animator.SetBool("Stance3", false);
        _animator.SetBool("Stance4", true);
    }

}