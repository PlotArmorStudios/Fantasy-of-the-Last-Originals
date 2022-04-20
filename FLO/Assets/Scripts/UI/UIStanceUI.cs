using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIStanceUI : MonoBehaviour
{
    //Stance Icon variables
    [FormerlySerializedAs("stanceTogglerToggler")] [SerializeField] private PlayerStanceToggler _stanceToggler;
    [SerializeField] Sprite m_stanceImage1, m_stanceImage2, m_stanceImage3, m_stanceImage4;
    [FormerlySerializedAs("m_mainStanceImage")] [SerializeField] Image _mainStanceImage;

    [FormerlySerializedAs("m_controlPanel")] public GameObject _controlPanel;
    private CombatManager _combatManager;

    // Start is called before the first frame update
    void Start()
    {
        _mainStanceImage.sprite = m_stanceImage1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!_controlPanel.activeInHierarchy)
            {
                _controlPanel.SetActive(true);
            }
            else if (_controlPanel.activeInHierarchy)
            {
                _controlPanel.SetActive(false);
            }
        }
        if (_stanceToggler.Stance == PlayerStance.Stance1)
        {
            _mainStanceImage.sprite = m_stanceImage1;
        }
        if (_stanceToggler.Stance == PlayerStance.Stance2)
        {
            _mainStanceImage.sprite = m_stanceImage2;
        }
        if (_stanceToggler.Stance == PlayerStance.Stance3)
        {
            _mainStanceImage.sprite = m_stanceImage3;
        }
        if (_stanceToggler.Stance == PlayerStance.Stance4)
        {
            _mainStanceImage.sprite = m_stanceImage4;
        }

    }
}
