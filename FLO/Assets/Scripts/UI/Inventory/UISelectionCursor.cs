using Skills;
using UnityEngine;
using UnityEngine.UI;

public class UISelectionCursor : MonoBehaviour
{
    [SerializeField] private Image _image;
    
    private UIInventoryPanel _inventoryPanel;
    private UISkillsPanel _skillInventoryPanel;
    private UISkillTree _uiSkillTree;
    public bool IconVisible => _image != null && _image.sprite != null && _image.enabled;
    public Sprite Icon => _image.sprite;

    private void Awake()
    {
        _inventoryPanel = FindObjectOfType<UIInventoryPanel>();
        _skillInventoryPanel = FindObjectOfType<UISkillsPanel>();
       // _uiSkillTree = FindObjectOfType<UISkillTree>();
        _image.enabled = false;
    }

    private void OnEnable()
    {
    _inventoryPanel.OnSelectionChanged += HandleSelectionChanged;
    _skillInventoryPanel.OnSelectionChanged += HandleSelectionChanged;
   // _uiSkillTree.OnSelectionChanged += HandleSelectionChanged;
    }

    private void OnDisable()
    {
    _inventoryPanel.OnSelectionChanged -= HandleSelectionChanged;
    _skillInventoryPanel.OnSelectionChanged -= HandleSelectionChanged;
 //   _uiSkillTree.OnSelectionChanged -= HandleSelectionChanged;
        
    }

    private void Update()
    {
        transform.position = PlayerInput.Instance.MousePosition;
    }

    private void HandleSelectionChanged()
    {
        if (_inventoryPanel.Selected)
            _image.sprite = _inventoryPanel.Selected.Icon;
        else
            _image.sprite = null;
        
        if (_skillInventoryPanel.Selected)
            _image.sprite = _skillInventoryPanel.Selected.Icon;
        else
            _image.sprite = null;
        
        _image.sprite = _inventoryPanel.Selected ? _inventoryPanel.Selected.Icon : null;
        _image.enabled = _image.sprite != null;
    }
}