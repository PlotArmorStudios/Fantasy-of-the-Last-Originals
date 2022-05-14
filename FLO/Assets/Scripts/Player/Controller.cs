using UnityEngine;

public class Controller : MonoBehaviour
{
    public int Index { get; private set; }
    public bool IsAssigned { get; set; }

    //Attack inputs
    public bool Attack { get; private set; }

    //Special Attack Inputs
    public bool SpecialAttack1 { get; private set; }
    public bool SpecialAttack2 { get; private set; }
    public bool SpecialAttack3 { get; private set; }
    public bool SpecialAttack4 { get; private set; }
    public bool SpecialAttack5 { get; private set; }
    public bool SpecialAttack6 { get; private set; }
    public bool SpecialAttack7 { get; private set; }
    public bool SpecialAttack8 { get; private set; }
    public bool SpecialAttack9 { get; private set; }
    public bool SpecialAttack10 { get; private set; }


    //Attack input strings
    private string _lightAttackButton;

    //Special Attack input strings
    private string _specialAttackButton1;
    private string _specialAttackButton2;
    private string _specialAttackButton3;
    private string _specialAttackButton4;
    private string _specialAttackButton5;
    private string _specialAttackButton6;
    private string _specialAttackButton7;
    private string _specialAttackButton8;
    private string _specialAttackButton9;
    private string _specialAttackButton10;

    private void Update()
    {
        if (!string.IsNullOrEmpty(_lightAttackButton))
        {
            Attack = Input.GetButtonDown(_lightAttackButton);
            
            SpecialAttack1 = Input.GetButtonDown(_specialAttackButton1);
            SpecialAttack2 = Input.GetButtonDown(_specialAttackButton2);
            SpecialAttack3 = Input.GetButtonDown(_specialAttackButton3);
            SpecialAttack4 = Input.GetButtonDown(_specialAttackButton4);
            SpecialAttack5 = Input.GetButtonDown(_specialAttackButton5);
            SpecialAttack6 = Input.GetButtonDown(_specialAttackButton6);
            SpecialAttack7 = Input.GetButtonDown(_specialAttackButton7);
            SpecialAttack8 = Input.GetButtonDown(_specialAttackButton8);
            SpecialAttack9 = Input.GetButtonDown(_specialAttackButton9);
            SpecialAttack10 = Input.GetButtonDown(_specialAttackButton10);
        }
    }

    public void SetIndex(int index)
    {
        Index = index;
        
        _lightAttackButton = $"Light Attack {Index}";

        _specialAttackButton1 = $"Special Attack {Index} {1}";
        _specialAttackButton2 = $"Special Attack {Index} {2}";
        _specialAttackButton3 = $"Special Attack {Index} {3}";
        _specialAttackButton4 = $"Special Attack {Index} {4}";
        _specialAttackButton5 = $"Special Attack {Index} {5}";
        _specialAttackButton6 = $"Special Attack {Index} {6}";
        _specialAttackButton7 = $"Special Attack {Index} {7}";
        _specialAttackButton8 = $"Special Attack {Index} {8}";
        _specialAttackButton9 = $"Special Attack {Index} {9}";
        _specialAttackButton10 = $"Special Attack {Index} {10}";

        gameObject.name = $"Controller {Index}";
    }

    public bool AnyButtonDown()
    {
        return Attack;
    }
}