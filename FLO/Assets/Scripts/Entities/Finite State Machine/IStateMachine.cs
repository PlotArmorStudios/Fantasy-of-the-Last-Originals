using System.Collections;
using System.Collections.Generic;

public interface IStateMachine
{
    bool Hitstun { get; set; }
    bool Launch { get; set; }
    IEnumerator SetStunFalse();
}