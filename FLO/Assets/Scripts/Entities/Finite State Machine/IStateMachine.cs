using System.Collections;
using System.Collections.Generic;

public interface IStateMachine
{
    bool Stun { get; set; }
    bool Launch { get; set; }
    IEnumerator ToggleStun();
    IEnumerator ToggleLaunch();
}